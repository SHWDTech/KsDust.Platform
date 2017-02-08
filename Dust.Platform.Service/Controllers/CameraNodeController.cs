using System.Net.Http;
using System.Web.Http;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/CameraNode")]
    public class CameraNodeController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public CameraNodeController()
        {
            _ctx = new KsDustDbContext();
        }

        public HttpResponseMessage Get()
        {

            return null;
        }
    }
}
