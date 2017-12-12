using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Dust.Platform.Web.Models.Account;
using Newtonsoft.Json;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Helper;
using Dust.Platform.Web.Schedules;
using Quartz;
using Quartz.Impl;

namespace Dust.Platform.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LoadInitThing();
            StartSchedule();
        }

        protected static void LoadInitThing()
        {
            using (var ctx = new KsDustDbContext())
            {
                var systemConfigurations = ctx.SystemConfigurations.Where(c => c.ConfigType == "SystemConfig").ToList();
                WebSiteConfigHelper.LoadSystemConfigurations(systemConfigurations);
            }
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return;
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket == null)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("Account/Login");
                return;
            }

            var serializeModel = JsonConvert.DeserializeObject<DustPrincipalModel>(authTicket.UserData);

            if (serializeModel == null)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("Account/Login");
                return;
            }

            HttpContext.Current.User = new DustPrincipal(serializeModel);
        }

        private static void StartSchedule()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<DayWeatherJob>()
                .Build();
            var trigger = TriggerBuilder.Create()
                .StartAt(DateTime.Now.AddDays(1).AddHours(-DateTime.Now.Hour - 1))
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }
    }
}
