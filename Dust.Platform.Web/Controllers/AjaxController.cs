using System;
using System.Web.Mvc;
using Dust.Platform.Web.Models.Ajax;
using System.Collections.Generic;
using System.Linq;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Web.Controllers
{
    public class AjaxController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public AjaxController()
        {
            _ctx = new KsDustDbContext();
        }

        // GET: Ajax
        public ActionResult MonitorData(MonitorDataPost model)
        {
            var ret = new List<MonitorChartData>();
            var query = _ctx.AverageMonitorDatas.Where(obj => obj.Category == model.tType && obj.Type == model.dCate);
            if (model.target != Guid.Empty)
            {
                query = query.Where(obj => obj.TargetId == model.target);
            }

            query = query.OrderByDescending(obj => obj.AverageDateTime).Take(model.count);
            switch (model.dType)
            {
                case MonitorDataValueType.Pm:
                    ret.AddRange(query.Select(obj => new MonitorChartData {value = obj.ParticulateMatter, date = obj.AverageDateTime}).ToList());
                    break;
                case MonitorDataValueType.Pm25:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Pm25, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.Pm100:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Pm100, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.Noise:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Noise, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.Temperature:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Temperature, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.Humidity:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Humidity, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.WindSPeed:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.WindSpeed, date = obj.AverageDateTime }).ToList());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Json(new
            {
                success = true,
                data = ret.Select(obj => new {obj.value, date = obj.date.ToString("yyyy-MM-dd HH:mm")}).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

    }
}