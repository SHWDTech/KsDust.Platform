using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Storage.ViewModel;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;

namespace Ks.Dust.Platform.TestConsole
{
    internal static class Program
    {
        private static void Main()
        {
            //GenerateReport();
            ExportExcelWithChart();
        }

        /// <summary>
        /// 生成昆山扬尘报表数据
        /// </summary>
        public static void GenerateReport()
        {
            var report = new GeneralReportViewModel()
            {
                ReportTitle = "2017年2月",
                TotalInstalled = new DeviceInstalled { Total = 50, Using = 44, Stoped = 6 },
                ConstructionSiteInstalled = new DeviceInstalled { Total = 35, Using = 33, Stoped = 2 },
                MunicipalWorksInstalled = new DeviceInstalled { Total = 10, Using = 8, Stoped = 2 },
                MixingPlantInstalled = new DeviceInstalled { Total = 5, Using = 3, Stoped = 2 }
            };

            var db = new KsDustDbContext();
            var districts = db.Districts.Where(obj => obj.Id != Guid.Empty).ToList();
            var disInstalled = new List<DistrictDeviceInstalled>();
            var avgs = new List<DistrictAvg>();
            var option = new ReportBarChartOption
            {
                title = "各区县试点工地颗粒物浓度图表",
                yAxisName = "颗粒物mg/m³"
            };
            option.series.Add(new BarOptionSeries
            {
                name = "PM",
                type = "bar"
            });
            option.series.Add(new BarOptionSeries
            {
                name = "PM2.5",
                type = "bar"
            });
            option.series.Add(new BarOptionSeries
            {
                name = "PM10",
                type = "bar"
            });
            foreach (var district in districts)
            {
                var disIns = new DistrictDeviceInstalled
                {
                    DistrictName = district.Name,
                    ConstructionSiteInstalled = 4,
                    MunicipalWorksInstalled = 1,
                    MixingPlantInstalled = 0
                };
                disInstalled.Add(disIns);
                var avg = new DistrictAvg
                {
                    DistrictName = district.Name,
                    AveragePm = 0.567,
                    AveragePm25 = 0.433,
                    AveragePm100 = 0.486
                };
                avgs.Add(avg);
                option.xAxis.Add(district.Name);
                option.series[0].data.Add(0.567);
                option.series[1].data.Add(0.443);
                option.series[2].data.Add(0.512);
            }

            report.DistrictInstalleds = disInstalled;
            report.DistrictAvgs = avgs;
            var projects = db.KsDustProjects.Include("District")
                .Include("Enterprise")
                .Where(obj => obj.Id != Guid.Empty)
                .Take(10)
                .ToList();
            var top = new List<ProjectRank>();
            var tail = new List<ProjectRank>();
            foreach (var ksDustProject in projects)
            {
                var t = new ProjectRank
                {
                    DistrictName = ksDustProject.District.Name,
                    EnterpriseName = ksDustProject.Enterprise.Name,
                    Average = 0.543,
                    ProjectName = ksDustProject.Name,
                    Rank = "优"
                };
                var ta = new ProjectRank
                {
                    DistrictName = ksDustProject.District.Name,
                    EnterpriseName = ksDustProject.Enterprise.Name,
                    Average = 0.543,
                    ProjectName = ksDustProject.Name,
                    Rank = "差"
                };
                top.Add(t);
                tail.Add(ta);
            }
            report.ProjectTopRanks = top;
            report.ProjectTailRanks = tail;
            report.BarChartOption = option;

            var json = JsonConvert.SerializeObject(report);
            using (var stream = new StreamWriter(File.OpenWrite(@"d:\mysql.json")))
            {
                stream.Write(json);
            }
        }

        public static void ExportExcelWithChart()
        {
            var ctx = new KsDustDbContext();
            var report = JsonConvert.DeserializeObject<GeneralReportViewModel>(ctx.Reports.First().ReportDataJson);
            using (var excelPackage = new ExcelPackage())
            {
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
                lastRow++;
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
                lastRow++;
                foreach (var avg in report.DistrictAvgs)
                {
                    barSheet.Cells[$"A{lastRow}"].Value = avg.DistrictName;
                    barSheet.Cells[$"B{lastRow}"].Value = avg.AveragePm;
                    barSheet.Cells[$"C{lastRow}"].Value = avg.AveragePm25;
                    barSheet.Cells[$"D{lastRow}"].Value = avg.AveragePm100;
                    lastRow++;
                }
                var end = lastRow;
                var seal = barChart.Series.Add(barSheet.Cells[$"B{start + 1}:B{end - 1}"], barSheet.Cells[$"A{start + 1}:A{end - 1}"]);
                seal.Header = "颗粒物";
                seal.Fill.Color = ColorTranslator.FromHtml("#3398DB");
                seal = barChart.Series.Add(barSheet.Cells[$"C{start + 1}:C{end - 1}"], barSheet.Cells[$"A{start + 1}:A{end - 1}"]);
                seal.Header = "PM2.5";
                seal.Fill.Color = ColorTranslator.FromHtml("#449d44");
                seal = barChart.Series.Add(barSheet.Cells[$"D{start + 1}:D{end - 1}"], barSheet.Cells[$"A{start + 1}:A{end - 1}"]);
                seal.Header = "PM10";
                seal.Fill.Color = ColorTranslator.FromHtml("#286090");

                //工地前十名、后十名
                lastRow++;
                using (var range = barSheet.Cells[$"A{lastRow}:G{lastRow}"])
                {
                    range.Merge = true;
                    range.Style.Font.Size = 22;
                    range.Value = $"{report.ReportTitle} - 评级为优的前十名工地";
                }
                lastRow++;
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
                lastRow++;
                foreach (var topRank in report.ProjectTopRanks)
                {
                    barSheet.Cells[$"A{lastRow}"].Value = topRank.Average;
                    barSheet.Cells[$"B{lastRow}"].Value = topRank.ProjectName;
                    barSheet.Cells[$"C{lastRow}"].Value = topRank.Rank;
                    barSheet.Cells[$"D{lastRow}"].Value = topRank.EnterpriseName;
                    barSheet.Cells[$"E{lastRow}"].Value = topRank.DistrictName;
                    lastRow++;
                }

                lastRow++;
                using (var range = barSheet.Cells[$"A{lastRow}:G{lastRow}"])
                {
                    range.Merge = true;
                    range.Style.Font.Size = 22;
                    range.Value = $"{report.ReportTitle} - 评级为优的后十名工地";
                }
                lastRow++;
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
                lastRow++;
                foreach (var topRank in report.ProjectTailRanks)
                {
                    barSheet.Cells[$"A{lastRow}"].Value = topRank.Average;
                    barSheet.Cells[$"B{lastRow}"].Value = topRank.ProjectName;
                    barSheet.Cells[$"C{lastRow}"].Value = topRank.Rank;
                    barSheet.Cells[$"D{lastRow}"].Value = topRank.EnterpriseName;
                    barSheet.Cells[$"E{lastRow}"].Value = topRank.DistrictName;
                    lastRow++;
                }

                excelPackage.File = new FileInfo(@"d:\\testBarExcel.xlsx");
                excelPackage.Save();
            }
        }
    }
}