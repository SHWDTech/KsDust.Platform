using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    [Serializable]
    public class DeviceMantanceRecord : LongModel
    {
        /// <summary>
        /// 关联的维保设备
        /// </summary>
        [Index("IX_Device_MantanceDateTime", IsClustered = true, Order = 0)]
        public Guid Device { get; set; }

        /// <summary>
        /// 维保负责人
        /// </summary>
        [Display(Name = "维保人")]
        [MaxLength(50)]
        public string MantancePerson { get; set; }

        /// <summary>
        /// 维保报告
        /// </summary>
        [Display(Name = "维保报告")]
        public string MantanceReport { get; set; }

        /// <summary>
        /// 维保时间
        /// </summary>
        [Display(Name = "维保时间")]
        [Index("IX_Device_MantanceDateTime", IsClustered = true, Order = 1)]
        public DateTime MantanceDateTime { get; set; }
    }
}
