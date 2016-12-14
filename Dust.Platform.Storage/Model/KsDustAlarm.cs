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
    public class KsDustAlarm : GuidModel
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
        /// 报警值
        /// </summary>
        [Display(Name = "报警值")]
        public double AlarmValue { get; set; }

        /// <summary>
        /// 报警时间
        /// </summary>
        [Display(Name = "报警时间")]
        public DateTime AlarmDateTime { get; set; }

        /// <summary>
        /// 是否已经阅读
        /// </summary>
        [Display(Name = "是否已经阅读")]
        public bool IsReaded { get; set; }
    }
}
