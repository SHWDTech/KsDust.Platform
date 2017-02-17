using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class OnlineStatisticsRepository : LongRepository<OnlineStatistics>
    {
        public OnlineStatisticsRepository()
        {

        }

        public OnlineStatisticsRepository(KsDustDbContext dbContext) : base(dbContext)
        {

        }
    }
}
