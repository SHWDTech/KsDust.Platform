using System.Web.Mvc;
using System.Web.Security;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Account;
using Dust.Platform.Web.Process;

namespace Dust.Platform.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthContext _ctx;

        public AccountController()
        {
            _ctx = new AuthContext();
        }

        // GET: Account
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginTitle = "昆山扬尘在线监控平台";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LoginTitle = "昆山扬尘在线监控平台";
                return View(model);
            }

            var result = AccountProcess.PasswordSignIn(model);

            if (result.Status == SignInStatus.Failure)
            {
                ViewBag.LoginTitle = "餐饮油烟在线监控平台";
                ModelState.AddModelError(result.ErrorElement, result.ErrorMessage);
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            Response.Cookies.Add(result.SignInCookie);

            return Redirect(returnUrl);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}