using SHWDTech.Platform.ProtocolService;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;

namespace NatioanalEnvirmentEncoder
{
    public class NatioanalEnvirmentEncoder : IProtocolEncoder
    {
        public IProtocolPackage Decode(byte[] bufferBytes)
        {
            throw new System.NotImplementedException();
        }

        public void Delive(IProtocolPackage package, IActiveClient client)
        {
            throw new System.NotImplementedException();
        }
    }
}
