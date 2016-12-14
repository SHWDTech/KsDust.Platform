using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    [Serializable]
    public class Vendor : GuidModel
    {
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Display(Name = "供应商名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        [Display(Name = "负责人名称")]
        [MaxLength(100)]
        public string Susperintend { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        [MaxLength(100)]
        public string Mobile { get; set; }
    }
}
