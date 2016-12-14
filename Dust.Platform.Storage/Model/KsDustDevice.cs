using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 监测设备信息
    /// </summary>
    [Serializable]
    public class KsDustDevice : GuidModel
    {
        /// <summary>
        /// 设备识别号
        /// </summary>
        [Display(Name = "设备识别号")]
        [MaxLength(100)]
        public string NodeId { get; set; }

        /// <summary>
        /// 设备所属工程ID
        /// </summary>
        [Required]
        [Display(Name = "所属工程ID")]
        public Guid ProjectId { get; set; } = Guid.Empty;

        /// <summary>
        /// 设备所属工程
        /// </summary>
        [ForeignKey("ProjectId")]
        [Display(Name = "所属工程")]
        public KsDustProject Project { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Display(Name = "设备名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [Display(Name = "经度")]
        public string Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [Display(Name = "纬度")]
        public string Latitude { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        [Display(Name = "在线状态")]
        public bool IsOnline { get; set; }

        /// <summary>
        /// 安装时间
        /// </summary>
        [Display(Name = "安装时间")]
        [DataType(DataType.DateTime)]
        public DateTime InstallDateTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// 最后维保日期
        /// </summary>
        [Display(Name ="最后维保日期")]
        [DataType(DataType.DateTime)]
        public DateTime LastMaintenance { get; set; }
    }
}
