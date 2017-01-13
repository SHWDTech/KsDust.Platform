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
            resultList.AddRange(_ctx.KsDustDevices.Where(dev => dev.Name.Contains(post.objectId)).Select(item => item.Name).ToList()
                .Select(obj => new SearchViewModel { name = obj, category = AverageCategory.Device }));
            resultList.AddRange(_ctx.KsDustProjects.Where(prj => prj.Name.Contains(post.objectId)).Select(item => item.Name).ToList()
                .Select(obj => new SearchViewModel { name = obj, category = AverageCategory.Project }));
            resultList.AddRange(_ctx.Enterprises.Where(ent => ent.Name.Contains(post.objectId)).Select(item => item.Name).ToList()
                .Select(obj => new SearchViewModel { name = obj, category = AverageCategory.Enterprise }));
            resultList.AddRange(_ctx.Districts.Where(dis => dis.Name.Contains(post.objectId)).Select(item => item.Name).ToList()
                .Select(obj => new SearchViewModel { name = obj, category = AverageCategory.District }));

            return Request.CreateResponse(HttpStatusCode.OK, resultList);
        }
    }
}
