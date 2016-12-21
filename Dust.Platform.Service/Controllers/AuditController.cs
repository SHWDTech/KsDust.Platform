using System;
using System.Linq;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Storage.Repository;
using SHWDTech.Platform.Utility;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/Audit")]
    [AllowAnonymous]
    public class AuditController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public AuditController()
        {
            _ctx = new KsDustDbContext();
        }

        public OuterPlatformExecuteResult Post([FromBody] AuditInfo model)
        {
            var result = new OuterPlatformExecuteResult
            {
                Result = "failed"
            };

            var project = _ctx.KsDustProjects.FirstOrDefault(p => p.ContractRecord == model.ContractRecord);
            if (project == null)
            {
                result.Message = "未找到合同备案号对应的工程。";
                return result;
            }
            project.Audited = model.AuditResult;
            if (project.Audited)
            {
                //TODO 添加备案失败的逻辑
            }
            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = $"{DateTime.Now: yyyyMMddHHmmss}";
                LogService.Instance.Error($"审核工程失败，错误号：{errorCode}", ex);
                result.Message = $"系统内部错误，错误号：{errorCode}";
                return result;
            }

            result.Result = "success";
            return result;
        }
    }
}
