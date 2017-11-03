using System.Collections.Generic;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Web.Models.Device
{
    public class DeviceCurrentViewModel
    {
        public List<DeviceCurrentInfo> DeviceCurrentInfos { get; set; } = new List<DeviceCurrentInfo>();
    }

    public class DeviceCurrentInfo
    {
        public string DeviceName { get; set; }

        public AverageMonitorData Fifteen { get; set; }

        public AverageMonitorData Day { get; set; }
    }
}