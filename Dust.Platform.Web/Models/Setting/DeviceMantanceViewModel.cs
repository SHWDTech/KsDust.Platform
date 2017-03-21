using Dust.Platform.Storage.Model;
using Dust.Platform.Web.Models.Table;

namespace Dust.Platform.Web.Models.Setting
{
    public class DeviceMantanceTablePost : TablePost
    {

    }

    public class EditDeviceMantanceViewModel : DeviceMantanceRecord
    {
        public string DeviceName { get; set; }
    }
}