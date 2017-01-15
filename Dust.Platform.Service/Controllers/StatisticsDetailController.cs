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
                    var districtName = _ctx.Districts.First(obj => obj.Id == post.elementId).Name;
                    var entQuery = _ctx.KsDustProjects.Where(obj => obj.DistrictId == post.elementId);
                    if (post.ProjectType != ProjectType.AllType)
                    {
                        entQuery = entQuery.Where(obj => obj.ProjectType == post.ProjectType);
                    }
                    var enterprises = entQuery.Select(item => item.Enterprise).Distinct().ToList();
                    var enterpriseList = (
                from dev in enterprises
                let data = _ctx.AverageMonitorDatas.Where(dat => dat.Type == AverageType.HourAvg && dat.TargetId == dev.Id).OrderBy(da => da.AverageDateTime).FirstOrDefault()
                select new StatisticsViewModel
                {
                    id = dev.Id,
                    parentName = districtName,
                    name = dev.Name,
                    tsp = data?.ParticulateMatter ?? 0,
                    pm25 = data?.Pm25 ?? 0,
                    pm100 = data?.Pm100 ?? 0,
                    rate = Helper.GetRate(data?.ParticulateMatter ?? 0)
                }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, enterpriseList);
                case AverageCategory.Enterprise:
                    var enterpriseName = _ctx.Enterprises.First(obj => obj.Id == post.elementId).Name;
                    var prjQuery = _ctx.KsDustProjects.Where(obj => obj.EnterpriseId == post.elementId);
                    if (post.ProjectType != ProjectType.AllType)
                    {
                        prjQuery = prjQuery.Where(obj => obj.ProjectType == post.ProjectType);
                    }
                    var projects = prjQuery.ToList();
                    var projectList = (from dev in projects
                                       let data = _ctx.AverageMonitorDatas.Where(dat => dat.Type == AverageType.HourAvg && dat.TargetId == dev.Id).OrderBy(da => da.AverageDateTime).FirstOrDefault()
                                       select new StatisticsViewModel
                                       {
                                           id = dev.Id,
                                           parentName = enterpriseName,
                                           name = dev.Name,
                                           tsp = data?.ParticulateMatter ?? 0,
                                           pm25 = data?.Pm25 ?? 0,
                                           pm100 = data?.Pm100 ?? 0,
                                           rate = Helper.GetRate(data?.ParticulateMatter ?? 0)
                                       }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, projectList);
                case AverageCategory.Project:
                    var projectName = _ctx.Enterprises.First(obj => obj.Id == post.elementId).Name;
                    var devQuery = _ctx.KsDustDevices.Where(obj => obj.ProjectId == post.elementId);
                    if (post.ProjectType != ProjectType.AllType)
                    {
                        devQuery = devQuery.Where(obj => obj.Project.ProjectType == post.ProjectType);
                    }
                    var devs = devQuery.ToList();
                    var devList = (from dev in devs
                                   let data = _ctx.AverageMonitorDatas.Where(dat => dat.Type == AverageType.HourAvg && dat.TargetId == dev.Id).OrderBy(da => da.AverageDateTime).FirstOrDefault()
                                   select new StatisticsViewModel
                                   {
                                       id = dev.Id,
                                       parentName = projectName,
                                       name = dev.Name,
                                       tsp = data?.ParticulateMatter ?? 0,
                                       pm25 = data?.Pm25 ?? 0,
                                       pm100 = data?.Pm100 ?? 0,
                                       rate = Helper.GetRate(data?.ParticulateMatter ?? 0)
                                   }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, devList);
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "参数错误");
            }
        }
    }
}
