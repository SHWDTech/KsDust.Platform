using System;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Web.Controllers
{
    public class MonitorController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public MonitorController()
        {
            _ctx = new KsDustDbContext();
        }

        [HttpGet]
        [Route("Monitor/{ContractRecord}/Current")]
        public ActionResult Current(string contractRecord)
        {
            var checkDate = DateTime.Now.AddMinutes(-15);
            var project = _ctx.KsDustProjects.FirstOrDefault(p => p.ContractRecord == contractRecord);
            if (project == null) return new HttpNotFoundResult("project not found");
            var monitorData = _ctx.KsDustMonitorDatas.Where(d => d.ProjectId == project.Id && d.UpdateTime > checkDate)
                .OrderByDescending(da => da.UpdateTime)
                .FirstOrDefault();
            if (monitorData == null)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Tsp = monitorData.ParticulateMatter,
                monitorData.Noise,
                PM25 = monitorData.Pm25,
                PM100 = monitorData.Pm100,
                monitorData.UpdateTime
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("Monitor/{ContractRecord}/Last")]
        public ActionResult Last(string contractRecord)
        {
            var project = _ctx.KsDustProjects.FirstOrDefault(p => p.ContractRecord == contractRecord);
            if (project == null) return new HttpNotFoundResult("project not found");
            var monitorData = _ctx.KsDustMonitorDatas.Where(d => d.ProjectId == project.Id)
                .OrderByDescending(da => da.UpdateTime)
                .FirstOrDefault();
            if (monitorData == null)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Tsp = monitorData.ParticulateMatter,
                monitorData.Noise,
                PM25 = monitorData.Pm25,
                PM100 = monitorData.Pm100,
                monitorData.UpdateTime
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("Monitor/{ContractRecord}/History")]
        public ActionResult History(string contractRecord)
        {
            var project = _ctx.KsDustProjects.FirstOrDefault(p => p.ContractRecord == contractRecord);
            if (project == null) return new HttpNotFoundResult("project not found");
            var startStr = Request.QueryString["start"];
            if (!DateTime.TryParse(startStr, out DateTime startDate))
            {
                return Json("params error, invald start", JsonRequestBehavior.AllowGet);
            }
            var endStr = Request.QueryString["end"];
            if (!DateTime.TryParse(endStr, out DateTime endDate))
            {
                return Json("params error, invald end", JsonRequestBehavior.AllowGet);
            }
            var rows = _ctx.KsDustMonitorDatas.Where(d =>
                    d.ProjectId == project.Id && d.UpdateTime > startDate && d.UpdateTime < endDate)
                .Select(da => new
                {
                    Tsp = da.ParticulateMatter,
                    da.Noise,
                    PM25 = da.Pm25,
                    PM100 = da.Pm100,
                    da.UpdateTime
                }).ToList();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
    }
}