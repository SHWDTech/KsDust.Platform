using SHWDTech.Platform.StorageConstrains.Repository;

namespace Dust.Platform.Storage.Repository
{
    public class KsDustRepository
    {
        public static T Repo<T>() where T : class, IRepositoryBase, new()
            => new T { DbContext = new KsDustDbContext() };

        public static T Repo<T>(KsDustDbContext dbContext) where T : class, IRepositoryBase, new()
            => new T {DbContext = dbContext};
    }
}
