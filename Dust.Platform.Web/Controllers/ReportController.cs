﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Storage.ViewModel;
using Dust.Platform.Web.Models.Home;
using Dust.Platform.Web.Models.Report;
using Dust.Platform.Web.Result;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;

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
    }
}