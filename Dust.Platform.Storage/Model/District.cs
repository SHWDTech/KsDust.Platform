using System;
using SHWDTech.Platform.StorageConstrains.Model;
using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 区县信息
    /// </summary>
    [Serializable]
    public class District : GuidModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "区县名称")]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
