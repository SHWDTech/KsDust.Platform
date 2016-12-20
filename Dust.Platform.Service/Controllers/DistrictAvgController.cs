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

        public List<DistrictAvgViewModel> Post()
        {
            var districtAvgs = new List<DistrictAvgViewModel>();
            var districts = _ctx.Districts.Select(d => new {d.Id, d.Name}).ToList();
            foreach (var district in districts)
            {
                districtAvgs.Add(new DistrictAvgViewModel
                {
                    name = district.Name,
                    count = _ctx.KsDustDevices.Count(d => d.Project.DistrictId == district.Id),
                    tspAvg = _ctx.AverageMonitorDatas.First(tsp => tsp.Type == AverageType.FifteenAvg && tsp.Category == AverageCategory.District).ParticulateMatter
                });
            }

            return districtAvgs;
        }
    }
}
