using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Home;
using Dust.Platform.Web.Models.Table;
using Dust.Platform.Web.Process;
using Dust.Platform.Web.Result;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Dust.Platform.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public HomeController()
        {
            _ctx = new KsDustDbContext();
        }

        public ActionResult Index()
        {
            var model = new IndexViewModel();
            var projects = _ctx.KsDustProjects.Where(obj => obj.Id != Guid.Empty && !obj.Stopped);
            var wholeCity = new DistrictInfo
            {
                DistrictName = "全市",
                ProjectsCount = projects.Count(),
                ProjectsInstalled = projects.Count(obj => obj.Installed)
            };
            wholeCity.InstallPercentage = $"{wholeCity.ProjectsInstalled * 100.00d / wholeCity.ProjectsCount:F2}%";
            model.DistrictInfos.Add(wholeCity);
            var now = DateTime.Now.AddHours(-1);
            foreach (var district in _ctx.Districts.Where(obj => obj.Id != Guid.Empty).Select(obj => new { obj.Id, obj.Name }).ToList())
            {
                var info = new DistrictInfo
                {
                    DistrictName = district.Name,
                    ProjectsCount = projects.Count(obj => obj.DistrictId == district.Id),
                    ProjectsInstalled = projects.Count(obj => obj.DistrictId == district.Id && obj.Installed)
                };
                info.InstallPercentage = info.ProjectsCount == 0 ? "0.0%" : $"{info.ProjectsInstalled * 100.00d / info.ProjectsCount:F1}%";
                model.DistrictInfos.Add(info);
                var avg = _ctx.AverageMonitorDatas.FirstOrDefault(
                        obj =>
                            obj.Type == AverageType.HourAvg && obj.TargetId == district.Id &&
                            obj.AverageDateTime > now);
                if (avg == null) continue;
                var avgValue = new DistrictRank
                {
                    DistrictName = district.Name,
                    CurrentTsp = avg.ParticulateMatter
                };
                model.DistrictRanks.Add(avgValue);
            }
            foreach (var enterprise in _ctx.Enterprises.Where(obj => obj.Id != Guid.Empty).Select(obj => new { obj.Id, obj.Name }).ToList())
            {
                var info = new Enterprises
                {
                    EnterpriseName = enterprise.Name,
                    DevicesCount = _ctx.KsDustDevices.Count(dev => dev.Project.EnterpriseId == enterprise.Id),
                    OnlineCount = _ctx.KsDustDevices.Count(dev => dev.Project.EnterpriseId == enterprise.Id && dev.IsOnline),
                    OfflineCount = _ctx.KsDustDevices.Count(dev => dev.Project.EnterpriseId == enterprise.Id && !dev.IsOnline)
                };
                info.OnlinePercentage = info.OnlineCount == 0
                    ? "0.0%"
                    : $"{info.OnlineCount * 100.00d / info.DevicesCount:F1}%";
                model.Enterpriseses.Add(info);
            }
            foreach (var vendor in _ctx.Vendors.Where(obj => obj.Id != Guid.Empty).ToList())
            {
                var info = new VendorInfo
                {
                    VendroName = vendor.Name,
                    Susperintend = vendor.Susperintend,
                    Mobile = vendor.Mobile
                };
                model.VendorInfos.Add(info);
            }

            foreach (var prj in _ctx.AverageMonitorDatas.Where(obj =>
            obj.Category == AverageCategory.Project
            && obj.Type == AverageType.HourAvg
            && obj.AverageDateTime > now)
            .OrderByDescending(item => item.ParticulateMatter).Take(10).ToList())
            {
                var prjRank = new ProjectRank
                {
                    ProjectName = _ctx.KsDustProjects.First(obj => obj.Id == prj.TargetId).Name,
                    CurrentTsp = prj.ParticulateMatter
                };
                model.ProjectRanks.Add(prjRank);
            }

            return View(model);
        }

        public ActionResult Monitor()
        {
            var model = new MonitorViewModel { TreeNodes = GetMenuNodes() };
            return View(model);
        }

        public ActionResult MonitorContent(MonitorContentPost model)
        {
            model.Title = model.ViewType == AverageCategory.WholeCity ? "全市" : model.TargetName;

            var checkDate = DateTime.Now.AddDays(-1);
            model.MonitorDatas =
                _ctx.AverageMonitorDatas.Where(obj =>
                obj.Type == AverageType.HourAvg
                && obj.Category == model.ViewType
                && obj.TargetId == model.TargetId
                && obj.AverageDateTime > checkDate).ToList();

            switch (model.ViewType)
            {
                case AverageCategory.WholeCity:
                    model.DistrictInfos = new List<DistrictInfo>();
                    foreach (var district in _ctx.Districts.Where(obj => obj.Id != Guid.Empty).Select(obj => new { obj.Id, obj.Name }).ToList())
                    {
                        var info = new DistrictInfo
                        {
                            DistrictName = district.Name,
                            ProjectsCount = _ctx.KsDustProjects.Count(obj => obj.DistrictId == district.Id && !obj.Stopped),
                            ProjectsInstalled = _ctx.KsDustProjects.Count(obj => obj.DistrictId == district.Id && obj.Installed && !obj.Stopped)
                        };
                        info.InstallPercentage = info.ProjectsCount == 0 ? "0.0%" : $"{info.ProjectsInstalled * 100.00d / info.ProjectsCount:F1}%";
                        model.DistrictInfos.Add(info);
                    }
                    break;
                case AverageCategory.District:
                    model.DistrictStatuses = new List<DistrictStatus>();
                    var projects = _ctx.KsDustProjects.Where(prj => prj.DistrictId == model.TargetId && !prj.Stopped).Select(obj => new { obj.Id, obj.Name, obj.OccupiedArea, obj.Floorage }).ToList();
                    var total = new DistrictStatus
                    {
                        ProjectName = "全区",
                        DevicesCount = _ctx.KsDustDevices.Count(obj => obj.Project.DistrictId == model.TargetId && !obj.Project.Stopped),
                        TotalOccupiedArea = projects.Sum(obj => obj.OccupiedArea),
                        TotalFloorage = projects.Sum(obj => obj.Floorage)
                    };
                    model.DistrictStatuses.Add(total);
                    foreach (var project in projects)
                    {
                        var statu = new DistrictStatus
                        {
                            ProjectName = project.Name,
                            DevicesCount = _ctx.KsDustDevices.Count(obj => obj.ProjectId == project.Id),
                            TotalOccupiedArea = project.OccupiedArea,
                            TotalFloorage = project.Floorage
                        };
                        model.DistrictStatuses.Add(statu);
                    }
                    break;
                case AverageCategory.Enterprise:
                    model.DistrictStatuses = new List<DistrictStatus>();
                    var entprojects = _ctx.KsDustProjects.Where(prj => prj.EnterpriseId == model.TargetId && !prj.Stopped).Select(obj => new { obj.Id, obj.Name, obj.OccupiedArea, obj.Floorage }).ToList();
                    var enttotal = new DistrictStatus
                    {
                        ProjectName = "全区",
                        DevicesCount = _ctx.KsDustDevices.Count(obj => obj.Project.DistrictId == model.TargetId && !obj.Project.Stopped),
                        TotalOccupiedArea = entprojects.Sum(obj => obj.OccupiedArea),
                        TotalFloorage = entprojects.Sum(obj => obj.Floorage)
                    };
                    model.DistrictStatuses.Add(enttotal);
                    foreach (var project in entprojects)
                    {
                        var statu = new DistrictStatus
                        {
                            ProjectName = project.Name,
                            DevicesCount = _ctx.KsDustDevices.Count(obj => obj.ProjectId == project.Id),
                            TotalOccupiedArea = project.OccupiedArea,
                            TotalFloorage = project.Floorage
                        };
                        model.DistrictStatuses.Add(statu);
                    }
                    break;
                case AverageCategory.Project:
                    model.Project = _ctx.KsDustProjects.Include(nameof(District))
                        .Include(nameof(Vendor))
                        .Include(nameof(Enterprise))
                        .First(obj => obj.Id == model.TargetId);
                    break;
                case AverageCategory.Device:
                    model.Device = _ctx.KsDustDevices.Include(nameof(Vendor))
                        .Include("Project")
                        .Include("Project.District")
                        .Include("Project.Enterprise")
                        .First(obj => obj.Id == model.TargetId);
                    break;
            }

            return View(model);
        }

        public ActionResult Statistics()
        {
            var model = new StatisticsViewModel
            {
                QueryMenuNodes = GetMenuNodes(),
                StatsMenuNodes = GetMenuNodes()
            };

            QueryMenuNodes(model.QueryMenuNodes);
            StatisticsMenuNodes(model.StatsMenuNodes);
            return View(model);
        }

        private static void QueryMenuNodes(List<Nodes> nodes)
        {
            foreach (var menuNode in nodes)
            {
                menuNode.ajaxurl = "/Statistics/HistoryQuery";
                menuNode.callBack = "setupHistoryQuery";
                menuNode.nodetype = "historyQuery";
                menuNode.tabTailfix = " - 历史查询";
                menuNode.routeValue = new
                {
                    ViewType = menuNode.viewType,
                    TargetId = menuNode.id,
                    TargetName = menuNode.name
                };
                if (menuNode.children != null)
                {
                    QueryMenuNodes(menuNode.children);
                }
            }
        }

        private static void StatisticsMenuNodes(List<Nodes> nodes)
        {
            foreach (var menuNode in nodes)
            {
                menuNode.ajaxurl = "/Statistics/HistoryStats";
                menuNode.callBack = "setupHistoryStats";
                menuNode.nodetype = "historyStats";
                menuNode.tabTailfix = " - 历史统计";
                menuNode.routeValue = new
                {
                    ViewType = menuNode.viewType,
                    TargetId = menuNode.id,
                    TargetName = menuNode.name
                };
                if (menuNode.children != null)
                {
                    QueryMenuNodes(menuNode.children);
                }
            }
        }

        private List<Nodes> GetMenuNodes()
        {
            var list = new List<Nodes>();
            foreach (var district in _ctx.Districts.Where(obj => obj.Id != Guid.Empty).ToList())
            {
                var disNodes = new Nodes
                {
                    name = district.Name,
                    id = district.Id.ToString(),
                    viewType = AverageCategory.District
                };
                var devices = _ctx.KsDustDevices.Include("Project")
                    .Include("Project.District")
                    .Include("Project.Enterprise")
                    .Where(dev => dev.Project.DistrictId == district.Id && dev.Project.Id != Guid.Empty && !dev.Project.Stopped).ToList();
                var ents = devices.Select(dev => dev.Project.Enterprise).Distinct().ToList();
                if (ents.Any())
                {
                    disNodes.children = new List<Nodes>();
                }
                foreach (var ent in ents)
                {
                    var entNode = new Nodes
                    {
                        name = ent.Name,
                        id = ent.Id.ToString(),
                        viewType = AverageCategory.Enterprise,
                        children = new List<Nodes>()
                    };
                    foreach (var prj in devices.Where(obj => obj.Project.EnterpriseId == ent.Id).Select(dev => dev.Project).Distinct().ToList())
                    {
                        var prjNode = new Nodes
                        {
                            name = $"{prj.Name}({prj.ContractRecord})",
                            id = prj.Id.ToString(),
                            viewType = AverageCategory.Project,
                            children = new List<Nodes>()
                        };

                        foreach (var dev in devices.Where(obj => obj.ProjectId == prj.Id).ToList())
                        {
                            prjNode.children.Add(new Nodes
                            {
                                name = dev.Name,
                                id = dev.Id.ToString(),
                                viewType = AverageCategory.Device
                            });
                        }

                        entNode.children.Add(prjNode);
                    }

                    disNodes.children.Add(entNode);
                }

                list.Add(disNodes);
            }

            return list;
        }

        public ActionResult MonitorLastDayData(HistoryQueryTablePost post)
        {
            var query =
               _ctx.AverageMonitorDatas.Where(
                   obj =>
                       obj.Type == AverageType.HourAvg &&
                       obj.TargetId == post.id &&
                       obj.Category == post.type && obj.AverageDateTime > post.start &&
                       obj.AverageDateTime < post.end);

            query = !string.IsNullOrWhiteSpace(post.sort) ? query.OrderByField(post.sort, post.order == "asc") : query.OrderByDescending(obj => obj.AverageDateTime);

            var rows = query.Skip(post.offset).Take(post.limit).ToList().Select(q => new
            {
                ParticulateMatter = Math.Round(q.ParticulateMatter, 2),
                Pm25 = Math.Round(q.Pm25, 2),
                Pm100 = Math.Round(q.Pm100, 2),
                Noise = Math.Round(q.Noise, 1),
                Temperature = Math.Round(q.Temperature, 1),
                Humidity = Math.Round(q.Humidity, 1),
                WindSpeed = Math.Round(q.WindSpeed, 1),
                AverageDateTime = q.AverageDateTime.ToString("yyyy-MM-dd HH:mm")
            }).ToList();

            if (post.act != "download")
                return Json(new
                {
                    total = query.Count(),
                    rows
                }, JsonRequestBehavior.AllowGet);
            var dt = new DataTable();
            dt.Columns.Add("TSP（mg/m³）", typeof(double));
            dt.Columns.Add("PM2.5（mg/m³）", typeof(double));
            dt.Columns.Add("PM10（mg/m³）", typeof(double));
            dt.Columns.Add("噪音（dB/A）", typeof(double));
            dt.Columns.Add("温度（℃）", typeof(double));
            dt.Columns.Add("湿度（%）", typeof(double));
            dt.Columns.Add("风速（m/s）", typeof(double));
            dt.Columns.Add("时间", typeof(DateTime));
            foreach (var dataRow in rows)
            {
                var row = dt.NewRow();
                row["TSP（mg/m³）"] = dataRow.ParticulateMatter;
                row["PM2.5（mg/m³）"] = dataRow.Pm25;
                row["PM10（mg/m³）"] = dataRow.Pm100;
                row["噪音（dB/A）"] = dataRow.Noise;
                row["温度（℃）"] = dataRow.Temperature;
                row["湿度（%）"] = dataRow.Humidity;
                row["风速（m/s）"] = dataRow.WindSpeed;
                row["时间"] = dataRow.AverageDateTime;
                dt.Rows.Add(row);
            }
            var package = new ExcelPackage();
            var currentSheet = package.Workbook.Worksheets.Add(post.title);
            for (var i = 1; i < 9; i++)
            {
                if (i < 8)
                {
                    currentSheet.Column(i).Width = 21;
                    currentSheet.Column(i).Style.Font.Size = 14;
                    currentSheet.Column(i).Style.Numberformat.Format = "0.00";
                    currentSheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else
                {
                    currentSheet.Column(i).Width = 35;
                    currentSheet.Column(i).Style.Font.Size = 14;
                    currentSheet.Column(i).Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
                    currentSheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            }

            using (var range = currentSheet.Cells["A1:H1"])
            {
                range.Merge = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Value = post.title;
                range.Style.Font.Size = 24;
            }

            using (var range = currentSheet.Cells["A2:H2"])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(26, 188, 156));
                range.Style.Font.Color.SetColor(Color.White);
            }

            currentSheet.View.FreezePanes(3, 9);
            currentSheet.Cells["A2"].LoadFromDataTable(dt, true);

            return new ExcelResult(package, $"{post.title}-{DateTime.Now:yyyy-MM-dd}.xlsx");
        }
    }
}