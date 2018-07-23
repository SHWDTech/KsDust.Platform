using System;
using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Web.Models.Setting
{
    public class ProjectBindAuditViewModel
    {
        public Guid DeviceGuid { get; set; }

        public AuditDevice Device { get; set; }

        public RequestProject Project { get; set; }
    }

    public class AuditDevice
    {
        [Display(Name = "设备名称")]
        public string Name { get; set; }

        [Display(Name = "供应商名称")]
        public string VendorName { get; set; }

        [Display(Name = "经度")]
        public string Longitude { get; set; }

        [Display(Name = "纬度")]
        public string Latitude { get; set; }
    }

    public class RequestProject
    {
        public Guid ProjectId { get; set; }

        [Display(Name = "所属区县")]
        public string DistrictName { get; set; }

        [Display(Name = "建设单位")]
        public string ConstructionUnit { get; set; }

        [Display(Name = "施工单位")]
        public string Enterprise { get; set; }

        [Display(Name = "工程名称")]
        public string Name { get; set; }

        [Display(Name = "合同备案号")]
        public string ContractRecord { get; set; }
    }
}