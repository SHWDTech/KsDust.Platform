using System.Collections.Generic;
using System.Web.Mvc;
using Dust.Platform.Web.Models.Home;
using Dust.Platform.Web.Models.Report;

namespace Dust.Platform.Web.Controllers
{
    public class ReportController : Controller
    {
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
                    id = "dayReport",
                    name = "日报表"
                },
                new Nodes
                {
                    id = "weekReport",
                    name = "周报表"
                },
                new Nodes
                {
                    id = "monthReport",
                    name = "月报表"
                },
                new Nodes
                {
                    id = "yearReport",
                    name = "年报表"
                }
            };

            return nodes;
        }
    }
}