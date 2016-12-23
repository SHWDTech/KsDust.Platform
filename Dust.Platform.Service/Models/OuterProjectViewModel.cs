using Dust.Platform.Storage.Model;
using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Service.Models
{
    public class OuterProjectViewModel
    {
        [Required(ErrorMessage = "区县信息不能为空。")]
        public string District { get; set; }

        [Required(ErrorMessage = "建设单位不能为空。")]
        public string ConstructionUnit { get; set; }

        [Required(ErrorMessage = "施工单位Id号不能为空。")]
        public string EnterpriseId { get; set; }

        [Required(ErrorMessage = "施工单位名称不能为空。")]
        public string Enterprise { get; set; }

        [Required(ErrorMessage = "工程合同备案号不能为空。")]
        public string ContractRecord { get; set; }

        [Required(ErrorMessage = "工程名称不能为空。")]
        public string Project { get; set; }

        [Required(ErrorMessage = "工程地址不能为空。")]
        public string Address { get; set; }

        [Required(ErrorMessage = "工程所在区域不能为空。")]
        public CityArea? CityArea { get; set; }

        [Required(ErrorMessage = "联系人姓名不能为空。")]
        public string Superintend { get; set; }

        [Required(ErrorMessage = "联系电话不能为空。")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "占地面积不能为空。")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "占地面积只能是数字。")]
        public double? OccupiedArea { get; set; }

        [Required(ErrorMessage = "建筑面积不能为空。")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "建筑面积只能是数字。")]
        public double? Floorage { get; set; }

        [Required(ErrorMessage = "设备信息不能为空")]
        public string[] Devices { get; set; }
    }

    public class ProjectDeleteParams
    {
        public string ContractRecord { get; set; }
    }
}