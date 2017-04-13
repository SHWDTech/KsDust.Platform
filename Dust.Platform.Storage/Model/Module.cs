using System;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class Module : GuidModel
    {
        public virtual bool IsMenu { get; set; }

        public virtual byte ModuleLevel { get; set; }

        public virtual int ModuleIndex { get; set; }

        public virtual string IconString { get; set; }

        public virtual string ModuleName { get; set; }

        public virtual string Controller { get; set; }

        public virtual string Action { get; set; }

        public virtual Guid? ParentModuleId { get; set; }

        public virtual Guid PermissionId { get; set; }
    }
}
