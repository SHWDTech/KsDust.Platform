using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Service.Process;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/CascadeElement")]
    public class CascadeElementController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public CascadeElementController()
        {
            _ctx = new KsDustDbContext();
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]CascadeElementPostParams model)
        {
            switch (model.CascadeElementLevel)
            {
                    case AverageCategory.District:
                    var enterprises =
                        this.CreateFilterProcess().GetAuthedProjects(null).Where(prj => prj.DistrictId == model.CascadeElementId)
                            .Select(obj => obj.Enterprise)
                            .Distinct()
                            .Select(
                                item =>
                                    new CascadeElementViewModel
                                    {
                                        CascadeElementId = item.Id,
                                        CascadeElementLevel = AverageCategory.Enterprise,
                                        CascadeElementName = item.Name
                                    });
                    return Request.CreateResponse(HttpStatusCode.OK, enterprises);
                    case AverageCategory.Enterprise:
                    var projects =
                        this.CreateFilterProcess().GetAuthedProjects(null).Where(obj => obj.EnterpriseId == model.CascadeElementId)
                            .Select(item => new CascadeElementViewModel
                            {
                                CascadeElementId = item.Id,
                                CascadeElementLevel = AverageCategory.Project,
                                CascadeElementName = item.Name
                            });
                    return Request.CreateResponse(HttpStatusCode.OK, projects);
                    case AverageCategory.Project:
                    var devices =
                        this.CreateFilterProcess().GetAuthedDevices(null).Where(obj => obj.ProjectId == model.CascadeElementId)
                        .Select(item => new CascadeElementViewModel
                        {
                            CascadeElementId = item.Id,
                            CascadeElementLevel = AverageCategory.Device,
                            CascadeElementName = item.Name
                        });
                    return Request.CreateResponse(HttpStatusCode.OK, devices);
                    case AverageCategory.Device:
                    return Request.CreateResponse(HttpStatusCode.OK);
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "未知层级或层级元素");
            }
        }
    }
}
