using System.Collections.Generic;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Web.Models.Account
{
    public class RolePermissionsModel
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public List<DustPermission> Permissions { get; set; }

        public List<DustPermission> RolePermissions { get; set; }
    }
}