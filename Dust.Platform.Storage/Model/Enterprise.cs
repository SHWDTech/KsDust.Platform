using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 施工单位信息
    /// </summary>
    [Serializable]
    public class Enterprise : GuidModel
    {
        /// <summary>
        /// 施工单位名称
        /// </summary>
        [Display(Name = "施工单位名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        [MaxLength(100)]
        public string Mobile { get; set; }

        /// <summary>
        /// 安监平台ID
        /// </summary>
        [Required]
        [Display(Name = "安监平台ID")]
        public string OuterId { get; set; }
    }
}
