using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Storage.ViewModel;
using Dust.Platform.Web.Models.Home;
using Dust.Platform.Web.Models.Report;
using Dust.Platform.Web.Models.Table;
using Dust.Platform.Web.Result;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using SHWDTech.Platform.Utility;

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
                    routeValue = new
                    {
                        type = ReportType.Month
                    },
                    ajaxurl = "/Report/ReportSelecter",
                    callBack = "onLoadButtonClick"
                },
                new Nodes
                {
                    id = "yearReport",
                    name = "年报表",
                    routeValue = new
                    {
                        type = ReportType.Year
                    },
                    ajaxurl = "/Report/ReportSelecter",
                    callBack = "onLoadButtonClick"
                },
                new Nodes
                {
                    id = "onlineStatus",
                    name = "在线率报表",
                    routeValue = new
                    {
                        type = ReportType.OnlineStatus
                    },
                    ajaxurl = "/Report/OnLineStatus"
                },
                new Nodes
                {
                    id = "avgReport",
                    name = "均值排名报表",
                    ajaxurl = "/Report/AvgReport",
                    callBack = "setupAvgReport",
                    param = new
                    {
                        tableUrl = "/Report/AvgReportTable"
                    }
                }
            };

            return nodes;
        }

        public ActionResult ReportSelecter(ReportType type)
        {
            var reports = _ctx.Reports.Where(obj => obj.ReportType == type).ToList();
            return View(reports);
        }

        public ActionResult Report(long id)
        {
            var report = _ctx.Reports.First(obj => obj.Id == id);
            return View(JsonConvert.DeserializeObject<GeneralReportViewModel>(report.ReportDataJson));
        }

        public ActionResult ExportPeriodicReport(long id)
        {
            var report = JsonConvert.DeserializeObject<GeneralReportViewModel>(_ctx.Reports.First(r => r.Id == id).ReportDataJson);
            var excelPackage = new ExcelPackage();
            var barSheet = excelPackage.Workbook.Worksheets.Add("barChart");
            var barChart = barSheet.Drawings.AddChart("barChart", eChartType.ColumnClustered);
            for (var i = 1; i < 8; i++)
            {
                barSheet.Column(i).Width = 20;
            }

            barSheet.Column(2).Style.WrapText = true;
            barSheet.Column(4).Style.WrapText = true;
            using (var range = barSheet.Cells["A1:G1"])
            {
                range.Merge = true;
                range.Style.Font.Size = 22;
                range.Value = report.ReportTitle;
            }

            //监测点总体情况
            using (var range = barSheet.Cells["A2:G2"])
            {
                range.Merge = true;
                range.Style.Font.Size = 22;
                range.Value = $"{report.ReportTitle} - 监测点总体情况";
            }
            barSheet.Cells["A3"].Value = "类型";
            barSheet.Cells["B3"].Value = "安装量";
            barSheet.Cells["C3"].Value = "在用量";
            barSheet.Cells["D3"].Value = "停用量";
            using (var range = barSheet.Cells["A3:G3"])
            {
                range.Style.Font.Size = 14;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9edf7"));
            }
            barSheet.Cells["A4"].Value = "全市";
            barSheet.Cells["B4"].Value = report.TotalInstalled.Total;
            barSheet.Cells["C4"].Value = report.TotalInstalled.Using;
            barSheet.Cells["D4"].Value = report.TotalInstalled.Stoped;
            barSheet.Cells["A5"].Value = "建筑工地";
            barSheet.Cells["B5"].Value = report.ConstructionSiteInstalled.Total;
            barSheet.Cells["C5"].Value = report.ConstructionSiteInstalled.Using;
            barSheet.Cells["D5"].Value = report.ConstructionSiteInstalled.Stoped;
            barSheet.Cells["A6"].Value = "市政工地";
            barSheet.Cells["B6"].Value = report.MunicipalWorksInstalled.Total;
            barSheet.Cells["C6"].Value = report.MunicipalWorksInstalled.Using;
            barSheet.Cells["D6"].Value = report.MunicipalWorksInstalled.Stoped;
            barSheet.Cells["A7"].Value = "拌合站";
            barSheet.Cells["B7"].Value = report.MixingPlantInstalled.Total;
            barSheet.Cells["C7"].Value = report.MixingPlantInstalled.Using;
            barSheet.Cells["D7"].Value = report.MixingPlantInstalled.Stoped;

            //区县设备总体情况
            using (var range = barSheet.Cells["A8:G8"])
            {
                range.Merge = true;
                range.Style.Font.Size = 22;
                range.Value = $"{report.ReportTitle} - 各区县监测点总体情况";
            }
            using (var range = barSheet.Cells["A9:G9"])
            {
                range.Style.Font.Size = 14;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9edf7"));
            }
            barSheet.Cells["A9"].Value = "区县名称";
            barSheet.Cells["B9"].Value = "安装量";
            barSheet.Cells["C9"].Value = "在用量";
            barSheet.Cells["D9"].Value = "停用量";
            barSheet.Cells["E9"].Value = "建筑工地在用";
            barSheet.Cells["F9"].Value = "市政工地在用";
            barSheet.Cells["G9"].Value = "拌合站在用";
            for (var i = 10; i < report.DistrictInstalleds.Count + 10; i++)
            {
                var installed = report.DistrictInstalleds[i - 10];
                barSheet.Cells[$"A{i}"].Value = installed.DistrictName;
                barSheet.Cells[$"B{i}"].Value = installed.Total;
                barSheet.Cells[$"C{i}"].Value = installed.Using;
                barSheet.Cells[$"D{i}"].Value = installed.Stoped;
                barSheet.Cells[$"E{i}"].Value = installed.ConstructionSiteInstalled;
                barSheet.Cells[$"F{i}"].Value = installed.MunicipalWorksInstalled;
                barSheet.Cells[$"G{i}"].Value = installed.MixingPlantInstalled;
            }

            var lastRow = report.DistrictInstalleds.Count + 11;

            //各区县试点工地颗粒物浓度柱状图
            using (var range = barSheet.Cells[$"A{lastRow}:G{lastRow}"])
            {
                range.Merge = true;
                range.Style.Font.Size = 22;
                range.Value = $"{report.ReportTitle} - 各区县试点工地颗粒物浓度柱状图";
            }
            barChart.SetSize(960, 400);
            barChart.SetPosition(lastRow, 5, 0, 5);
            barChart.Title.Text = "各区县试点工地颗粒物浓度评价";
            lastRow += 22;
            using (var range = barSheet.Cells[$"A{lastRow}:G{lastRow}"])
            {
                range.Merge = true;
                range.Style.Font.Size = 22;
                range.Value = $"{report.ReportTitle} - 各区县试点工地颗粒物平均浓度";
            }
            lastRow += 1;
            var start = lastRow;
            barSheet.Cells[$"A{lastRow}"].Value = "区县名称";
            barSheet.Cells[$"B{lastRow}"].Value = "颗粒物";
            barSheet.Cells[$"C{lastRow}"].Value = "PM2.5";
            barSheet.Cells[$"D{lastRow}"].Value = "PM10";
            using (var range = barSheet.Cells[$"A{lastRow}:G{lastRow}"])
            {
                range.Style.Font.Size = 14;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9edf7"));
            }
            lastRow += 1;
            foreach (var avg in report.DistrictAvgs)
            {
                barSheet.Cells[$"A{lastRow}"].Value = avg.DistrictName;
                barSheet.Cells[$"B{lastRow}"].Value = avg.AveragePm;
                barSheet.Cells[$"C{lastRow}"].Value = avg.AveragePm25;
                barSheet.Cells[$"D{lastRow}"].Value = avg.AveragePm100;
                lastRow++;
            }
            var end = lastRow;
            if ((end - start) > 1)
            {
                var seal = barChart.Series.Add(barSheet.Cells[$"B{start + 1}:B{end - 1}"],
                barSheet.Cells[$"A{start + 1}:A{end - 1}"]);
                seal.Header = "颗粒物";
                seal.Fill.Color = ColorTranslator.FromHtml("#3398DB");
                seal = barChart.Series.Add(barSheet.Cells[$"C{start + 1}:C{end - 1}"],
                    barSheet.Cells[$"A{start + 1}:A{end - 1}"]);
                seal.Header = "PM2.5";
                seal.Fill.Color = ColorTranslator.FromHtml("#449d44");
                seal = barChart.Series.Add(barSheet.Cells[$"D{start + 1}:D{end - 1}"],
                    barSheet.Cells[$"A{start + 1}:A{end - 1}"]);
                seal.Header = "PM10";
                seal.Fill.Color = ColorTranslator.FromHtml("#286090");
            }

            //工地前十名、后十名
            lastRow += 1;
            using (var range = barSheet.Cells[$"A{lastRow}:G{lastRow}"])
            {
                range.Merge = true;
                range.Style.Font.Size = 22;
                range.Value = $"{report.ReportTitle} - 评级为优的前十名工地";
            }
            lastRow += 1;
            using (var range = barSheet.Cells[$"A{lastRow}:G{lastRow}"])
            {
                range.Style.Font.Size = 14;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#dff0d8"));
            }
            barSheet.Cells[$"A{lastRow}"].Value = "颗粒物浓度（mg/m³）";
            barSheet.Cells[$"B{lastRow}"].Value = "工地名称";
            barSheet.Cells[$"C{lastRow}"].Value = "评级";
            barSheet.Cells[$"D{lastRow}"].Value = "建设单位";
            barSheet.Cells[$"E{lastRow}"].Value = "所属区县";
            lastRow += 1;
            foreach (var topRank in report.ProjectTopRanks)
            {
                barSheet.Cells[$"A{lastRow}"].Value = topRank.Average;
                barSheet.Cells[$"B{lastRow}"].Value = topRank.ProjectName;
                barSheet.Cells[$"C{lastRow}"].Value = topRank.Rank;
                barSheet.Cells[$"D{lastRow}"].Value = topRank.EnterpriseName;
                barSheet.Cells[$"E{lastRow}"].Value = topRank.DistrictName;
                lastRow += 1;
            }

            lastRow += 1;
            using (var range = barSheet.Cells[$"A{lastRow}:G{lastRow}"])
            {
                range.Merge = true;
                range.Style.Font.Size = 22;
                range.Value = $"{report.ReportTitle} - 评级为优的后十名工地";
            }
            lastRow += 1;
            using (var range = barSheet.Cells[$"A{lastRow}:G{lastRow}"])
            {
                range.Style.Font.Size = 14;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#f2dede"));
            }
            barSheet.Cells[$"A{lastRow}"].Value = "颗粒物浓度（mg/m³）";
            barSheet.Cells[$"B{lastRow}"].Value = "工地名称";
            barSheet.Cells[$"C{lastRow}"].Value = "评级";
            barSheet.Cells[$"D{lastRow}"].Value = "建设单位";
            barSheet.Cells[$"E{lastRow}"].Value = "所属区县";
            lastRow += 1;
            foreach (var topRank in report.ProjectTailRanks)
            {
                barSheet.Cells[$"A{lastRow}"].Value = topRank.Average;
                barSheet.Cells[$"B{lastRow}"].Value = topRank.ProjectName;
                barSheet.Cells[$"C{lastRow}"].Value = topRank.Rank;
                barSheet.Cells[$"D{lastRow}"].Value = topRank.EnterpriseName;
                barSheet.Cells[$"E{lastRow}"].Value = topRank.DistrictName;
                lastRow += 1;
            }

            return new ExcelResult(excelPackage, $"{report.ReportTitle}.xlsx");
        }

        public ActionResult AvgReport() => View();

        public ActionResult AvgReportTable(AvgReportTablePost post)
        {
            var avgs = _ctx.AverageMonitorDatas.Where(d => d.Type == AverageType.MonthAvg
                                                           && d.Category == AverageCategory.Project
                                                           && d.AverageDateTime > post.start
                                                           && d.AverageDateTime < post.end)
                .GroupBy(g => g.TargetId)
                .Select(t => new { Project = t.Key, Avg = t.Sum(p => p.ParticulateMatter) / t.Count() })
                .OrderBy(t => t.Avg).ToList();
            var table = avgs.Take((int)(avgs.Count * 1.0 / 100 * post.percent)).ToList();
            var title = $"{post.start:yyyy-MM-dd}至{post.end:yyyy-MM-dd}期间排名前{post.percent}工程报表";
            var report = new AvgRankReport
            {
                ReportTitle = title,
                FileName = title,
                Items = (from td in table
                    let project = _ctx.KsDustProjects.Include("Enterprise").FirstOrDefault(p => p.Id == td.Project)
                    select new AvgRankReportItem
                    {
                        Rank = table.IndexOf(td) + 1,
                        ProjectName = project?.Name,
                        EnterpriseName = project?.Enterprise.Name,
                        AvgPm = Math.Round(td.Avg, 3)
                    }).ToList()
            };
            var reportId = string.Empty;
            if (report.Items != null && report.Items.Count > 0)
            {
                reportId = Globals.NewIdentityCode();
                HttpContext.Cache.Insert(reportId, report, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration);
            }
            return Json(new
            {
                total = report.Items.Count,
                rows = report.Items,
                reportId
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OnLineStatus() => View();

        public ActionResult OnlineStatisticsDates(OnlineStatisticsDatesParam post)
        {
            var query = _ctx.OnlineStatisticses.Where(s => s.Category == post.ObjectType &&
                                                           s.StatusType == post.DateType);
            var ret = query.GroupBy(o => o.UpdateTime)
                .OrderByDescending(g => g.Key)
                .Select(item => item.Key)
                .ToList()
                .Select(obj => new
                {
                    id = obj.Ticks,
                    text = DatesString(obj, post.DateType)
                });

            return Json(new
            {
                ret
            }, JsonRequestBehavior.AllowGet);
        }

        private string DatesString(DateTime date, AverageType dateType)
        {
            switch (dateType)
            {
                case AverageType.DayAvg:
                    return $"{date:yyyy年MM月dd日}";
                case AverageType.MonthAvg:
                    return $"{date:yyyy年MM月}";
                case AverageType.Year:
                    return $"{date:yyyy年}";
            }

            return string.Empty;
        }

        public ActionResult OnlineStatisticsTargets(OnlineStatisticsDatesParam post)
        {
            switch (post.ObjectType)
            {
                case AverageCategory.District:
                    var disRet = _ctx.Districts.Where(dis => dis.Id != Guid.Empty).Select(d => new
                    {
                        id = d.Id,
                        text = d.Name
                    });
                    return Json(disRet, JsonRequestBehavior.AllowGet);
                case AverageCategory.Enterprise:
                    var entRet = _ctx.Enterprises.Where(ent => ent.Id != Guid.Empty).Select(d => new
                    {
                        id = d.Id,
                        text = d.Name
                    });
                    return Json(entRet, JsonRequestBehavior.AllowGet);
                case AverageCategory.Project:
                    var prjRet = _ctx.KsDustProjects.Where(prj => prj.Id != Guid.Empty).Select(d => new
                    {
                        id = d.Id,
                        text = d.Name
                    });
                    return Json(prjRet, JsonRequestBehavior.AllowGet);
                case AverageCategory.Device:
                    var devRet = _ctx.KsDustDevices.Where(dev => dev.Id != Guid.Empty).Select(d => new
                    {
                        id = d.Id,
                        text = d.Name
                    });
                    return Json(devRet, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public ActionResult OnlineStatisticsRankTable(OnlineStatisticsRankTablePost post)
        {
            var query = _ctx.OnlineStatisticses.Where(o => o.Category == post.Category
                                                           && o.StatusType == post.StatusType);
            var targetObjects = Request["TargetObjects[]"]?.Split(',').Select(Guid.Parse).ToList();
            if (targetObjects != null && targetObjects.Count > 0)
            {
                targetObjects.RemoveAll(o => o == Guid.Empty);
                query = query.Where(o => targetObjects.Contains(o.TargetGuid));
            }
            query = query.Where(q => q.UpdateTime == post.UpdateTime);
            var total = query.Count();
            var ranks = query
                .OrderBy(q => q.Id)
                .Skip(post.offset)
                .Take(post.limit)
                .Select(r => new
                {
                    r.TargetGuid,
                    r.Statistics,
                    r.UpdateTime
                })
                .ToList();
            var report = new OnlineStatisticsReport();
            switch (post.Category)
            {
                case AverageCategory.Device:
                    report.ReportTitle = "设备在线率统计";
                    report.Items = ranks.Select(r => new OnlineStatisticsReportItem
                    {
                        TargetName = _ctx.KsDustDevices.First(d => d.Id == r.TargetGuid).Name,
                        OnlineRank = $"{Math.Round(r.Statistics * 100, 3)}%",
                        DateTime = $"{r.UpdateTime:yyyy年MM月}"
                    }).ToList();
                    break;
                case AverageCategory.Project:
                    report.ReportTitle = "工程在线率统计";
                    report.Items = ranks.Select(r => new OnlineStatisticsReportItem
                    {
                        TargetName = _ctx.KsDustProjects.First(d => d.Id == r.TargetGuid).Name,
                        OnlineRank = $"{Math.Round(r.Statistics * 100, 3)}%",
                        DateTime = $"{r.UpdateTime:yyyy年MM月}"
                    }).ToList();
                    break;
                case AverageCategory.Enterprise:
                    report.ReportTitle = "企业在线率统计";
                    report.Items = ranks.Select(r => new OnlineStatisticsReportItem
                    {
                        TargetName = _ctx.Enterprises.First(d => d.Id == r.TargetGuid).Name,
                        OnlineRank = $"{Math.Round(r.Statistics * 100, 3)}%",
                        DateTime = $"{r.UpdateTime:yyyy年MM月}"
                    }).ToList();
                    break;
                case AverageCategory.District:
                    report.ReportTitle = "区县在线率统计";
                    report.Items = ranks.Select(r => new OnlineStatisticsReportItem
                    {
                        TargetName = _ctx.Districts.First(d => d.Id == r.TargetGuid).Name,
                        OnlineRank = $"{Math.Round(r.Statistics * 100, 3)}%",
                        DateTime = $"{r.UpdateTime:yyyy年MM月}"
                    }).ToList();
                    break;
            }

            var reportId = string.Empty;
            if (report.Items != null && report.Items.Count > 0)
            {
                reportId = Globals.NewIdentityCode();
                report.ReportTitle = report.Items[0].DateTime + report.ReportTitle;
                report.FileName = report.ReportTitle;
                HttpContext.Cache.Insert(reportId, report, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration);
            }
            return Json(new
            {
                total,
                rows = report.Items,
                reportId
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportOnlineStatusReport(string id)
        {
            var reportObj = HttpContext.Cache.Get(id);
            if (!(reportObj is OnlineStatisticsReport report)) return new HttpNotFoundResult();

            var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("在线率报表");
            worksheet.Column(1).Width = 40;
            for (var i = 2; i < 4; i++)
            {
                worksheet.Column(i).Width = 20;
            }
            using (var range = worksheet.Cells["A1:C1"])
            {
                range.Merge = true;
                range.Style.Font.Size = 22;
                range.Value = report.ReportTitle;
            }

            //数据表
            worksheet.Cells["A2"].Value = "对象名称";
            worksheet.Cells["B2"].Value = "在线率";
            worksheet.Cells["C2"].Value = "统计时间";
            using (var range = worksheet.Cells["A2:C2"])
            {
                range.Style.Font.Size = 14;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9edf7"));
            }
            for (var i = 3; i < report.Items.Count + 3; i++)
            {
                var item = report.Items[i - 3];
                worksheet.Cells[$"A{i}"].Value = item.TargetName;
                worksheet.Cells[$"B{i}"].Value = item.OnlineRank;
                worksheet.Cells[$"C{i}"].Value = item.DateTime;
            }

            return new ExcelResult(excelPackage, $"{report.FileName}.xlsx");
        }

        public ActionResult ExportAvgRankReport(string id)
        {
            var reportObj = HttpContext.Cache.Get(id);
            if (!(reportObj is AvgRankReport report)) return new HttpNotFoundResult();

            var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("均值排名报表");
            worksheet.Column(1).Width = 20;
            worksheet.Column(2).Width = 40;
            worksheet.Column(3).Width = 40;
            worksheet.Column(4).Width = 40;
            using (var range = worksheet.Cells["A1:D1"])
            {
                range.Merge = true;
                range.Style.Font.Size = 22;
                range.Value = report.ReportTitle;
            }

            //数据表
            worksheet.Cells["A2"].Value = "排名";
            worksheet.Cells["B2"].Value = "工程名称";
            worksheet.Cells["C2"].Value = "建设单位";
            worksheet.Cells["D2"].Value = "颗粒物均值(mg/m³)";
            using (var range = worksheet.Cells["A2:D2"])
            {
                range.Style.Font.Size = 14;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#d9edf7"));
            }
            for (var i = 3; i < report.Items.Count + 3; i++)
            {
                var item = report.Items[i - 3];
                worksheet.Cells[$"A{i}"].Value = item.Rank;
                worksheet.Cells[$"B{i}"].Value = item.ProjectName;
                worksheet.Cells[$"C{i}"].Value = item.EnterpriseName;
                worksheet.Cells[$"D{i}"].Value = item.AvgPm;
            }

            return new ExcelResult(excelPackage, $"{report.FileName}.xlsx");
        }
    }
}