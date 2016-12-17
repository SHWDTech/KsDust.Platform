using SHWDTech.Platform.ProtocolService;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;

namespace KsDust.Platform.Protocol
{
    public class KsDustClientSource : IClientSource
    {
        public string ClientIdentity { get; set; }

        public string BusinessName { get; set; }

        public IProtocolEncoder ProtocolEncoder { get; set; }
    }
}
