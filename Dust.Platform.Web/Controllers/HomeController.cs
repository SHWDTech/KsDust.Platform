﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Home;

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
            var wholeCity = new DistrictInfo
            {
                DistrictName = "全市",
                ProjectsCount = _ctx.KsDustProjects.Count(),
                ProjectsInstalled = _ctx.KsDustProjects.Count(obj => obj.Installed)
            };
            wholeCity.InstallPercentage = $"{wholeCity.ProjectsInstalled * 100.00d / wholeCity.ProjectsCount:F2}%";
            model.DistrictInfos.Add(wholeCity);
            var now = DateTime.Now.AddHours(-1);
            foreach (var district in _ctx.Districts.Select(obj => new { obj.Id, obj.Name }).ToList())
            {
                var info = new DistrictInfo
                {
                    DistrictName = district.Name,
                    ProjectsCount = _ctx.KsDustProjects.Count(obj => obj.DistrictId == district.Id),
                    ProjectsInstalled = _ctx.KsDustProjects.Count(obj => obj.DistrictId == district.Id && obj.Installed)
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
            foreach (var enterprise in _ctx.Enterprises.Select(obj => new { obj.Id, obj.Name }).ToList())
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
            foreach (var vendor in _ctx.Vendors.ToList())
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
            var model = new MonitorViewModel();
            foreach (var district in _ctx.Districts.ToList())
            {
                var disNodes = new Nodes
                {
                    name = district.Name,
                };
                var devices = _ctx.KsDustDevices.Include("Project")
                    .Include("Project.District")
                    .Include("Project.Enterprise")
                    .Where(dev => dev.Project.DistrictId == district.Id).ToList();
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
                        children = new List<Nodes>()
                    };
                    foreach (var prj in devices.Where(obj => obj.Project.EnterpriseId == ent.Id).Select(dev => dev.Project).Distinct().ToList())
                    {
                        var prjNode = new Nodes
                        {
                            name = prj.Name,
                            children = new List<Nodes>()
                        };

                        foreach (var dev in devices.Where(obj => obj.ProjectId == prj.Id).ToList())
                        {
                            prjNode.children.Add(new Nodes
                            {
                                name = dev.Name
                            });
                        }

                        entNode.children.Add(prjNode);
                    }

                    disNodes.children.Add(entNode);
                }

                model.TreeNodes.Add(disNodes);
            }

            return View(model);
        }

        public ActionResult MonitorContent(MonitorContentPost model)
        {
            return View(model);
        }
    }
}