using Dust.Platform.Storage.Model;
using SHWDTech.Platform.ProtocolService;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;

namespace KsDust.Platform.Protocol
{
    public class KsDustClientSource : IClientSource
    {
        public KsDustDevice Device { get; }

        public KsDustClientSource(string businessName, KsDustDevice device)
        {
            BusinessName = businessName;
            Device = device;
        }

        public string ClientIdentity => Device.Id.ToString();

        public string BusinessName { get; }

        public IProtocolEncoder ProtocolEncoder { get; set; }
    }
}
