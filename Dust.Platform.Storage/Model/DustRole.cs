using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class DustRole : GuidModel
    {
        public string DisplayName { get; set; }

        public RoleStatus Status { get; set; }
    }
}
