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
        public ActionResult Current(string contractRecord)
        {
            var project = _ctx.KsDustProjects.FirstOrDefault(p => p.ContractRecord == contractRecord);
            if (project == null) return View();

            var devs = _ctx.KsDustDevices.Where(d => d.ProjectId == project.Id).ToList();
            if (devs.Count <= 0) return View();
            var model = new DeviceCurrentViewModel();
            foreach (var dev in devs)
            {
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
                var info = new DeviceCurrentInfo
                {
                    DeviceName = dev.Name,
                    Fifteen = fifteen,
                    Day = day
                };
                model.DeviceCurrentInfos.Add(info);
            }

            return View(model);
        }
    }
}