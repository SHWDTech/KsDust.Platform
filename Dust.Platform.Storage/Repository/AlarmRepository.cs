using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class AlarmRepository : GuidRepository<KsDustAlarm>
    {
        public AlarmRepository()
        {

        }

        public AlarmRepository(KsDustDbContext dbContext) : base(dbContext)
        {

        }
    }
}
