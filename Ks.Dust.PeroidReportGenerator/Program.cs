using System;
using System.Collections.Generic;
using System.Linq;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.ViewModel;
using Newtonsoft.Json;

namespace Ks.Dust.PeroidReportGenerator
{
    class Program
    {
        private static readonly KsDustDbContext Ctx = new KsDustDbContext();

        private static string _reportType;

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                return (int)ExecuteResult.NoneArgument;
            }
            _reportType = args[0];
            int ret;
            try
            {
                ret = GenerateReport();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (int)ExecuteResult.StopWithException;
            }
            return ret;
        }

        private static int GenerateReport()
        {
            if (CheckExisted()) return (int)ExecuteResult.Done;
            var report = new GeneralReportViewModel { ReportTitle = MakeTitle() };
            CalcInstalled(report);
            GetDistrictReportData(report);
            ProcessProjectsRank(report);
            if (Storage(report) <= 0)
            {
                return (int)ExecuteResult.Failed;
            }

            return (int)ExecuteResult.Done;
        }

        private static bool CheckExisted()
        {
            var nowDate = $"{DateTime.Now:yyyy-MM}-01";
            switch (_reportType)
            {
                case CommandReportType.Month:
                    return Ctx.Reports.Any(rpt => rpt.ReportDate == nowDate && rpt.ReportType == ReportType.Month);
                case CommandReportType.Year:
                    return Ctx.Reports.Any(rpt => rpt.ReportDate == nowDate && rpt.ReportType == ReportType.Year);
            }

            return false;
        }

        private static string MakeTitle()
        {
            switch (_reportType)
            {
                case CommandReportType.Month:
                    return $"{DateTime.Now.Year}年{DateTime.Now.Month}月";
                case CommandReportType.Year:
                    return $"{DateTime.Now.Year}年";
                default:
                    return "";
            }
        }

        private static void CalcInstalled(GeneralReportViewModel model)
        {
            var devs = Ctx.KsDustDevices.Where(dev => dev.ProjectId != Guid.Empty);
            var totalInstalled = new DeviceInstalled
            {
                Total = devs.Count(dev => dev.Id != Guid.Empty),
                Stoped = devs.Count(dev => dev.Project.Stopped),
                Using = devs.Count(dev => !dev.Project.Stopped)
            };
            model.TotalInstalled = totalInstalled;
            devs = Ctx.KsDustDevices.Where(dev => dev.Project.ProjectType == ProjectType.ConstructionSite);
            var consDev = new DeviceInstalled
            {
                Total = devs.Count(dev => dev.Id != Guid.Empty),
                Stoped = devs.Count(dev => dev.Project.Stopped),
                Using = devs.Count(dev => !dev.Project.Stopped)
            };
            model.ConstructionSiteInstalled = consDev;
            devs = Ctx.KsDustDevices.Where(dev => dev.Project.ProjectType == ProjectType.MunicipalWorks);
            var munDev = new DeviceInstalled
            {
                Total = devs.Count(dev => dev.Id != Guid.Empty),
                Stoped = devs.Count(dev => dev.Project.Stopped),
                Using = devs.Count(dev => !dev.Project.Stopped)
            };
            model.MunicipalWorksInstalled = munDev;
            devs = Ctx.KsDustDevices.Where(dev => dev.Project.ProjectType == ProjectType.MixingPlant);
            var mixingDev = new DeviceInstalled
            {
                Total = devs.Count(dev => dev.Id != Guid.Empty),
                Stoped = devs.Count(dev => dev.Project.Stopped),
                Using = devs.Count(dev => !dev.Project.Stopped)
            };
            model.MixingPlantInstalled = mixingDev;
        }

        private static void GetDistrictReportData(GeneralReportViewModel model)
        {
            var dis = Ctx.Districts.Where(d => d.Id != Guid.Empty).ToList();
            var option = new ReportBarChartOption
            {
                title = "各区县试点工地颗粒物浓度评价",
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
            foreach (var district in dis)
            {
                var installed = new DistrictDeviceInstalled
                {
                    DistrictName = district.Name,
                    Total = Ctx.KsDustDevices.Count(dev => dev.Project.DistrictId == district.Id),
                    Stoped = Ctx.KsDustDevices.Count(dev => dev.Project.DistrictId == district.Id && dev.Project.Stopped),
                    Using = Ctx.KsDustDevices.Count(dev => dev.Project.DistrictId == district.Id && !dev.Project.Stopped),
                    ConstructionSiteInstalled = Ctx.KsDustDevices.Count(dev => dev.Project.DistrictId == district.Id && dev.Project.ProjectType == ProjectType.ConstructionSite),
                    MunicipalWorksInstalled = Ctx.KsDustDevices.Count(dev => dev.Project.DistrictId == district.Id && dev.Project.ProjectType == ProjectType.MunicipalWorks),
                    MixingPlantInstalled = Ctx.KsDustDevices.Count(dev => dev.Project.DistrictId == district.Id && dev.Project.ProjectType == ProjectType.MixingPlant)
                };
                model.DistrictInstalleds.Add(installed);
                option.xAxis.Add(district.Name);
                AverageMonitorData avg = null;
                if (_reportType == CommandReportType.Month)
                {
                    avg =
                        Ctx.AverageMonitorDatas.Where(a => a.TargetId == district.Id && a.Type == AverageType.MonthAvg)
                            .OrderByDescending(a => a.AverageDateTime)
                            .FirstOrDefault();
                }
                else if (_reportType == CommandReportType.Year)
                {
                    avg =
                        Ctx.AverageMonitorDatas.Where(a => a.TargetId == district.Id && a.Type == AverageType.Year)
                            .OrderByDescending(a => a.AverageDateTime)
                            .FirstOrDefault();
                }
                option.series[0].data.Add(avg?.ParticulateMatter ?? 0);
                option.series[1].data.Add(avg?.Pm25 ?? 0);
                option.series[2].data.Add(avg?.Pm100 ?? 0);
                model.DistrictAvgs.Add(new DistrictAvg
                {
                    DistrictName = district.Name,
                    AveragePm = avg?.ParticulateMatter ?? 0,
                    AveragePm25 = avg?.Pm25 ?? 0,
                    AveragePm100 = avg?.Pm100 ?? 0
                });
            }
            model.BarChartOption = option;
        }

        private static void ProcessProjectsRank(GeneralReportViewModel model)
        {
            List<AverageMonitorData> avgDatas = null;
            var compareDate = DateTime.Now.AddMonths(-1);
            if (_reportType == CommandReportType.Month)
            {
                avgDatas = Ctx.AverageMonitorDatas.Where(a => a.Category == AverageCategory.Project && a.Type == AverageType.MonthAvg && a.AverageDateTime > compareDate).ToList();
            }
            else if (_reportType == CommandReportType.Year)
            {
                avgDatas = Ctx.AverageMonitorDatas.Where(a => a.Category == AverageCategory.Project && a.Type == AverageType.Year && a.AverageDateTime > compareDate).ToList();
            }
            if (avgDatas == null) return;

            var topTen = avgDatas.OrderBy(d => d.ParticulateMatter).Take(10).ToArray();
            foreach (var averageMonitorData in topTen)
            {
                var project = Ctx.KsDustProjects.Include("Enterprise").Include("District").First(obj => obj.Id == averageMonitorData.TargetId);
                model.ProjectTopRanks.Add(new ProjectRank
                {
                    Average = averageMonitorData.ParticulateMatter,
                    ProjectName = project.Name,
                    EnterpriseName = project.Enterprise.Name,
                    DistrictName = project.District.Name
                });
            }
            var tailTen = avgDatas.OrderByDescending(d => d.ParticulateMatter).Take(10).ToArray();
            foreach (var averageMonitorData in tailTen)
            {
                var project = Ctx.KsDustProjects.Include("Enterprise").Include("District").First(obj => obj.Id == averageMonitorData.TargetId);
                model.ProjectTailRanks.Add(new ProjectRank
                {
                    Average = averageMonitorData.ParticulateMatter,
                    ProjectName = project.Name,
                    EnterpriseName = project.Enterprise.Name,
                    DistrictName = project.District.Name
                });
            }
        }

        private static int Storage(GeneralReportViewModel model)
        {
            var report = new Report
            {
                ReportDataJson = JsonConvert.SerializeObject(model),
                ReportType = _reportType == CommandReportType.Month ? ReportType.Month : ReportType.Year,
                ReportDate = $"{DateTime.Now:yyyy-MM}-01"
            };
            Ctx.Reports.Add(report);
            return Ctx.SaveChanges();
        }
    }

    class CommandReportType
    {
        public const string Month = "Month";

        public const string Year = "Year";
    }
}
