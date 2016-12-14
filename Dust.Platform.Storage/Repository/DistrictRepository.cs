using System.Data.Entity;
using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class DistrictRepository : GuidRepository<District>
    {
        public DistrictRepository()
        {
            
        }

        public DistrictRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
