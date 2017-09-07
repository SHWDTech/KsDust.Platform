using System;
using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Web.Models.Setting
{
    public class DeviceRegisterViewModel
    {
        [Display(Name = "设备名称")]
        [Required(ErrorMessage = "必须填写设备名称")]
        public string Name { get; set; }

        [Display(Name = "设备MN码")]
        [Required(ErrorMessage = "必须填写设备MN码")]
        public string NodeId { get; set; }

        [Display(Name = "摄像头名称")]
        public string CameraName { get; set; }

        [Display(Name = "摄像头序列号")]
        public string SerialNumber { get; set; }

        [Display(Name = "登录用户名")]
        public string CameraUserName { get; set; }

        [Display(Name = "登陆密码")]
        public string CameraPassword { get; set; }

        [Display(Name = "设备所处经度")]
        [Required(ErrorMessage = "必须填写设备经度")]
        public string Longitude { get; set; }

        [Display(Name = "设备所处纬度")]
        [Required(ErrorMessage = "必须填写设备纬度")]
        public string Latitude { get; set; }

        [Display(Name = "所属工程")]
        [Required(ErrorMessage = "必须选择所属工程")]
        public Guid ProjectId { get; set; }
    }
}