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

            return (from district in districts
                    let avg = _ctx.AverageMonitorDatas.FirstOrDefault(tsp => tsp.Type == AverageType.FifteenAvg 
                    && tsp.TargetId == district.Id
                    && tsp.Category == AverageCategory.District 
                    && tsp.ProjectType == (ProjectType)model.projectType)
                    select new DistrictAvgViewModel
                    {
                        name = district.Name,
                        count = _ctx.KsDustDevices.Count(d => d.Project.DistrictId == district.Id && d.Project.ProjectType == (ProjectType)model.projectType),
                        tspAvg = avg?.ParticulateMatter ?? 0
                    }).ToList();
        }
    }
}
