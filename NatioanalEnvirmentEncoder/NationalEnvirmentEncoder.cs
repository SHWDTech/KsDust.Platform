using System;
using System.Linq;
using System.Text;
using SHWDTech.Platform.ProtocolService.DataBase;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding.Generics;

namespace SHWDTech.Platform.NatioanalEnvirmentEncoder
{
    public class NationalEnvirmentEncoder : ProtocolEncoder
    {
        public override IProtocolPackage Decode(byte[] bufferBytes)
            => Decode(bufferBytes, Protocol);

        public override IProtocolPackage Decode(byte[] bufferBytes, IProtocol protocol)
        {
            var protocolString = Encoding.ASCII.GetString(bufferBytes);

            var package = new StringProtocolPackage { Protocol = protocol, ReceiveDateTime = DateTime.Now };

            var structures = protocol.ProtocolStructures.ToList();

            var currentIndex = 0;

            var knownStructureLength = structures.Sum(protocolStructure => protocolStructure.StructureDataLength);

            for (var i = 0; i < structures.Count; i++)
            {
                var structure = structures.First(obj => obj.StructureIndex == i);

                var componentDataLength = structure.StructureName == StructureNames.Data && structure.StructureDataLength == 0
                    ? int.Parse(package["ContentLength"].ComponentContent) - knownStructureLength + 12
                    : structure.StructureDataLength;

                if (currentIndex + componentDataLength > protocolString.Length)
                {
                    package.Status = PackageStatus.NoEnoughBuffer;
                    return package;
                }

                if (structure.StructureName == StructureNames.Data)
                {
                    DetectCommand(package, protocol);
                    if (package.Command == null)
                    {
                        package.Status = PackageStatus.InvalidHead;
                        return package;
                    }
                    componentDataLength = package.Command.ReceiveBytesLength == 0 ? componentDataLength : package.Command.ReceiveBytesLength;
                }

                var component = new PackageComponent<string>
                {
                    ComponentName = structure.StructureName,
                    DataType = structure.DataType,
                    ComponentIndex = structure.StructureIndex,
                    ComponentContent = protocolString.Substring(currentIndex, componentDataLength)
                };

                component.ComponentValue = ParseStructureValue(component.ComponentContent);

                currentIndex += componentDataLength;

                package[structure.StructureName] = component;
            }

            DecodeCommand(package);

            return package;
        }

        protected override void DecodeCommand(IProtocolPackage<string> package)
        {
            var container = package[StructureNames.Data].ComponentContent;

            container = container.Replace("CP=&&", string.Empty).Replace("&&", string.Empty);

            var sectionGroups = container.Split(';');
            var commandDataDicts = (from dataGroup in sectionGroups
                                    from data in dataGroup.Split(',')
                                    select data.Split('='))
                                .ToDictionary(dataKeyValuePair => dataKeyValuePair[0], dataKeyValuePair => dataKeyValuePair[1]);

            foreach (var commandDataDic in commandDataDicts)
            {
                var commandData = package.Command.CommandDatas.FirstOrDefault(obj => obj.DataName == commandDataDic.Key);
                if (commandData == null) continue;
                var component = new PackageComponent<string>
                {
                    ComponentName = commandDataDic.Key,
                    DataType = commandData.DataConvertType,
                    ComponentIndex = commandData.DataIndex,
                    ComponentContent = commandDataDic.Value
                };
                component.ComponentValue = component.ComponentContent;

                package.AppendData(component);
            }

            package.DeviceNodeId = package[StructureNames.NodeId].ComponentValue;

            package.Finalization();
        }

        protected override void DetectCommand(IProtocolPackage<string> package, IProtocol matchedProtocol)
        {
            foreach (var command in matchedProtocol.ProtocolCommands.Where(command =>
            (package[StructureNames.CmdType].ComponentValue == Encoding.ASCII.GetString(command.CommandTypeCode))
            && (package[StructureNames.CmdByte].ComponentValue == Encoding.ASCII.GetString(command.CommandCode))))
            {
                package.Command = command;
            }
        }

        private static string ParseStructureValue(string structureString)
        {
            if (structureString.Contains("CP"))
            {
                return structureString;
            }

            if (structureString.Contains(";"))
            {
                structureString = structureString.Replace(";", string.Empty);
            }

            if (structureString.Contains("="))
            {
                structureString = structureString.Split('=')[1];
            }

            return structureString;
        }
    }
}
