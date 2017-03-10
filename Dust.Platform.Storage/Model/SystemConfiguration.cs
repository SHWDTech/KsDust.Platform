using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 系统配置信息
    /// </summary>
    [Serializable]
    public class SystemConfiguration : GuidModel
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        [Required]
        [MaxLength(500)]
        [Display(Name = "配置类型")]
        [Index("IX_ConfigType_ConfigName", IsClustered = true, Order = 0)]
        public string ConfigType { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        [Required]
        [MaxLength(500)]
        [Display(Name = "配置名称")]
        [Index("IX_ConfigType_ConfigName", IsClustered = true, Order = 1)]
        public string ConfigName { get; set; }

        /// <summary>
        /// 配置值
        /// </summary>
        [Required]
        [MaxLength(8000)]
        [Display(Name = "配置值")]
        public string ConfigValue { get; set; }
    }
}
