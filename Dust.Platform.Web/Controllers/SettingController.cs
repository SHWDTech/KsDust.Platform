using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Account;
using Dust.Platform.Web.Models.Home;
using Dust.Platform.Web.Models.Setting;
using Dust.Platform.Web.Models.Table;
using Dust.Platform.Web.Process;
using Microsoft.AspNet.Identity.EntityFramework;
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

            var menus = new AuthRepository().FindModuleByParentName(((DustPrincipal)User).Id, "设置", false);
            nodes.AddRange(menus.Select(module => new Nodes
            {
                id = module.Id.ToString(),
                name = module.ModuleName,
                nodetype = "setting",
                ajaxurl = $"/{module.Controller}/{module.Action}"
            }));

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
            var user = AccountProcess.FindUserByName(User.Identity.Name);
            if (user == null || !AccountProcess.UserIsInRole(user.Id, "VendorManager"))
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
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VendorsInfo() => View();

        public ActionResult VendorsTable(TablePost post)
        {
            var total = _ctx.Vendors.Count(v => v.Id != Guid.Empty);
            var rows = _ctx.Vendors.Where(ven => ven.Id != Guid.Empty).OrderBy(v => v.Id).Skip(post.offset).Take(post.limit);

            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Vendor(Guid id)
        {
            var model = id == Guid.Empty ? new Vendor() : _ctx.Vendors.FirstOrDefault(v => v.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Vendor(Vendor model)
        {
            try
            {
                if (model.Id == Guid.Empty)
                {
                    model.Id = Globals.NewCombId();
                    _ctx.Vendors.Add(model);
                    _ctx.SaveChanges();
                    ModelState.AddModelError("AddSuccess", "添加成功！");
                }
                else
                {
                    _ctx.Vendors.Attach(model);
                    _ctx.Entry(model).State = EntityState.Modified;
                    _ctx.SaveChanges();
                    ModelState.AddModelError("UpdateSuccess", "更新成功！");
                }
            }
            catch (Exception ex)
            {
                if (!(ex is DbEntityValidationException))
                {
                    ModelState.AddModelError("发生未知异常，请联系管理人员", ex.Message);
                }
                return View(model);
            }

            return View(model);
        }

        public ActionResult UserManager() => View();

        public ActionResult UserTable(TablePost post)
        {
            var repo = new AuthRepository();
            var total = repo.GetUserCount(null);
            var users = repo.GetUserTable(post.offset, post.limit);
            var rows = users.Select(u => new
            {
                u.Id,
                u.UserName,
                UserRole = repo.GetDustRole(u).DisplayName,
                u.PhoneNumber
            });

            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserEdit(string id)
        {
            var model = new UserEditModel
            {
                Id = id
            };
            var repo = new AuthRepository();
            ViewBag.Roles = repo.GetDustRoles(null)
                .Select(r => new SelectListItem
                {
                    Text = r.DisplayName,
                    Value = r.Id.ToString()
                })
                .ToList();
            var user = repo.FindById(id).Result;
            if (user == null)
            {
                return View(model);
            }
            model.Id = user.Id;
            model.UserName = user.UserName;
            model.PhoneNumber = user.PhoneNumber;
            model.UserRole = repo.GetDustRole(user).Id.ToString();

            return View(model);
        }

        [HttpPost]
        public ActionResult UserEdit(UserEditModel model)
        {
            var repo = new AuthRepository();
            ViewBag.Roles = repo.GetDustRoles(null)
                .Select(r => new SelectListItem
                {
                    Text = r.DisplayName,
                    Value = r.Id.ToString()
                })
                .ToList();
            var user = repo.FindById(model.Id).Result;
            return user != null ? UpdateUser(repo, user, model) : AddNewUser(repo, model);
        }

        private ActionResult UpdateUser(AuthRepository repo, IdentityUser user, UserEditModel model)
        {
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            if (user.Roles.Count > 0)
            {
                repo.UserRemoveFromRoles(user, user.Roles.Select(r => r.RoleId).ToArray());
            }
            repo.UserAddRole(user, model.UserRole);
            var ret = repo.Update(user);
            if (ret.Succeeded)
            {
                ModelState.AddModelError("Success", "更新成功！");
            }
            else
            {
                ModelState.AddModelError("Failed", "更新失败！");
            }

            return View(model);
        }

        private ActionResult AddNewUser(AuthRepository repo, UserEditModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("Failed", "两次输入的密码不一致！");
                return View(model);
            }
            var ret = repo.RegisterUser(new UserModel
            {
                UserName = model.UserName,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                PhoneNumber = model.PhoneNumber
            });
            if (!ret.Result.Succeeded)
            {
                ModelState.AddModelError("Failed", "新增用户失败！");
                return View(model);
            }

            var usr = repo.FindByName(model.UserName);
            repo.UserAddRole(usr, model.UserRole);

            ModelState.AddModelError("Success", "新增用户成功！");

            return View(model);
        }

        public ActionResult DeleteUser(string id)
        {
            var repo = new AuthRepository();
            if (repo.DeleteUser(id).Succeeded)
            {
                return Json(new
                {
                    message = "删除成功！"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                message = "删除失败！"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RoleManager() => View();

        public ActionResult RoleTable(TablePost post)
        {
            var repo = new AuthRepository();
            var total = repo.GetRoleCount(null);
            var roles = repo.GetDustRoleTable(post.offset, post.limit);
            var rows = roles.Select(u => new
            {
                u.Id,
                RoleName = u.DisplayName
            });

            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserPassword(string id, string name) => View(new UserPasswordModel
        {
            UserId = id,
            UserName = name
        });

        [HttpPost]
        public ActionResult UserPassword(UserPasswordModel model)
        {
            var repo = new AuthRepository();
            if (string.IsNullOrWhiteSpace(model.CurrentPassword))
            {
                ModelState.AddModelError("Failed", "请输入当前密码！");
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("Failed", "请输入新密码！");
                return View(model);
            }
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("Failed", "两次输入的密码不一致！");
                return View(model);
            }
            var ret = repo.ChangePassword(model.UserId, model.CurrentPassword, model.Password);
            if (ret.Succeeded)
            {
                ModelState.AddModelError("Success", "修改密码成功！");
            }
            else
            {
                ModelState.AddModelError("Failed", "修改密码失败！");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult RolePermissions(string id)
        {
            var repo = new AuthRepository();
            var role = repo.FindDustRoleById(id);
            var permissions = repo.FindRolePermissions(role);

            return View(new RolePermissionsModel
            {
                RoleId = role.Id.ToString(),
                RoleName = role.DisplayName,
                Permissions = repo.GetDustPermissions(null),
                RolePermissions = permissions
            });
        }

        [HttpPost]
        public ActionResult RolePermissions(RolePermissionsModel model)
        {
            var repo = new AuthRepository();
            var role = repo.FindDustRoleById(model.RoleId);
            model.RoleName = role.DisplayName;
            model.Permissions = repo.GetDustPermissions(null);

            var rolePermissions = Request["RolePermissions"]?.Split(',').Select(Guid.Parse).ToList();
            try
            {
                repo.UpdateRolePermissions(model.RoleId, rolePermissions);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Failed", "更新角色权限失败！");
                model.RolePermissions = repo.FindRolePermissions(role);
                return View(model);
            }

            ModelState.AddModelError("Success", "更新角色权限成功！");
            model.RolePermissions = repo.FindRolePermissions(role);

            return View(model);
        }
    }
}