using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class DeviceOnlineStatusRepository : LongRepository<DeviceOnlineStatus>
    {
        public DeviceOnlineStatusRepository()
        {

        }

        public DeviceOnlineStatusRepository(KsDustDbContext dbContext) : base(dbContext)
        {

        }
    }
}
