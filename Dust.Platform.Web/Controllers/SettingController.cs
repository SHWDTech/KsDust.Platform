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
                    callBack = "deviceMantanceTable",
                    nodetype = "setting"
                },
                new Nodes
                {
                    id = "deviceRegister",
                    name = "设备注册",
                    nodetype = "setting",
                    ajaxurl = "/Setting/DeviceRegister"
                }
            };

            return nodes;
        }

        public ActionResult DeviceMantance() => View();

        public ActionResult DeviceMantanceInfo(DevMantanceTablePost post)
        {
            var overed = DateTime.Now.AddMonths(-6);
            var needMantance = DateTime.Now.AddMonths(-5);
            var query = _ctx.KsDustDevices.Where(obj => obj.ProjectId != Guid.Empty && !obj.Project.Stopped);
            switch (post.MantanceStatus)
            {
                case MantanceStatus.Overdue:
                    query = query.Where(obj => obj.LastMaintenance < overed);
                    break;
                case MantanceStatus.NeedMantance:
                    query = query.Where(obj => obj.LastMaintenance < needMantance && obj.LastMaintenance > overed);
                    break;
            }
            var devs = query.OrderBy(obj => obj.Id).Skip(post.offset).Take(post.limit).Select(dev => new
            {
                dev.Id,
                dev.Name,
                VendorName = dev.Vendor.Name,
                ProjectName = dev.Project.Name,
                dev.Project.SuperIntend,
                dev.Project.Mobile,
                dev.LastMaintenance
            }).ToList()
            .Select(obj => new
            {
                obj.Id,
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
                total = query.Count(),
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
        public ActionResult DeviceRegister() => View();

        [HttpPost]
        public ActionResult DeviceRegister(DeviceRegisterViewModel model)
        {
            var user = AccountProcess.FindUserByName(HttpContext.User.Identity.Name);
            if (!AccountProcess.UserIsInRole(user.Id, "VendorManager"))
            {
                ModelState.AddModelError("Vendor", "只有设备提供商可以注册设备。");
                return View(model);
            }
            model.Device.InstallDateTime = DateTime.Now;
            model.Device.StartDateTime = DateTime.Now;
            model.Device.LastMaintenance = DateTime.Now;
            model.Device.VendorId = AccountProcess.FindVendorId(user);
            _ctx.KsDustDevices.Add(model.Device);
            model.Camera.Device = model.Device;
            _ctx.KsDustCameras.Add(model.Camera);
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

        [HttpGet]
        public ActionResult EditDeviceMantanceRecord(Guid deviceGuid)
        {
            var dev = _ctx.KsDustDevices.FirstOrDefault(d => d.Id == deviceGuid);
            if (dev == null)
            {
                ViewBag.CustomerErrorMessage = "未找到指定设备信息。";
                return PartialView("Error");
            }
            var model = new EditDeviceMantanceViewModel
            {
                DeviceName = dev.Name,
                MantancePerson = User.Identity.Name,
                Device = dev.Id
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditDeviceMantanceRecord(EditDeviceMantanceViewModel model)
        {
            try
            {
                var record = new DeviceMantanceRecord
                {
                    Device = model.Device,
                    MantancePerson = model.MantancePerson,
                    MantanceReport = model.MantanceReport,
                    MantanceDateTime = DateTime.Now
                };
                _ctx.DeviceMantanceRecords.Add(record);
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = Globals.NewIdentityCode();
                LogService.Instance.Error($"新增维保记录失败，错误码：{errorCode}", ex);
                ModelState.AddModelError("Save", $"新增维保记录失败，请联系管理员， 错误码：{errorCode}。");
                return View(model);
            }

            return Json("添加成功！");
        }

        [HttpGet]
        public ActionResult DeviceMantanceRecord(Guid deviceGuid)
        {
            var dev = _ctx.KsDustDevices.FirstOrDefault(d => d.Id == deviceGuid);
            if (dev == null)
            {
                ViewBag.CustomerErrorMessage = "未找到指定设备信息。";
                return PartialView("Error");
            }

            ViewBag.TableTitle = $"{dev.Name} - 维保记录查询";
            return View();
        }

        [HttpGet]
        public ActionResult DeviceMantanceTable(DeviceMantanceRecordTablePost post)
        {
            var query = _ctx.DeviceMantanceRecords.Where(r => r.Device == post.DeviceGuid);
            var records = query.OrderBy(item => item.MantanceDateTime).Skip(post.offset).Take(post.limit)
                .ToList()
                .Select(obj => new
                {
                    obj.MantancePerson,
                    obj.MantanceReport,
                    MantanceDateTime = obj.MantanceDateTime.ToString("yyyy-MM-dd HH:mm:ss")
                });
            return Json(new
            {
                total = query.Count(),
                rows = records
            },JsonRequestBehavior.AllowGet);
        }
    }
}