using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Device;

namespace Dust.Platform.Web.Controllers
{
    public class DeviceController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public DeviceController()
        {
            _ctx = new KsDustDbContext();
        }

        [AllowAnonymous]
        public ActionResult Current(string nodeId)
        {
            var dev = _ctx.KsDustDevices.FirstOrDefault(d => d.NodeId == nodeId);
            if (dev == null) return null;
            var fifteen = _ctx.AverageMonitorDatas.Where(m => m.Category == AverageCategory.Device
                                                              && m.Type == AverageType.FifteenAvg
                                                              && m.TargetId == dev.Id)
                .OrderByDescending(d => d.AverageDateTime)
                .FirstOrDefault();
            var day = _ctx.AverageMonitorDatas.Where(m => m.Category == AverageCategory.Device
                                                          && m.Type == AverageType.DayAvg
                                                          && m.TargetId == dev.Id)
                .OrderByDescending(d => d.AverageDateTime)
                .FirstOrDefault();
            var model = new DeviceCurrentViewModel
            {
                DeviceName = dev.Name,
                Fifteen = fifteen,
                Day = day
            };

            return View(model);
        }
    }
}