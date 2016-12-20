using System.Web.Http;
using Dust.Platform.Service.Models;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/DistrictAvg")]
    [AllowAnonymous]
    public class ProjectController : ApiController
    {
        public void Post([FromBody]OuterProjectViewModel model )
        {
            
        }
    }
}
