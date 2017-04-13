using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dust.Platform.Web.Models.Account
{
    public class DustPrincipalModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<IdentityUserRole> Roles { get; set; }

        public List<IdentityUserClaim> Claims { get; set; }
    }
}