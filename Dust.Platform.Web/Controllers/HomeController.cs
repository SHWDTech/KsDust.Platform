using System.Web.Mvc;

namespace Dust.Platform.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}