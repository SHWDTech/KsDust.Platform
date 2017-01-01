using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Statistics;
using Dust.Platform.Web.Models.Table;

namespace Dust.Platform.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public StatisticsController()
        {
            _ctx = new KsDustDbContext();
        }

        public ActionResult Projects()
        {
            var model = new ProjectsViewModel
            {
                Districts =
                    _ctx.Districts.Select(obj => new SelectListItem { Text = obj.Name, Value = obj.Id.ToString() })
                        .ToList(),
                Enterprises =
                _ctx.Enterprises.Select(obj => new SelectListItem { Text = obj.Name, Value = obj.Id.ToString() })
                        .ToList()
            };
            return View(model);
        }

        public ActionResult GetProjects(TablePost model)
        {
            var total = _ctx.KsDustProjects.Count();
            var projects = _ctx.KsDustProjects.OrderBy(obj => obj.Id).Skip(model.offset).Take(model.limit).Select(prj => new
            {
                prj.Name,
                prj.Address,
                prj.ContractRecord,
                prj.ConstructionUnit,
                DistrictName = prj.District.Name,
                EnterpriseName = prj.Enterprise.Name,
                VendorName = prj.Vendor.Name,
                prj.SuperIntend,
                prj.Mobile,
                prj.OccupiedArea,
                prj.Floorage
            }).ToList();

            return Json(new
            {
                total,
                rows = projects
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Districts()
        {
            return View();
        }

        public ActionResult GetDistricts(TablePost model)
        {
            var total = _ctx.Districts.Count();
            var districts = _ctx.Districts.Select(obj => new {obj.Id, obj.Name}).ToList();
            var lastDay = DateTime.Now.AddDays(-1);
            var lastMonth = DateTime.Now.AddMonths(-1);
            var ret = (from district in districts
                let query = _ctx.AverageMonitorDatas.Where(obj => obj.TargetId == district.Id)
                let lastDayValue = query.FirstOrDefault(obj => obj.AverageDateTime > lastDay && obj.Type == AverageType.DayAvg)
                let lastMonthValue = query.FirstOrDefault(obj => obj.AverageDateTime > lastMonth && obj.Type == AverageType.MonthAvg)
                let pcount = _ctx.KsDustProjects.Count(obj => obj.DistrictId == district.Id)
                let dcount = _ctx.KsDustDevices.Count(obj => obj.Project.DistrictId == district.Id)
                let toa = pcount == 0 ? 0 : _ctx.KsDustProjects.Where(item => item.DistrictId == district.Id).Sum(obj => obj.OccupiedArea)
                let tfa = pcount == 0 ? 0 : _ctx.KsDustProjects.Where(item => item.DistrictId == district.Id).Sum(obj => obj.Floorage)
                select new TotalDistrictsTable
                {
                    Name = district.Name,
                    ProjectsCount = pcount,
                    DevicesCount = dcount,
                    TotalOccupiedArea = toa,
                    TotalFloorage = tfa,
                    LastDayValue = lastDayValue?.ParticulateMatter ?? 0d,
                    LastMonthValue = lastMonthValue?.ParticulateMatter ?? 0d
                }).ToList();

            return Json(new
            {
                total,
                rows = ret
            }, JsonRequestBehavior.AllowGet);
        }
    }
}