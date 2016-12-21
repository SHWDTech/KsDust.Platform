using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    [Serializable]
    public class AverageMonitorData : LongModel
    {
        /// <summary>
        /// 工程类型
        /// </summary>
        public ProjectType ProjectType { get; set; }

        /// <summary>
        /// 对象ID
        /// </summary>
        [Required]
        public Guid TargetId { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        [Required]
        public AverageCategory Category { get; set; }

        /// <summary>
        /// 均值类型
        /// </summary>
        [Required]
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
        /// 均值时间
        /// </summary>
        public DateTime AverageDateTime { get; set; }
    }
}
