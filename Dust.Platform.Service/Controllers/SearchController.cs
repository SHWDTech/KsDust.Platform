using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/Search")]
    public class SearchController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public SearchController()
        {
            _ctx = new KsDustDbContext();
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]SearchPost post)
        {
            var resultList = new List<SearchViewModel>();
            resultList.AddRange(_ctx.KsDustDevices.Where(dev => dev.Name.Contains(post.searchName)).Select(item => new {item.Id, item.Name}).ToList()
                .Select(obj => new SearchViewModel { objectId = obj.Id, objectName = obj.Name, objectLevel = AverageCategory.Device }));
            //resultList.AddRange(_ctx.KsDustProjects.Where(prj => prj.Name.Contains(post.searchName)).Select(item => new { item.Id, item.Name }).ToList()
            //    .Select(obj => new SearchViewModel { objectId = obj.Id, objectName = obj.Name, objectLevel = AverageCategory.Project }));
            resultList.AddRange(_ctx.Enterprises.Where(ent => ent.Name.Contains(post.searchName)).Select(item => new { item.Id, item.Name }).ToList()
                .Select(obj => new SearchViewModel { objectId = obj.Id, objectName = obj.Name, objectLevel = AverageCategory.Enterprise }));
            resultList.AddRange(_ctx.Districts.Where(dis => dis.Name.Contains(post.searchName)).Select(item => new { item.Id, item.Name }).ToList()
                .Select(obj => new SearchViewModel { objectId = obj.Id, objectName = obj.Name, objectLevel = AverageCategory.District }));
            var projects = _ctx.KsDustProjects.Include(nameof(Enterprise))
                .Include(nameof(District))
                .Where(prj => prj.ContractRecord.Contains(post.searchName))
                .ToList();
            foreach (var project in projects)
            {
                if (resultList.All(r => r.objectId != project.EnterpriseId))
                {
                    resultList.Add(new SearchViewModel
                    {
                        objectId = project.EnterpriseId,
                        objectLevel = AverageCategory.Enterprise,
                        objectName = project.Enterprise.Name
                    });
                }
                if (resultList.All(r => r.objectId != project.DistrictId))
                {
                    resultList.Add(new SearchViewModel
                    {
                        objectId = project.DistrictId,
                        objectLevel = AverageCategory.District,
                        objectName = project.District.Name
                    });
                }
                var devs = _ctx.KsDustDevices.Where(d => d.ProjectId == project.Id);
                foreach (var dev in devs)
                {
                    if (resultList.All(r => r.objectId != dev.Id))
                    {
                        resultList.Add(new SearchViewModel
                        {
                            objectId = dev.Id,
                            objectLevel = AverageCategory.Device,
                            objectName = dev.Name
                        });
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, resultList.Distinct());
        }
    }
}
