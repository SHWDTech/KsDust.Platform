using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Service.Process;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/DistrictDetail")]
    public class DistrictDetailController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public DistrictDetailController()
        {
            _ctx = new KsDustDbContext();
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]DistrictDetailPostParams model)
        {
            var district = _ctx.Districts.FirstOrDefault(d => d.Id == model.District);
            if (district == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "区县不存在。");
            }

            var devs = this.CreateFilterProcess().GetAuthedDevices(dev => dev.Project.ProjectType == model.ProjectType 
            && dev.Project.DistrictId == district.Id).ToList();
            var devList = (
                from dev in devs
                let data = _ctx.AverageMonitorDatas.Where(dat => dat.Type == AverageType.HourAvg && dat.TargetId == dev.Id)
                .OrderBy(da => da.AverageDateTime)
                .FirstOrDefault()
                select new DistrictDetailViewModel
                {
                    id = dev.Id,
                    districtName = district.Name,
                    name = dev.Name,
                    tsp = data?.ParticulateMatter ?? 0,
                    pm25 = data?.Pm25 ?? 0,
                    pm100 = data?.Pm100 ?? 0,
                    rate = Helper.GetRate(data?.ParticulateMatter ?? 0)
                }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, devList);
        }
    }
}
