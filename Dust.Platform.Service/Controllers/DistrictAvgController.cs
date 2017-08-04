using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/DistrictAvg")]
    public class DistrictAvgController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public DistrictAvgController()
        {
            _ctx = new KsDustDbContext();
        }

        [Authorize]
        public List<DistrictAvgViewModel> Post([FromBody]DistrictAvgPostParams model)
        {
            var districts = _ctx.Districts.Where(dis => dis.Id != Guid.Empty).Select(d => new { d.Id, d.Name }).ToList();

            var avgs = _ctx.AverageMonitorDatas.Where(tsp => tsp.Type == model.dataType
                                                                      && tsp.Category == AverageCategory.District);
            if (model.projectType != null)
            {
                avgs = avgs.Where(item => item.ProjectType == model.projectType.Value);
            }

            var devs = model.projectType == null
                ? _ctx.KsDustDevices
                : _ctx.KsDustDevices.Where(c => c.Project.ProjectType == model.projectType);

            return (from district in districts
                    let avg = avgs.OrderByDescending(a => a.AverageDateTime).FirstOrDefault(av => av.TargetId == district.Id)
                    let dev = devs.Where(obj => obj.Project.DistrictId == district.Id)
                    select new DistrictAvgViewModel
                    {
                        id = district.Id,
                        name = district.Name,
                        count = dev.Count(),
                        tspAvg = avg?.ParticulateMatter ?? 0,
                        rate = avg == null ? 0 : Helper.GetRate(avg.ParticulateMatter)
                    }).ToList();
        }
    }
}
