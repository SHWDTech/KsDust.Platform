using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 扬尘监控数据
    /// </summary>
    [Serializable]
    public class KsDustMonitorData : LongModel
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        [Index("Ix_MonitorType_DistrictId_EnterpriseId_ProjectId_DeviceId_UpdateTime", 0)]
        public MonitorType MonitorType { get; set; }

        /// <summary>
        /// 所属区县ID
        /// </summary>
        [Required]
        [Display(Name = "所属区县ID")]
        [Index("Ix_MonitorType_DistrictId_EnterpriseId_ProjectId_DeviceId_UpdateTime", 1)]
        public Guid DistrictId { get; set; } = Guid.Empty;

        /// <summary>
        /// 所属区县
        /// </summary>
        [ForeignKey("DistrictId")]
        [Display(Name = "所属区县")]
        public District District { get; set; }

        /// <summary>
        /// 所属施工单位ID
        /// </summary>
        [Required]
        [Display(Name = "所属施工单位ID")]
        [Index("Ix_MonitorType_DistrictId_EnterpriseId_ProjectId_DeviceId_UpdateTime", 2)]
        public Guid EnterpriseId { get; set; } = Guid.Empty;

        /// <summary>
        /// 所属施工单额我i
        /// </summary>
        [ForeignKey("EnterpriseId")]
        [Display(Name = "所属施工单位")]
        public Enterprise Enterprise { get; set; }

        /// <summary>
        /// 所属工程ID
        /// </summary>
        [Required]
        [Display(Name = "所属工程ID")]
        [Index("Ix_MonitorType_DistrictId_EnterpriseId_ProjectId_DeviceId_UpdateTime", 3)]
        public Guid ProjectId { get; set; } = Guid.Empty;

        /// <summary>
        /// 所属工程
        /// </summary>
        [ForeignKey("ProjectId")]
        [Display(Name = "所属工程")]
        public KsDustProject Project { get; set; }

        /// <summary>
        /// 所属设备ID
        /// </summary>
        [Required]
        [Display(Name = "所属设备ID")]
        [Index("Ix_MonitorType_DistrictId_EnterpriseId_ProjectId_DeviceId_UpdateTime", 4)]
        public Guid DeviceId { get; set; } = Guid.Empty;

        /// <summary>
        /// 所属设备
        /// </summary>
        [ForeignKey("DeviceId")]
        [Display(Name = "所属设备")]
        public KsDustProject Device { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [DataType(DataType.DateTime)]
        [Index("Ix_MonitorType_DistrictId_EnterpriseId_ProjectId_DeviceId_UpdateTime", 5)]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 颗粒物总成值
        /// </summary>
        [Display(Name = "颗粒物总成值")]
        public double ParticulateMatter { get; set; }

        /// <summary>
        /// PM2.5含量
        /// </summary>
        [Display(Name = "PM2.5含量")]
        public double Pm25 { get; set; }

        /// <summary>
        /// PM10含量
        /// </summary>
        [Display(Name = "PM10含量")]
        public double Pm100 { get; set; }

        /// <summary>
        /// 噪音值
        /// </summary>
        [Display(Name = "噪音值")]
        public double Noise { get; set; }

        /// <summary>
        /// 温度值
        /// </summary>
        [Display(Name = "温度值")]
        public double Templeture { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        [Display(Name = "湿度")]
        public double Humidity { get; set; }

        /// <summary>
        /// 风速
        /// </summary>
        [Display(Name = "风速")]
        public double WindSpeed { get; set; }

        /// <summary>
        /// 风向
        /// </summary>
        [Display(Name = "风向")]
        public int WindDirection { get; set; }
    }
}
