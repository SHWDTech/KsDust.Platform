using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class CameraRepository : GuidRepository<KsDustCamera>
    {
        public CameraRepository()
        {
            
        }

        public CameraRepository(KsDustDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
