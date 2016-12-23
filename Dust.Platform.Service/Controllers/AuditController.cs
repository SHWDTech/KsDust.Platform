using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public HttpResponseMessage Post([FromBody] AuditInfo model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new OuterPlatformExecuteResult(ModelState));
            }

            var project = _ctx.KsDustProjects.FirstOrDefault(p => p.ContractRecord == model.ContractRecord);
            if (project == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK,new OuterPlatformExecuteResult($"未找到合同备案号对应的工程。{model.ContractRecord}"));
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"系统内部错误，错误号：{errorCode}");
            }

            return Request.CreateResponse(HttpStatusCode.OK, new OuterPlatformExecuteResult().Success());
        }
    }
}
