using System.Linq;
using System.Net;
using System.Net.Http;
using Dust.Platform.Storage.Repository;
using System.Web.Http;
using Dust.Platform.Service.Models;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/VersionCode")]
    public class VersionCodeController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public VersionCodeController()
        {
            _ctx = new KsDustDbContext();
        }

        public HttpResponseMessage Get()
        {
            var cfgs = _ctx.SystemConfigurations.Where(c => c.ConfigType == "AndroidVersionCode");
            var info = new AndroidVersionInfo
            {
                VersionName = cfgs.First(c => c.ConfigName == "VersionName").ConfigValue,
                VersionCode = int.Parse(cfgs.First(c => c.ConfigName == "VersionCode").ConfigValue),
                ApkAddress = cfgs.First(c => c.ConfigName == "ApkAddress").ConfigValue
            };

            return Request.CreateResponse(HttpStatusCode.OK, info);
        }
    }
}
