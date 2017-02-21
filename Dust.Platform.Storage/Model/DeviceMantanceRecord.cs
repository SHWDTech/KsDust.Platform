using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class DeviceMantanceRecord : LongModel
    {
        /// <summary>
        /// 关联的维保设备
        /// </summary>
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
        public DateTime MantanceDateTime { get; set; }
    }
}
