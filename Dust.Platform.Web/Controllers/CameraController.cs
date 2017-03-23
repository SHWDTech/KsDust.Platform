using System.Web.Mvc;

namespace Dust.Platform.Web.Controllers
{
    public class CameraController : Controller
    {
        // GET: Camera
        public ActionResult Preview() => PartialView();
    }
}