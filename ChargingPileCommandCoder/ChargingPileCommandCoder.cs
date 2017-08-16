using System;
using System.Linq;
using SHWDTech.Platform.ProtocolService.DataBase;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding.Generics;
using SHWDTech.Platform.Utility;

namespace SHWDTech.Platform.ChargingPileCommandCoder
{
    public class ChargingPileCommandCoder : ProtocolEncoder<byte[]>
    {
        public override IProtocolPackage Decode(byte[] bufferBytes)
            => Decode(bufferBytes, Protocol);

        public override IProtocolPackage Decode(byte[] bufferBytes, IProtocol protocol)
        {
            var package = new ChargingPileProtocolPackage { Protocol = protocol, ReceiveDateTime = DateTime.Now };

            var structures = protocol.ProtocolStructures.ToList();

            var currentIndex = 0;

            for (var i = 0; i < structures.Count; i++)
            {
                var structure = structures.First(obj => obj.StructureIndex == i);

                var componentDataLength = structure.StructureName == StructureNames.Data && structure.StructureDataLength == 0
                    ? Globals.BytesToInt16(package["ContentLength"].ComponentContent, 0, true)
                    : structure.StructureDataLength;

                if (currentIndex + componentDataLength > bufferBytes.Length)
                {
                    package.Status = PackageStatus.NoEnoughBuffer;
                    return package;
                }

                if (structure.StructureName == StructureNames.Data)
                {
                    DetectCommand(package, protocol);
                    if (package.Command == null)
                    {
                        package.Status = PackageStatus.InvalidCommand;
                        return package;
                    }
                    componentDataLength = package.Command.ReceiveBytesLength == 0 ? componentDataLength : package.Command.ReceiveBytesLength;
                }

                var component = new PackageComponent<byte[]>
                {
                    ComponentName = structure.StructureName,
                    DataType = structure.DataType,
                    ComponentIndex = structure.StructureIndex,
                    ComponentContent = bufferBytes.SubBytes(currentIndex, currentIndex + componentDataLength)
                };

                currentIndex += componentDataLength;

                package[structure.StructureName] = component;
            }

            DecodeCommand(package);

            return package;
        }

        protected override void DecodeCommand(IProtocolPackage<byte[]> package)
        {
            var currentIndex = 0;

            var container = package[StructureNames.Data].ComponentContent;

            for (var i = 0; i < package.Command.CommandDatas.Count; i++)
            {
                var data = package.Command.CommandDatas.First(obj => obj.DataIndex == i);

                if (currentIndex + data.DataLength > container.Length)
                {
                    package.Status = PackageStatus.NoEnoughBuffer;
                    return;
                }

                var component = new PackageComponent<byte[]>
                {
                    ComponentName = data.DataName,
                    DataType = data.DataConvertType,
                    ComponentIndex = data.DataIndex,
                    ComponentContent = container.SubBytes(currentIndex, currentIndex + data.DataLength)
                };

                currentIndex += data.DataLength;

                package.AppendData(component);
            }
            if (package.Command.CommandTypeCode[0] == 0xF1 && package.Command.CommandCode[0] == 0x06)
            {
                package.DeviceNodeId =
                    $"{Globals.BytesToUint64(package[StructureNames.Data].ComponentContent, 0, false)}";
            }

            package.Finalization();
        }

        protected override void DetectCommand(IProtocolPackage<byte[]> package, IProtocol matchedProtocol)
        {
            foreach (var command in matchedProtocol.ProtocolCommands.Where(command =>
                (package[StructureNames.CmdType].ComponentContent.SequenceEqual(command.CommandTypeCode))
                && (package[StructureNames.CmdByte].ComponentContent.SequenceEqual(command.CommandCode))))
            {
                package.Command = command;
            }
        }
    }
}
