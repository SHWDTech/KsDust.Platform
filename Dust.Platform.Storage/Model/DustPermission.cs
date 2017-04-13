using System;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class DustPermission : GuidModel
    {
        public virtual string PermissionName { get; set; }

        public virtual string PermissionDisplayName { get; set; }

        public virtual Guid? ParentPermissionId { get; set; }
    }
}
