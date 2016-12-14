using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class DeviceRepository : GuidRepository<KsDustDevice>
    {
        public DeviceRepository()
        {
            
        }

        public DeviceRepository(KsDustDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
