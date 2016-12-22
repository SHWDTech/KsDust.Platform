using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 工程信息
    /// </summary>
    [Serializable]
    public class KsDustProject : GuidModel
    {
        /// <summary>
        /// 工程类型
        /// </summary>
        [Required]
        [Display(Name = "工程类型")]
        public ProjectType ProjectType { get; set; }

        /// <summary>
        /// 所属区县ID
        /// </summary>
        [Required]
        [Display(Name = "所属区县ID")]
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
        public Guid EnterpriseId { get; set; } = Guid.Empty;

        /// <summary>
        /// 所属施工单位
        /// </summary>
        [ForeignKey("EnterpriseId")]
        [Display(Name = "所属施工单位")]
        public Enterprise Enterprise { get; set; }

        /// <summary>
        /// 所属供应商ID
        /// </summary>
        [Required]
        [Display(Name = "供应商ID")]
        public Guid VendorId { get; set; } = Guid.Empty;

        /// <summary>
        /// 所属供应商
        /// </summary>
        [ForeignKey("VendorId")]
        [Display(Name = "所属供应商")]
        public Vendor Vendor { get; set; }

        /// <summary>
        /// 工程所在区域
        /// </summary>
        [Display(Name = "工程所在区域")]
        public CityArea CityArea { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        [Display(Name = "工程名称")]
        [MaxLength(400)]
        public string Name { get; set; }

        /// <summary>
        /// 合同备案号
        /// </summary>
        [Display(Name = "合同备案号")]
        [Required]
        public string ContractRecord { get; set; }

        /// <summary>
        /// 工程地址
        /// </summary>
        [Display(Name = "工程地质")]
        [MaxLength(400)]
        public string Address { get; set; }

        /// <summary>
        /// 建设单位
        /// </summary>
        [Display(Name = "建设单位")]
        [MaxLength(100)]
        public string ConstructionUnit { get; set; }

        /// <summary>
        /// 工程负责人
        /// </summary>
        [Display(Name = "工程负责人")]
        [MaxLength(100)]
        public string SuperIntend { get; set; }

        /// <summary>
        /// 工程负责人联系电话
        /// </summary>
        [Display(Name = "工程负责人联系电话")]
        [MaxLength(100)]
        public string Mobile { get; set; }

        /// <summary>
        /// 工程占地面积
        /// </summary>
        [Display(Name = "工程占地面积")]
        public double OccupiedArea { get; set; }

        /// <summary>
        /// 工程建筑面积
        /// </summary>
        [Display(Name = "工程建筑面积")]
        public double Floorage { get; set; }

        /// <summary>
        /// 是否完成安装
        /// </summary>
        [Display(Name = "是否完成安装")]
        public bool Installed { get; set; }

        /// <summary>
        /// 是否已经审核
        /// </summary>
        [Display(Name = "是否已经审核")]
        public bool Audited { get; set; }

        /// <summary>
        /// 项目已经停工
        /// </summary>
        [Display(Name = "项目已经停工")]
        public bool Stopped { get; set; }
    }
}
