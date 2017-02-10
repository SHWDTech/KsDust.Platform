using Dust.Platform.Storage.Model;

namespace Dust.Platform.Web.Models.Setting
{
    public class DeviceRegisterViewModel
    {
        public KsDustDevice Device { get; set; } = new KsDustDevice();

        public KsDustCamera Camera { get; set; } = new KsDustCamera();
    }
}