using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models;
using SHWDTech.Platform.Utility;

// ReSharper disable PossibleInvalidOperationException

namespace Dust.Platform.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public ProfileController()
        {
            _ctx = new KsDustDbContext();
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Project()
        {
            LoadSelections();
            return View();
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Edit(Guid id)
        {
            var project = _ctx.KsDustProjects.Include("District").Include("Enterprise").First(p => p.Id == id);
            var model = new ManualOuterProjectViewModel
                        {
                            Id               = project.Id,
                            District         = project.District.Name,
                            ProjectType      = project.ProjectType,
                            ConstructionUnit = project.ConstructionUnit,
                            EnterpriseId     = project.Enterprise.OuterId,
                            Enterprise       = project.Enterprise.Name,
                            ContractRecord   = project.ContractRecord,
                            Project          = project.Name,
                            Address          = project.Address,
                            CityArea         = project.CityArea,
                            Superintend      = project.SuperIntend,
                            Mobile           = project.Mobile,
                            OccupiedArea     = project.OccupiedArea,
                            Floorage         = project.Floorage
                        };
            LoadSelections();
            return View("Project", model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Edit([FromBody] ManualOuterProjectViewModel model)
        {
            LoadSelections();
            try
            {
                var project = _ctx.KsDustProjects.First(p => p.Id == model.Id.Value);
                model.Trim();
                var district = _ctx.Districts.FirstOrDefault(d => d.Id.ToString() == model.District);
                if (district == null)
                {
                    ModelState.AddModelError("Save", $@"不存在此区县：{model.District}");
                    return View("Project", model);
                }
                var enterprise = _ctx.Enterprises.FirstOrDefault(e => e.OuterId == model.EnterpriseId.Trim());
                if (enterprise == null)
                {
                    enterprise = new Enterprise
                                 {
                                     Id      = Guid.NewGuid(),
                                     Mobile  = model.Mobile,
                                     Name    = model.Enterprise,
                                     OuterId = model.EnterpriseId
                                 };
                    _ctx.Enterprises.Add(enterprise);
                }
                else
                {
                    enterprise.Name   = model.Enterprise;
                    enterprise.Mobile = model.Mobile;
                }
                project.DistrictId       = district.Id;
                project.ProjectType      = model.ProjectType.Value;
                project.ConstructionUnit = model.ConstructionUnit;
                project.EnterpriseId     = enterprise.Id;
                project.ContractRecord   = model.ContractRecord;
                project.Address          = model.Address;
                project.CityArea         = model.CityArea.Value;
                project.SuperIntend      = model.Superintend;
                project.Mobile           = model.Mobile;
                project.OccupiedArea     = model.OccupiedArea.Value;
                project.Floorage         = model.Floorage.Value;
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = $"{DateTime.Now: yyyyMMddHHmmss}";
                LogService.Instance.Error($"更新工程成功，错误号：{errorCode}", ex);
                ModelState.AddModelError("Save", $@"更新工程成功，错误号：{errorCode}");
                return View("Project", model);
            }
            ModelState.AddModelError("Save", @"更新工程成功");
            return View("Project", model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Project([FromBody]ManualOuterProjectViewModel model)
        {
            LoadSelections();
            if (model == null || !ModelState.IsValid)
            {
                return View(model);
            }
            model.Trim();

            var district = _ctx.Districts.FirstOrDefault(d => d.Id.ToString() == model.District);
            if (district == null)
            {
                ModelState.AddModelError("Save", $@"不存在此区县：{model.District}");
                return View(model);
            }

            if (_ctx.KsDustProjects.Any(prj => prj.ContractRecord == model.ContractRecord))
            {
                ModelState.AddModelError("Save", $@"合同备案号已经存在：{model.ContractRecord.Trim()}");
                return View(model);
            }

            var enterprise = _ctx.Enterprises.FirstOrDefault(e => e.OuterId == model.EnterpriseId.Trim());
            if (enterprise == null)
            {
                enterprise = new Enterprise
                {
                    Id = Guid.NewGuid(),
                    Mobile = model.Mobile,
                    Name = model.Enterprise,
                    OuterId = model.EnterpriseId
                };
                _ctx.Enterprises.Add(enterprise);
            }

            var project = new KsDustProject
            {
                Id = Guid.NewGuid(),
                ProjectType = model.ProjectType.Value,
                Name = model.Project,
                CityArea = model.CityArea.Value,
                SuperIntend = model.Superintend,
                Mobile = model.Mobile,
                ContractRecord = model.ContractRecord,
                Address = model.Address,
                ConstructionUnit = model.ConstructionUnit,
                DistrictId = district.Id,
                EnterpriseId = enterprise.Id,
                OccupiedArea = model.OccupiedArea.Value,
                Floorage = model.Floorage.Value,
                Installed = true
            };
            _ctx.KsDustProjects.Add(project);

            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = $"{DateTime.Now: yyyyMMddHHmmss}";
                LogService.Instance.Error($"新增工程失败，错误号：{errorCode}", ex);
                ModelState.AddModelError("Save", $@"新增工程失败，错误号：{errorCode}");
                return View(model);
            }
            ModelState.AddModelError("Save", @"新增工程成功");
            return View(model);
        }

        private void LoadSelections()
        {
            ViewBag.Districts =
                _ctx.Districts.Where(dis => dis.Id != Guid.Empty).Select(d => new SelectListItem {Text = d.Name, Value = d.Id.ToString()}).ToList();
            ViewBag.ProjectType = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "建筑工地",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "市政工地",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "搅拌站工地",
                    Value = "2"
                },
            };
            ViewBag.CityArea = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "内环内",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "内环外",
                    Value = "1"
                }
            };
        }
    }
}