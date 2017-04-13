using System.Data.Entity;
using Dust.Platform.Service.Entities;
using Dust.Platform.Storage.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dust.Platform.Storage.Repository
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext() : base("AuthContext")
        {
            
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<DustRole> DustRoles { get; set; }

        public DbSet<DustPermission> DustPermissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }
    }
}