using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class MonitorDataRepository : LongRepository<KsDustMonitorData>
    {
        public MonitorDataRepository()
        {
            
        }

        public MonitorDataRepository(KsDustDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
