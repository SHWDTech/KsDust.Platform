using System;
using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Web.Models.Setting
{
    public class ProjectBindViewModel
    {
        public Guid? DeviceGuid { get; set; }

        [Display(Name = "设备名称")]
        public string DeviceName { get; set; }

        [Display(Name = "绑定工程")]
        public Guid? ProjectBindRequestGuid { get; set; }
    }
}