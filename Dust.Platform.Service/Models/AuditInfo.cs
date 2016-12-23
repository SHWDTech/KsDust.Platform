using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Service.Models
{
    public class AuditInfo
    {
        [Required(ErrorMessage = "必须提供合同备案号。")]
        public string ContractRecord { get; set; }

        [Required(ErrorMessage = "必须提供审核结果")]
        public bool? AuditResult { get; set; }
    }
}