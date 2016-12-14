using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class VendorRepository : GuidRepository<Vendor>
    {
        public VendorRepository()
        {
            
        }

        public VendorRepository(KsDustDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
