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

            return Request.CreateResponse(HttpStatusCode.OK, resultList);
        }
    }
}
