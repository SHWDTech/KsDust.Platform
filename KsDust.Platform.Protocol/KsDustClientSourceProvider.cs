using System.Linq;
using Dust.Platform.Storage.Repository;
using SHWDTech.Platform.ProtocolService;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;

namespace KsDust.Platform.Protocol
{
    class KsDustClientSourceProvider : IClientSourceProvider
    {
        private readonly KsDustDbContext _ctx;

        public KsDustClientSourceProvider()
        {
            _ctx = new KsDustDbContext();
        }

        public IClientSource GetClientSource(object nodeId)
        {
            var device = _ctx.KsDustDevices.FirstOrDefault(dev => dev.NodeId == nodeId.ToString());
            if (device == null) return null;
            var clientSource = new KsDustClientSource
            {
                ClientIdentity = $"{device.Id}",
                BusinessName = Properties.Resource.BusinessName
            };

            return clientSource;
        }
    }
}
