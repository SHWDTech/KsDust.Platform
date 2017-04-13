using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dust.Platform.Web.Models.Account
{
    public class DustPrincipal : IPrincipal
    {
        private readonly List<IdentityUserRole> _roles;

        public List<IdentityUserClaim> Claims { get; }

        public string Id { get; }

        public string Name { get; }

        public DustPrincipal(DustPrincipalModel model)
        {
            Identity = new GenericIdentity(model.Name);
            _roles = model.Roles;
            Id = model.Id;
            Name = model.Name;
            Claims = model.Claims;
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity { get; }
    }
}