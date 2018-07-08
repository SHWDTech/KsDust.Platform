using System.ComponentModel.DataAnnotations;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Web.Models
{
    public class ManualOuterProjectViewModel
    {
        [Display(Name = "区县信息")]
        [Required(ErrorMessage = "区县信息不能为空。")]
        public string District { get; set; }

        [Display(Name = "工程类型")]
        [Required(ErrorMessage = "工程类型不能为空")]
        public ProjectType? ProjectType { get; set; }

        [Display(Name = "建设单位")]
        [Required(ErrorMessage = "建设单位不能为空。")]
        public string ConstructionUnit { get; set; }

        [Display(Name = "施工单位ID")]
        [Required(ErrorMessage = "施工单位Id号不能为空。")]
        public string EnterpriseId { get; set; }

        [Display(Name = "施工单位名称")]
        [Required(ErrorMessage = "施工单位名称不能为空。")]
        public string Enterprise { get; set; }

        [Display(Name = "工程合同备案号")]
        [Required(ErrorMessage = "工程合同备案号不能为空。")]
        public string ContractRecord { get; set; }

        [Display(Name = "工程名称")]
        [Required(ErrorMessage = "工程名称不能为空。")]
        public string Project { get; set; }

        [Display(Name = "工程地址")]
        [Required(ErrorMessage = "工程地址不能为空。")]
        public string Address { get; set; }

        [Display(Name = "工程所在区域")]
        [Required(ErrorMessage = "工程所在区域不能为空。")]
        public CityArea? CityArea { get; set; }

        [Display(Name = "联系人姓名")]
        [Required(ErrorMessage = "联系人姓名不能为空。")]
        public string Superintend { get; set; }

        [Display(Name = "联系电话")]
        [Required(ErrorMessage = "联系电话不能为空。")]
        public string Mobile { get; set; }

        [Display(Name = "占地面积")]
        [Required(ErrorMessage = "占地面积不能为空。")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "占地面积只能是数字。")]
        public double? OccupiedArea { get; set; }

        [Display(Name = "建筑面积")]
        [Required(ErrorMessage = "建筑面积不能为空。")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "建筑面积只能是数字。")]
        public double? Floorage { get; set; }

        public void Trim()
        {
            District = District.Trim();
            ConstructionUnit = ConstructionUnit.Trim();
            EnterpriseId = EnterpriseId.Trim();
            Enterprise = Enterprise.Trim();
            ContractRecord = ContractRecord.Trim();
        }
    }
}