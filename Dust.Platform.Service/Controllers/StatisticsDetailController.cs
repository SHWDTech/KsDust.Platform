using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/StatisticsDetail")]
    public class StatisticsDetailController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public StatisticsDetailController()
        {
            _ctx = new KsDustDbContext();
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]StatisticsDetailPost post)
        {
            switch (post.elementType)
            {
                case AverageCategory.District:
                    var enterprises = _ctx.KsDustProjects.Where(obj => obj.DistrictId == post.elementId).Select(item => item.Enterprise).Distinct().ToList();
                    var entavgs = _ctx.AverageMonitorDatas.Where(tsp => tsp.Type == post.dataType && tsp.Category == AverageCategory.Enterprise);
                    var projects = _ctx.KsDustProjects.Where(obj => obj.DistrictId == post.elementId).ToList();
                    var enterpriseList = (from ent in enterprises
                                          let avg = entavgs.OrderByDescending(a => a.AverageDateTime).FirstOrDefault(av => av.TargetId == ent.Id)
                                          let prjs = projects.Where(obj => obj.EnterpriseId == ent.Id)
                                          select new DistrictAvgViewModel
                                          {
                                              id = ent.Id,
                                              name = ent.Name,
                                              count = prjs.Count(),
                                              tspAvg = avg?.ParticulateMatter ?? 0
                                          }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, enterpriseList);
                case AverageCategory.Enterprise:
                    var prjQuery = _ctx.KsDustProjects.Where(obj => obj.EnterpriseId == post.elementId).ToList();
                    var prjavgs = _ctx.AverageMonitorDatas.Where(tsp => tsp.Type == post.dataType && tsp.Category == AverageCategory.Project);
                    var devices = _ctx.KsDustDevices.Where(obj => obj.Project.EnterpriseId == post.elementId);
                    var projectList = (from prj in prjQuery
                                       let avg = prjavgs.OrderByDescending(a => a.AverageDateTime).FirstOrDefault(av => av.TargetId == prj.Id)
                                       let devs = devices.Where(obj => obj.ProjectId == prj.Id)
                                       select new DistrictAvgViewModel
                                       {
                                           id = prj.Id,
                                           name = prj.Name,
                                           count = devs.Count(),
                                           tspAvg = avg?.ParticulateMatter ?? 0
                                       }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, projectList);
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "参数错误");
            }
        }
    }
}
