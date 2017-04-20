using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Dust.Platform.Web.Models.Account;
using Newtonsoft.Json;

namespace Dust.Platform.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return;
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket == null)
            {
                throw new InvalidDataException("Ticket Decrype Failed");
            }

            var serializeModel = JsonConvert.DeserializeObject<DustPrincipalModel>(authTicket.UserData);

            if (serializeModel == null) return;
            var newUser = new DustPrincipal(serializeModel);

            HttpContext.Current.User = newUser;
        }
    }
}
