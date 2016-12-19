using System.Linq;
using Dust.Platform.Storage.Repository;
using SHWDTech.Platform.ProtocolService;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;

namespace KsDust.Platform.Protocol
{
    public class KsDustClientSourceProvider : IClientSourceProvider
    {
        public IClientSource GetClientSource(object nodeId)
        {
            var ctx = new KsDustDbContext();
            var device = ctx.KsDustDevices.Include("Project").FirstOrDefault(dev => dev.NodeId == nodeId.ToString());
            if (device == null) return null;
            var clientSource = new KsDustClientSource(Properties.Resource.BusinessName, device);

            return clientSource;
        }
    }
}
