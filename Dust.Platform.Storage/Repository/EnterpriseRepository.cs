using System.Data.Entity;
using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class EnterpriseRepository : GuidRepository<District>
    {
        public EnterpriseRepository()
        {
            
        }

        public EnterpriseRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
