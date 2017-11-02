using Dust.Platform.Storage.Model;

namespace Dust.Platform.Web.Models.Device
{
    public class DeviceCurrentViewModel
    {
        public string DeviceName { get; set; }

        public AverageMonitorData Fifteen { get; set; }

        public AverageMonitorData Day { get; set; }
    }
}