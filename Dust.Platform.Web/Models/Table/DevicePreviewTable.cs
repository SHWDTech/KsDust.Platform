using System;

namespace Dust.Platform.Web.Models.Table
{
    public class DevicePreviewTable
    {
        public Guid Id { get; set; }

        public Guid DistrictGuid { get; set; }

        public Guid EnterpriseGuid { get; set; }

        public Guid ProjectGuid { get; set; }

        public bool Audited => ProjectGuid != Guid.Empty;

        public string Name { get; set; }

        public string VendorName { get; set; }

        public string NodeId { get; set; }

        public string ProjectName { get; set; }

        public string SuperIntend { get; set; }

        public string Mobile { get; set; }

        public string LastDataTime { get; set; }
    }
}