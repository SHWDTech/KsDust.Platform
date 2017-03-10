using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    [Serializable]
    public class AverageMonitorData : LongModel
    {
        /// <summary>
        /// 工程类型
        /// </summary>
        [Index("IX_ProjectType_AverageCategory_AverageType_TargetId_UpdateTime", IsClustered = true, Order = 0)]
        public ProjectType ProjectType { get; set; }

        /// <summary>
        /// 对象ID
        /// </summary>
        [Required]
        [Index("IX_ProjectType_AverageCategory_AverageType_TargetId_UpdateTime", IsClustered = true, Order = 3)]
        public Guid TargetId { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        [Required]
        [Index("IX_ProjectType_AverageCategory_AverageType_TargetId_UpdateTime", IsClustered = true, Order = 1)]
        public AverageCategory Category { get; set; }

        /// <summary>
        /// 均值类型
        /// </summary>
        [Required]
        [Index("IX_ProjectType_AverageCategory_AverageType_TargetId_UpdateTime", IsClustered = true, Order = 2)]
        public AverageType Type { get; set; }

        /// <summary>
        /// TSP均值
        /// </summary>
        public double ParticulateMatter { get; set; }

        /// <summary>
        /// PM2.5均值
        /// </summary>
        public double Pm25 { get; set; }

        /// <summary>
        /// PM10均值
        /// </summary>
        public double Pm100 { get; set; }

        /// <summary>
        /// 噪音值
        /// </summary>
        public double Noise { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        public double Humidity { get; set; }

        /// <summary>
        /// 风速
        /// </summary>
        public double WindSpeed { get; set; }

        /// <summary>
        /// 均值时间
        /// </summary>
        [Index("IX_ProjectType_AverageCategory_AverageType_TargetId_UpdateTime", IsClustered = true, Order = 4)]
        public DateTime AverageDateTime { get; set; }
    }
}
