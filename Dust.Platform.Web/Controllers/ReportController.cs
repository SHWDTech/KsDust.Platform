using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Home;
using Dust.Platform.Web.Models.Report;
using Newtonsoft.Json;

namespace Dust.Platform.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public ReportController()
        {
            _ctx = new KsDustDbContext();
        }

        // GET: Report
        public ActionResult Index()
        {
            var model = new ReportViewModel
            {
                MenuNodes = ReportNodes()
            };
            return View(model);
        }

        private List<Nodes> ReportNodes()
        {
            var nodes = new List<Nodes>
            {
                new Nodes
                {
                    id = "monthReport",
                    name = "月报表",
                    routeValue = ReportType.Month
                },
                new Nodes
                {
                    id = "yearReport",
                    name = "年报表",
                    routeValue = ReportType.Year
                }
            };

            return nodes;
        }

        public ActionResult ReportSelecter(ReportType type)
        {
            var reports = _ctx.Reports.Where(obj => obj.ReportType == type).ToList();
            return View(reports);
        }

        public ActionResult Report(Guid id)
        {
            var report = _ctx.Reports.First(obj => obj.Id == id);
            return View(JsonConvert.DeserializeObject<GeneralReportViewModel>(report.ReportDataJson));
        }
    }
}