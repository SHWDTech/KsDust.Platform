using Dust.Platform.Storage.Model;
using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class ProjectRepository : GuidRepository<KsDustProject>
    {
        public ProjectRepository()
        {
            
        }

        public ProjectRepository(KsDustDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
