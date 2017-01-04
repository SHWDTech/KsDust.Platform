using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Home;
using Dust.Platform.Web.Models.Setting;
using Dust.Platform.Web.Models.Table;
using Dust.Platform.Web.Process;
using SHWDTech.Platform.Utility;

namespace Dust.Platform.Web.Controllers
{
    public class SettingController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public SettingController()
        {
            _ctx = new KsDustDbContext();
        }

        // GET: Setting
        public ActionResult Index()
        {
            var model = new SettingViewModel
            {
                MenuNodes = SettingMenu()
            };
            return View(model);
        }

        private List<Nodes> SettingMenu()
        {
            var nodes = new List<Nodes>
            {
                new Nodes
                {
                    id = "deviceMantance",
                    name = "设备维保",
                    ajaxurl = "/Setting/DeviceMantance",
                    callBack = "deviceMantanceTable"
                },
                new Nodes
                {
                    id = "deviceRegister",
                    name = "设备注册",
                    ajaxurl = "/Setting/DeviceRegister"
                }
            };

            return nodes;
        }

        public ActionResult DeviceMantance()
        {
            return View();
        }

        public ActionResult DeviceMantanceInfo(TablePost post)
        {
            var devs = _ctx.KsDustDevices.Select(dev => new
            {
                dev.Name,
                VendorName = dev.Vendor.Name,
                ProjectName = dev.Project.Name,
                dev.Project.SuperIntend,
                dev.Project.Mobile,
                dev.LastMaintenance
            }).ToList()
            .Select(obj => new
            {
                obj.Name,
                obj.VendorName,
                obj.ProjectName,
                obj.SuperIntend,
                obj.Mobile,
                LastMaintenance = obj.LastMaintenance.ToString("yyyy-MM-dd"),
                MantanceStatus = GetStatus(obj.LastMaintenance)
            }).ToList();

            return Json(new
            {
                total = devs.Count,
                rows = devs
            }, JsonRequestBehavior.AllowGet);
        }

        private MantanceStatus GetStatus(DateTime lastDateTime)
        {
            var diff = DateTime.Now - lastDateTime;
            if (diff.TotalDays > 180)
            {
                return MantanceStatus.Overdue;
            }
            return diff.TotalDays > 150 ? MantanceStatus.NeedMantance : MantanceStatus.Mantanced;
        }

        [HttpGet]
        public ActionResult DeviceRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeviceRegister(KsDustDevice model)
        {
            var user = AccountProcess.FindUserByName(HttpContext.User.Identity.Name);
            if (!AccountProcess.UserIsInRole(user.Id, "VendorManager"))
            {
                ModelState.AddModelError("Vendor", "只有设备提供商可以注册设备。");
                return View(model);
            }
            model.InstallDateTime = DateTime.Now;
            model.StartDateTime = DateTime.Now;
            model.LastMaintenance = DateTime.Now;
            model.VendorId = AccountProcess.FindVendorId(user);
            _ctx.KsDustDevices.Add(model);
            try
            {
                _ctx.SaveChanges();
            }
            catch
            {
                var errorCode = Globals.NewIdentityCode();
                LogService.Instance.Error($"新增设备失败，错误码：{errorCode}");
                ModelState.AddModelError("Save", $"保存设备信息失败，请联系管理员， 错误码：{errorCode}。");
                return View(model);
            }

            return View();
        }
    }
}