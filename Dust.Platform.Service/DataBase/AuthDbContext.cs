using System.Data.Entity;
using Dust.Platform.Service.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dust.Platform.Service.DataBase
{
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        public AuthDbContext() : base("AuthDbContext")
        {
            
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}