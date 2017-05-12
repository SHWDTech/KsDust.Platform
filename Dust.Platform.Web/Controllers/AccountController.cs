using System.Web.Mvc;
using System.Web.Security;
using Dust.Platform.Web.Models.Account;
using Dust.Platform.Web.Process;

namespace Dust.Platform.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = AccountProcess.PasswordSignIn(model);

            if (result.Status == SignInStatus.Failure)
            {
                ModelState.AddModelError(result.ErrorElement, result.ErrorMessage);
                return View(model);
            }
            Response.Cookies.Add(result.SignInCookie);
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

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