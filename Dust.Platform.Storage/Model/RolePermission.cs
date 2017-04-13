using System;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class RolePermission : LongModel
    {
        [Index("Ix_Role_Permission", IsClustered = true, IsUnique = true, Order = 0)]
        public Guid RoleId { get; set; }

        [Index("Ix_Role_Permission", IsClustered = true, IsUnique = true, Order = 2)]
        public Guid PermissionId { get; set; }
    }
}
