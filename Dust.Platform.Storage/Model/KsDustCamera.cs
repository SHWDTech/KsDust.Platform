using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 摄像头信息
    /// </summary>
    [Serializable]
    public class KsDustCamera : GuidModel
    {
        /// <summary>
        /// 关联设备ID
        /// </summary>
        [Required]
        [Display(Name = "相关设备ID")]
        public Guid DeviceId { get; set; }

        /// <summary>
        /// 关联设备
        /// </summary>
        [ForeignKey("DeviceId")]
        [Display(Name = "相关设备")]
        public KsDustDevice Device { get; set; }

        /// <summary>
        /// 摄像头名称
        /// </summary>
        [Display(Name = "摄像头名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 摄像头域名
        /// </summary>
        [Display(Name = "摄像头序列号")]
        [MaxLength(100)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 登陆用户名
        /// </summary>
        [Display(Name = "登陆用户名")]
        [MaxLength(100)]
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Display(Name = "登陆密码")]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
