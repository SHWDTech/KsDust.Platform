using System.Collections.Generic;
using System.Linq;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Web.Helper
{
    public static class WebSiteConfigHelper
    {
        public static List<SystemConfiguration> SystemConfigurations;

        public static void LoadSystemConfigurations(List<SystemConfiguration> configurations)
        {
            if (SystemConfigurations != null) return;
            SystemConfigurations = configurations;
        }

        public static string ExceedPhotoStoratePath => SystemConfigurations
            .FirstOrDefault(c => c.ConfigType == "SystemConfig" && c.ConfigName == "ExceedPhotoStoratePath")
            ?.ConfigValue;
    }
}