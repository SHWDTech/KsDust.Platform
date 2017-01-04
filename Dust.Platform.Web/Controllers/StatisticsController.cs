using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Ajax;
using Dust.Platform.Web.Models.Home;
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
            var districts = _ctx.Districts.Select(obj => new { obj.Id, obj.Name }).ToList();
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

        public ActionResult HistoryRank(HistoryRankPost post)
        {
            var model = new HistoryViewModel
            {
                Title = post.Type == 0 ? "区县数据排名" : "工程颗粒排名",
                Uuid = post.Uuid,
                Type = post.Type
            };
            return View(model);
        }

        public ActionResult HistoryRankChart(HistoryRankChartPost post)
        {
            var query =
                _ctx.AverageMonitorDatas.Where(
                    obj =>
                        obj.Category == post.Type && obj.AverageDateTime > post.Start &&
                        obj.AverageDateTime < post.End);
            if (post.Type == AverageCategory.District)
            {
                var avgs = query.GroupBy(obj => obj.TargetId).Select(item => new
                {
                    _ctx.Districts.FirstOrDefault(obj => obj.Id == item.Key).Name,
                    Avg = item.Any() ? Math.Round(item.Average(val => val.ParticulateMatter), 2) : 0
                }).ToList();

                return Json(avgs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var avgs = query.GroupBy(obj => obj.TargetId).Select(item => new
                {
                    _ctx.KsDustProjects.FirstOrDefault(obj => obj.Id == item.Key).Name,
                    Avg = item.Any() ? Math.Round(item.Average(val => val.ParticulateMatter), 2) : 0
                }).OrderByDescending(avg => avg.Avg).Take(10).ToList();

                return Json(avgs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult HistoryRankTable(HistoryTablePost post)
        {

            var query =
               _ctx.AverageMonitorDatas.Where(
                   obj =>
                       obj.Type == post.DateType &&
                       obj.Category == post.type && obj.AverageDateTime > post.start &&
                       obj.AverageDateTime < post.end);
            if (post.type == AverageCategory.District)
            {
                var avgs = query.GroupBy(obj => obj.TargetId).Select(item => new
                {
                    _ctx.Districts.FirstOrDefault(obj => obj.Id == item.Key).Name,
                    Tsp = item.Any() ? Math.Round(item.Average(val => val.ParticulateMatter), 2) : 0,
                    Pm25 = item.Any() ? Math.Round(item.Average(val => val.Pm25), 2) : 0,
                    Pm100 = item.Any() ? Math.Round(item.Average(val => val.Pm100), 2) : 0,
                    Noise = item.Any() ? Math.Round(item.Average(val => val.Noise), 2) : 0,
                    Temp = item.Any() ? Math.Round(item.Average(val => val.Temperature), 2) : 0,
                    Humidity = item.Any() ? Math.Round(item.Average(val => val.Humidity), 2) : 0
                }).ToList();

                return Json(new
                {
                    total = avgs.Count,
                    rows = avgs
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var avgs = query.GroupBy(obj => obj.TargetId).Select(item => new
                {
                    _ctx.KsDustProjects.FirstOrDefault(obj => obj.Id == item.Key).Name,
                    Tsp = item.Any() ? Math.Round(item.Average(val => val.ParticulateMatter), 2) : 0,
                    Pm25 = item.Any() ? Math.Round(item.Average(val => val.Pm25), 2) : 0,
                    Pm100 = item.Any() ? Math.Round(item.Average(val => val.Pm100), 2) : 0,
                    Noise = item.Any() ? Math.Round(item.Average(val => val.Noise), 2) : 0,
                    Temp = item.Any() ? Math.Round(item.Average(val => val.Temperature), 2) : 0,
                    Humidity = item.Any() ? Math.Round(item.Average(val => val.Humidity), 2) : 0
                });

                return Json(new
                {
                    total = avgs.Count(),
                    rows = avgs.OrderByDescending(avg => avg.Tsp).Skip(post.offset).Take(post.limit).ToList()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult HistoryQuery(HistoryQueryPost model)
        {
            return View(model);
        }

        public ActionResult HistoryStats(HistoryStatsViewModel model)
        {
            return View(model);
        }

        public ActionResult HistoryChart(HistoryQueryChartPost post)
        {
            var ret = new List<HistoryLineChartViewModel>();
            var query =
                _ctx.AverageMonitorDatas.Where(
                    obj =>
                        obj.Type == post.DateType &&
                        obj.Category == post.Type && obj.TargetId == post.Id && obj.AverageDateTime > post.Start &&
                        obj.AverageDateTime < post.End);
            switch (post.DataType)
            {
                case MonitorDataValueType.Pm:
                    ret.AddRange(query.Select(q => new { q.AverageDateTime, q.ParticulateMatter }).ToList().Select(model => new HistoryLineChartViewModel
                    {
                        name = model.AverageDateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                        value =
                        {
                            [0] = model.AverageDateTime.ToString("yyyy-MM-dd HH:mm"), [1] = model.ParticulateMatter
                        }
                    }));
                    break;
                case MonitorDataValueType.Pm25:
                    ret.AddRange(query.Select(q => new { q.AverageDateTime, q.Pm25 }).ToList().Select(model => new HistoryLineChartViewModel
                    {
                        name = model.AverageDateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                        value =
                        {
                            [0] = model.AverageDateTime.ToString("yyyy-MM-dd HH:mm"), [1] = model.Pm25
                        }
                    }));
                    break;
                case MonitorDataValueType.Pm100:
                    ret.AddRange(query.Select(q => new { q.AverageDateTime, q.Pm100 }).ToList().Select(model => new HistoryLineChartViewModel
                    {
                        name = model.AverageDateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                        value =
                        {
                            [0] = model.AverageDateTime.ToString("yyyy-MM-dd HH:mm"), [1] = model.Pm100
                        }
                    }));
                    break;
                case MonitorDataValueType.Noise:
                    ret.AddRange(query.Select(q => new { q.AverageDateTime, q.Noise }).ToList().Select(model => new HistoryLineChartViewModel
                    {
                        name = model.AverageDateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                        value =
                        {
                            [0] = model.AverageDateTime.ToString("yyyy-MM-dd HH:mm"), [1] = model.Noise
                        }
                    }));
                    break;
                case MonitorDataValueType.Temperature:
                    ret.AddRange(query.Select(q => new { q.AverageDateTime, q.Temperature }).ToList().Select(model => new HistoryLineChartViewModel
                    {
                        name = model.AverageDateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                        value =
                        {
                            [0] = model.AverageDateTime.ToString("yyyy-MM-dd HH:mm"), [1] = model.Temperature
                        }
                    }));
                    break;
                case MonitorDataValueType.Humidity:
                    ret.AddRange(query.Select(q => new { q.AverageDateTime, q.Humidity }).ToList().Select(model => new HistoryLineChartViewModel
                    {
                        name = model.AverageDateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                        value =
                        {
                            [0] = model.AverageDateTime.ToString("yyyy-MM-dd HH:mm"), [1] = model.Humidity
                        }
                    }));
                    break;
                case MonitorDataValueType.WindSPeed:
                    ret.AddRange(query.Select(q => new { q.AverageDateTime, q.WindSpeed }).ToList().Select(model => new HistoryLineChartViewModel
                    {
                        name = model.AverageDateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                        value =
                        {
                            [0] = model.AverageDateTime.ToString("yyyy-MM-dd HH:mm"), [1] = model.WindSpeed
                        }
                    }));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HistoryQueryTable(HistoryQueryTablePost post)
        {
            var query =
               _ctx.AverageMonitorDatas.Where(
                   obj =>
                       obj.Type == AverageType.FifteenAvg &&
                       obj.TargetId == post.id &&
                       obj.Category == post.type && obj.AverageDateTime > post.start &&
                       obj.AverageDateTime < post.end).OrderByDescending(o => o.AverageDateTime);

            return Json(new
            {
                total = query.Count(),
                rows = query.Skip(post.offset).Take(post.limit).ToList().Select(q => new
                {
                    q.ParticulateMatter,
                    q.Pm25,
                    q.Pm100,
                    q.Noise,
                    q.Temperature,
                    q.Humidity,
                    q.WindSpeed,
                    AverageDateTime = q.AverageDateTime.ToString("yyyy-MM-dd HH:mm")
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HistoryStatsTable(HistoryStatsTablePost post)
        {
            var query =
               _ctx.AverageMonitorDatas.Where(
                   obj =>
                       obj.Type == post.dataType &&
                       obj.TargetId == post.id &&
                       obj.Category == post.type && obj.AverageDateTime > post.start &&
                       obj.AverageDateTime < post.end).OrderByDescending(o => o.AverageDateTime);

            return Json(new
            {
                total = query.Count(),
                rows = query.Skip(post.offset).Take(post.limit).ToList().Select(q => new
                {
                    q.ParticulateMatter,
                    q.Pm25,
                    q.Pm100,
                    q.Noise,
                    q.Temperature,
                    q.Humidity,
                    q.WindSpeed,
                    AverageDateTime = q.AverageDateTime.ToString("yyyy-MM-dd HH:mm")
                })
            }, JsonRequestBehavior.AllowGet);
        }
    }
}