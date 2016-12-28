using System.Data.Entity;
using Dust.Platform.Service.Entities;
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
    }
}