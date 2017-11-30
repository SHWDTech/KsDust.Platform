using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required(ErrorMessage = "必须填写供应商名称！")]
        public string Name { get; set; }

        /// <summary>
        /// 供应商代码
        /// </summary>
        [Display(Name = "供应商代码")]
        [Index("Ix_ShortCode", IsUnique = true)]
        [StringLength(4, ErrorMessage = "供应商代码必须为四位")]
        [Required(ErrorMessage = "必须填写供应商代码")]
        public string ShortCode { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        [Display(Name = "负责人名称")]
        [MaxLength(100)]
        [Required(ErrorMessage = "必须填写负责人名称！")]
        public string Susperintend { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        [MaxLength(100)]
        [Required(ErrorMessage = "必须填写负责人联系电话！")]
        public string Mobile { get; set; }
    }
}
