using System.Web.Mvc;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public AdminController()
        {
            _ctx = new KsDustDbContext();
        }

        // GET: Admin
        public ActionResult Message()
        {
            return View();
        }
    }
}