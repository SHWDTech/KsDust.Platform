using System;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Dust.Platform.Service.Providers;
using Dust.Platform.Storage.Repository;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(Dust.Platform.Service.StartUp))]
namespace Dust.Platform.Service
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);

            LoadSystemConfiguaration();
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

        private static void LoadSystemConfiguaration()
        {
            var ctx = new KsDustDbContext();
            var firstOrDefault = ctx.SystemConfigurations.FirstOrDefault(cfg => cfg.ConfigType == "RsaKeys" && cfg.ConfigName == "RsaPrivate");
            if (firstOrDefault != null)
            {
                BasicConfig.RsaPrivateKey = firstOrDefault.ConfigValue;
            }
        }
    }
}