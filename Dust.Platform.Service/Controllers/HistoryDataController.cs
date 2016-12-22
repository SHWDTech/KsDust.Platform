using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/HistoryData")]
    public class HistoryDataController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public HistoryDataController()
        {
            _ctx = new KsDustDbContext();
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]HistoryDataPostParams model)
        {
            var device = _ctx.KsDustDevices.FirstOrDefault(dev => dev.Id == model.device);
            if (device == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "设备不存在。");
            }

            var avgDatas =
                _ctx.AverageMonitorDatas.Where(
                    dat =>
                        dat.Type == (AverageType)model.dataType && dat.Category == AverageCategory.Device &&
                        dat.TargetId == device.Id).OrderByDescending(d => d.AverageDateTime).Take(30)
                        .ToList()
                        .OrderByDescending(obj => obj.AverageDateTime)
                        .Select(data => new HistoryDataViewModel { date = data.AverageDateTime.ToString("yyyy-MM-dd HH:mm:ss"), tsp = data.ParticulateMatter, pm25 = data.Pm25, pm100 = data.Pm100 }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, avgDatas);
        }
    }
}
