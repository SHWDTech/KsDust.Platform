using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Transactions;
using Dust.Platform.Web.Helper;

namespace Dust.Platform.Web.Controllers
{
    public class SettingController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public SettingController()
        {
            _ctx = new KsDustDbContext();
        }

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
            using (var authRepository = new AuthRepository())
            {
                var nodes = new List<Nodes>();
                var menus = authRepository.FindModuleByParentName(((DustPrincipal)User).Id, "设置", false).OrderBy(m => m.ModuleIndex);
                nodes.AddRange(menus.Select(module => new Nodes
                {
                    id = module.Id.ToString(),
                    name = module.ModuleName,
                    nodetype = "setting",
                    ajaxurl = $"/{module.Controller}/{module.Action}"
                }));

                return nodes;
            }
        }

        public ActionResult DeviceMantance() => View();

        public ActionResult DeviceMantanceInfo(DevMantanceTablePost post)
        {
            var overed = DateTime.Now.AddMonths(-6);
            var needMantance = DateTime.Now.AddMonths(-5);
            var query = _ctx.KsDustDevices.Where(obj => obj.ProjectId != Guid.Empty && !obj.Project.Stopped);
            var user = AccountProcess.FindUserByName(User.Identity.Name);
            if (AccountProcess.UserIsInRole(user.Id, "VendorManager"))
            {
                var vendorId = AccountProcess.FindVendorId(user);
                query = query.Where(v => v.VendorId == vendorId);
            }
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

        private static MantanceStatus GetStatus(DateTime lastDateTime)
        {
            var diff = DateTime.Now - lastDateTime;
            if (diff.TotalDays > 180)
            {
                return MantanceStatus.Overdue;
            }
            return diff.TotalDays > 150 ? MantanceStatus.NeedMantance : MantanceStatus.Mantanced;
        }

        private void LoadProjects()
        {
            ViewBag.ProjectSelectItems = _ctx.KsDustProjects.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();
        }

        [HttpGet]
        public ActionResult DeviceRegister()
        {
            LoadProjects();
            return View();
        }

        [HttpPost]
        public ActionResult DeviceRegister(DeviceRegisterViewModel model)
        {
            LoadProjects();

            var user = AccountProcess.FindUserByName(User.Identity.Name);
            if (user == null 
                || !AccountProcess.UserIsInRole(user.Id, "VendorManager")
                && !AccountProcess.UserIsInRole(user.Id, "admin"))
            {
                ModelState.Clear();
                ModelState.AddModelError(nameof(Vendor), @"只有管理员或设备供应商才能注册设备！");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (_ctx.KsDustDevices.Any(d => d.Name == model.Name))
            {
                ModelState.AddModelError("Save", @"已存在相同名称的设备");
                return View(model);
            }
            
            var vendorId = AccountProcess.FindVendorId(user);
            var vendor = _ctx.Vendors.First(v => v.Id == vendorId);
            model.NodeId = $"{vendor.ShortCode}{model.NodeId}";
            if (_ctx.KsDustDevices.Any(d => d.NodeId == model.NodeId))
            {
                ModelState.AddModelError("Save", @"已存在相同MN码的设备");
                return View(model);
            }
            var device = new KsDustDevice
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                NodeId = model.NodeId,
                InstallDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                LastMaintenance = DateTime.Now,
                VendorId = vendorId,
                Longitude = model.Longitude,
                Latitude = model.Latitude
            };


            _ctx.KsDustDevices.Add(device);
            if (!string.IsNullOrWhiteSpace(model.SerialNumber))
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    model.Name = model.SerialNumber;
                }
                var camera = new KsDustCamera
                {
                    Id = Guid.NewGuid(),
                    Device = device,
                    Name = model.CameraName,
                    SerialNumber = model.SerialNumber,
                    UserName = model.CameraUserName,
                    Password = model.CameraPassword
                };
                _ctx.KsDustCameras.Add(camera);
            }

            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = Globals.NewIdentityCode();
                LogService.Instance.Error($"新增设备失败，错误码：{errorCode}", ex);
                ModelState.AddModelError("Save", $@"保存设备信息失败，请联系管理员， 错误码：{errorCode}。");
                return View(model);
            }

            ModelState.Clear();
            ModelState.AddModelError("Save", @"保存设备信息成功");
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
                var nowDate = DateTime.Now;
                var record = new DeviceMantanceRecord
                {
                    Device = model.Device,
                    MantancePerson = model.MantancePerson,
                    MantanceReport = model.MantanceReport,
                    MantanceDateTime = nowDate
                };
                _ctx.DeviceMantanceRecords.Add(record);
                var dev = _ctx.KsDustDevices.FirstOrDefault(d => d.Id == model.Device);
                if (dev != null)
                {
                    dev.LastMaintenance = nowDate;
                }
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = Globals.NewIdentityCode();
                LogService.Instance.Error($"新增维保记录失败，错误码：{errorCode}", ex);
                ModelState.AddModelError("Save", $@"新增维保记录失败，请联系管理员， 错误码：{errorCode}。");
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
                    ModelState.AddModelError("AddSuccess", @"添加供应商成功！");
                }
                else
                {
                    _ctx.Vendors.Attach(model);
                    _ctx.Entry(model).State = EntityState.Modified;
                    _ctx.SaveChanges();
                    ModelState.AddModelError("UpdateSuccess", @"更新供应商信息成功！");
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
            using (var repo = new AuthRepository())
            {
                var total = repo.GetUserCount(null);
                var users = repo.GetUserTable(post.offset, post.limit);
                var rows = users.Select(u => new
                {
                    u.Id,
                    u.UserName,
                    UserRole = repo.GetDustRole(u).DisplayName,
                    u.PhoneNumber
                }).ToList();

                return Json(new
                {
                    total,
                    rows
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private void LoadRoles()
        {
            using (var authRepository = new AuthRepository())
            {
                ViewBag.Roles = authRepository.GetDustRoles(null)
                    .Select(r => new SelectListItem
                    {
                        Text = r.DisplayName,
                        Value = r.Id.ToString()
                    })
                    .ToList();
            }
        }

        [HttpGet]
        public ActionResult UpdateUser(string id)
        {
            using (var repo = new AuthRepository())
            {
                var user = repo.FindById(id).Result;
                if (user == null)
                {
                    ModelState.AddModelError("", @"未找到用户信息");
                    return View();
                }
                var model = new UserEditModel
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    UserRole = repo.GetDustRole(user).Id.ToString(),
                    UserRelateEntity = user.Claims.FirstOrDefault(c => c.ClaimType == nameof(UserRelatedEntity))?.ClaimValue
                };
                LoadRoles();

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult AddNewUser()
        {
            LoadRoles();

            return View();
        }

        [HttpPost]
        public ActionResult UpdateUser(UserEditModel model)
        {
            using (var repo = new AuthRepository())
            {
                var user = repo.FindById(model.Id).Result;
                using (var scope = new TransactionScope())
                {
                    user.UserName = model.UserName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Roles.Clear();
                    user.Roles.Add(new IdentityUserRole
                    {
                        UserId = user.Id,
                        RoleId = model.UserRole
                    });
                    RefreshUserRelatedEntity(user, model);
                    var ret = repo.Update(user);
                    scope.Complete();
                    ModelState.AddModelError(ret.Succeeded ? "Success" : "Failed", ret.Succeeded ? @"更新用户信息成功！" : string.Join("\r\n", ret.Errors));
                }

                LoadRoles();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult AddNewUser(NewUserModel model)
        {
            LoadRoles();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                using (var repo = new AuthRepository())
                {
                    using (var scope = new TransactionScope())
                    {
                        var ret = repo.RegisterUser(new UserModel
                        {
                            UserName = model.UserName,
                            Password = model.Password,
                            ConfirmPassword = model.ConfirmPassword,
                            PhoneNumber = model.PhoneNumber
                        });
                        if (ret.Result == null || !ret.Result.Succeeded)
                        {
                            ModelState.AddModelError("Failed", ret.Result == null ? "新增用户失败" : string.Join("\r\n", ret.Result.Errors));
                            return View(model);
                        }

                        var user = repo.FindByName(model.UserName);
                        user.Roles.Add(new IdentityUserRole
                        {
                            UserId = user.Id,
                            RoleId = model.UserRole
                        });
                        AddUserRelatedEntity(user, model);
                        var updateRet = repo.Update(user);
                        ModelState.AddModelError(updateRet.Succeeded ? "Success" : "Failed",
                            updateRet.Succeeded ? @"新增用户信息成功！" : string.Join("\r\n", updateRet.Errors));
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                var errorCode = Globals.NewIdentityCode();
                LogService.Instance.Error($"新增用户失败，错误码：{errorCode}", ex);
                ModelState.AddModelError("Save", $@"新增用户失败，请联系管理员， 错误码：{errorCode}。");
                return View(model);
            }

            return View(model);
        }

        private void AddUserRelatedEntity(IdentityUser user, NewUserModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.UserRelateEntity))
            {
                var relatedEntityClaim = new IdentityUserClaim
                {
                    UserId = user.Id,
                    ClaimType = nameof(UserRelatedEntity),
                    ClaimValue = model.UserRelateEntity
                };
                user.Claims.Add(relatedEntityClaim);
                var relatedEntity = new UserRelatedEntity
                {
                    User = Guid.Parse(user.Id)
                };
                _ctx.UserRelatedEntities.Add(relatedEntity);
                _ctx.SaveChanges();
            }
        }

        private void RefreshUserRelatedEntity(IdentityUser user, UserEditModel model)
        {
            var relatedEntityClaim = user.Claims.FirstOrDefault(c => c.ClaimType == nameof(UserRelatedEntity));

            if (!string.IsNullOrWhiteSpace(model.UserRelateEntity))
            {
                if (relatedEntityClaim == null)
                {
                    relatedEntityClaim = new IdentityUserClaim
                    {
                        UserId = user.Id,
                        ClaimType = nameof(UserRelatedEntity),
                        ClaimValue = model.UserRelateEntity
                    };
                    user.Claims.Add(relatedEntityClaim);
                }
                else
                {
                    relatedEntityClaim.ClaimValue = model.UserRelateEntity;
                }
                var relatedEntity = _ctx.UserRelatedEntities.FirstOrDefault(r => r.User.ToString() == user.Id);
                if (relatedEntity != null)
                {
                    _ctx.UserRelatedEntities.Remove(relatedEntity);
                }
                relatedEntity = new UserRelatedEntity
                {
                    User = Guid.Parse(user.Id),
                    Entity = Guid.Parse(model.UserRelateEntity)
                };
                _ctx.UserRelatedEntities.Add(relatedEntity);
                _ctx.SaveChanges();
            }
        }

        public ActionResult DeleteUser(string id)
        {
            using (var repo = new AuthRepository())
            {
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
        }

        public ActionResult RoleManager() => View();

        public ActionResult RoleTable(TablePost post)
        {
            using (var repo = new AuthRepository())
            {
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
            using (var repo = new AuthRepository())
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var ret = repo.ChangePassword(model.UserId, model.CurrentPassword, model.Password);
                ModelState.AddModelError(ret.Succeeded ? "Success" : "Failed", ret.Succeeded ? @"修改密码成功！" : string.Join("\r\n", ret.Errors));

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult RolePermissions(string id)
        {
            using (var repo = new AuthRepository())
            {
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
        }

        [HttpPost]
        public ActionResult RolePermissions(RolePermissionsModel model)
        {
            using (var repo = new AuthRepository())
            {
                var role = repo.FindDustRoleById(model.RoleId);
                model.RoleName = role.DisplayName;
                model.Permissions = repo.GetDustPermissions(null);

                var rolePermissions = Request[nameof(RolePermissions)]?.Split(',').Select(Guid.Parse).ToList();
                try
                {
                    repo.UpdateRolePermissions(model.RoleId, rolePermissions);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Failed", @"更新角色权限失败！");
                    model.RolePermissions = repo.FindRolePermissions(role);
                    return View(model);
                }

                ModelState.AddModelError("Success", @"更新角色权限成功！");
                model.RolePermissions = repo.FindRolePermissions(role);

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult DevicePreview()
        {
            var vendorList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = @"所有供应商",
                    Value = null
                }
            };
            var vendors = _ctx.Vendors.Where(ve => ve.Id != Guid.Empty);
            var user = AccountProcess.FindUserByName(User.Identity.Name);
            if (AccountProcess.UserIsInRole(user.Id, "VendorManager"))
            {
                var vendorId = AccountProcess.FindVendorId(user);
                vendors = vendors.Where(v => v.Id == vendorId);
            }
            var exists = vendors.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            }).ToList();
            vendorList.AddRange(exists);
            ViewBag.Vendors = vendorList;

            return View();
        }

        public ActionResult DevicePreviewTable(DevicePreviewTablePost post)
        {
            var devs = _ctx.KsDustDevices.AsQueryable();
            var user = AccountProcess.FindUserByName(User.Identity.Name);
            if (post.VendorGuid != null)
            {
                devs = devs.Where(d => d.VendorId == post.VendorGuid);
            }
            if (AccountProcess.UserIsInRole(user.Id, "VendorManager"))
            {
                var vendorId = AccountProcess.FindVendorId(user);
                devs = devs.Where(d => d.VendorId == vendorId);
            }

            var total = devs.Count();
            var rows = devs.Select(d => new DevicePreviewTable
            {
                Id = d.Id,
                DistrictGuid = d.Project.DistrictId,
                EnterpriseGuid = d.Project.EnterpriseId,
                ProjectGuid = d.ProjectId.Value,
                Name = d.Name,
                NodeId = d.NodeId,
                VendorName = d.Vendor.Name,
                ProjectName = d.Project.Name,
                SuperIntend = d.Project.SuperIntend,
                Mobile = d.Project.Mobile
            }).ToList();
            foreach (var row in rows)
            {
                var lastData = _ctx.KsDustMonitorDatas.Where(d => d.MonitorType == MonitorType.RealTime
                                                                  && d.DistrictId == row.DistrictGuid
                                                                  && d.EnterpriseId == row.EnterpriseGuid
                                                                  && d.ProjectId == row.ProjectGuid
                                                                  && d.DeviceId == row.Id)
                    .OrderByDescending(da => da.UpdateTime)
                    .FirstOrDefault();
                if (lastData != null)
                {
                    row.LastDataTime = $"{lastData.UpdateTime:yyyy-MM-dd HH:mm:ss}";
                }
            }

            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeviceHistoryData(Guid deviceGuid)
        {
            ViewBag.DevId = deviceGuid;
            return View();
        }

        public ActionResult DeviceHistoryDataTable(DeviceHistoryDataTablePost post)
        {
            var dev = _ctx.KsDustDevices.Include("Project").First(d => d.Id == post.devideGuid);
            var query = _ctx.KsDustMonitorDatas.Where(md => md.MonitorType == MonitorType.RealTime
                                                            && md.DistrictId == dev.Project.DistrictId
                                                            && md.EnterpriseId == dev.Project.EnterpriseId
                                                            && md.ProjectId == dev.ProjectId
                                                            && md.DeviceId == dev.Id);
            var ids = query
                .OrderByDescending(d => d.Id)
                .Skip(post.offset)
                .Take(post.limit)
                .Select(de => de.Id);

            var datas = _ctx.KsDustMonitorDatas.AsQueryable().Join(ids, m => m.Id, l => l, (m, l) => new
            {
                m.Id,
                m.ParticulateMatter,
                m.Pm25,
                m.Pm100,
                m.Noise,
                m.Temperature,
                m.Humidity,
                m.WindSpeed,
                m.UpdateTime
            }).ToList();
            var total = query.Count();
            var rows = datas.Select(q => new
            {
                q.ParticulateMatter,
                q.Pm25,
                q.Pm100,
                q.Noise,
                q.Temperature,
                q.Humidity,
                q.WindSpeed,
                UpdateTime = q.UpdateTime.ToString("yyyy-MM-dd HH:mm")
            });

            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AlarmPhotoTable(TablePost post)
        {
            var userId = Guid.Parse(((DustPrincipal)HttpContext.User).Id);
            var devs = AccountProcess.UserIsInRole(userId.ToString(), "admin") ? _ctx.KsDustDevices.AsQueryable() : _ctx.KsDustDevices.Where(d => d.VendorId == userId);
            var alarms = _ctx.KsDustAlarms.Include("Device").Where(a => devs.Contains(a.Device));
            var total = devs.Count();
            var rows = alarms.OrderByDescending(d => d.AlarmDateTime)
                .Skip(post.offset)
                .Take(post.limit)
                .ToList()
                .Select(q => new
                {
                    q.Id,
                    DeviceName = q.Device.Name,
                    AlarmDateTime = $"{q.AlarmDateTime:yyyy-MM-dd HH:mm:ss fff}",
                    q.AlarmValue
                });

            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExceedPhoto() => View();

        public ActionResult ViewExceedPhoto(Guid alarmIdGuid)
        {
            var images = _ctx.ExceedPhotos.Where(e => e.AlarmGuid == alarmIdGuid).ToList();
            var model = images.Select(r => new ExceedPhotoViewModel
            {
                PhotoName = $"{r.UploadDateTime:yyyy-MM-dd HH:mm:ss}",
                PhotoPath = r.PhotoPath.Replace("d:", string.Empty).Replace("\\", "/"),
                ThumbPath = r.PhotoThumbPath.Replace("d:", string.Empty).Replace("\\", "/")
            }).ToList();

            return View(model);
        }

        public ActionResult ExceedPhotoUpload(ExceedPhotoUploadViewModel model)
        {
            try
            {
                var bitmap = (Bitmap)Image.FromStream(model.File.InputStream);
                ImageHelper.GetImageThumbSize(bitmap, out int width, out int height);
                var thumb = bitmap.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                var fileName = $"{WebSiteConfigHelper.ExceedPhotoStoratePath}\\{model.Id}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss_fff}";
                var photoPath = $"{fileName}.png";
                var thumbPath = $"{fileName}_thumb_128.png";
                bitmap.Save(photoPath, ImageFormat.Png);
                thumb.Save(thumbPath, ImageFormat.Png);
                var info = new ExceedPhoto
                {
                    AlarmGuid = model.Id,
                    Comment = model.Comment,
                    PhotoPath = photoPath,
                    PhotoThumbPath = thumbPath,
                    UploadDateTime = DateTime.Now
                };
                _ctx.ExceedPhotos.Add(info);
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("保存图片失败！", ex);
                return Json(new
                {
                    successed = false
                });
            }
            return Json(new
            {
                successed = true
            });
        }

        [HttpGet]
        public ActionResult DeviceProjectBind(Guid deviceGuid)
        {
            var device = _ctx.KsDustDevices.FirstOrDefault(d => d.Id == deviceGuid);
            ViewBag.Projects = _ctx.KsDustProjects.Where(p => p.Id != Guid.Empty).Select(pro => new SelectListItem
            {
                Text = pro.Name,
                Value = pro.Id.ToString()
            }).ToList();
            return View(new ProjectBindViewModel
            {
                DeviceGuid = device?.Id,
                DeviceName = device?.Name,
                ProjectBindRequestGuid = device?.ProjectBindRequest
            });
        }

        [HttpPost]
        public ActionResult DeviceProjectBind(ProjectBindViewModel model)
        {
            var device = _ctx.KsDustDevices.FirstOrDefault(d => d.Id == model.DeviceGuid);
            ViewBag.Projects = _ctx.KsDustProjects.Where(p => p.Id != Guid.Empty).Select(pro => new SelectListItem
            {
                Text = pro.Name,
                Value = pro.Id.ToString()
            }).ToList();

            if (device != null)
            {
                device.ProjectBindRequest = model.ProjectBindRequestGuid;
                _ctx.SaveChanges();
                ModelState.AddModelError("Save", @"申请审核完成，请等待审核结果。");
            }
            return View(model);
        }

        public ActionResult DeviceProjectBindAudit() => View();

        public ActionResult DeviceProjectBindAuditTable(TablePost post)
        {
            var query = _ctx.KsDustDevices.Where(dev => dev.ProjectId == Guid.Empty && dev.ProjectBindRequest != null)
                .OrderBy(dev => dev.Id)
                .Skip(post.offset)
                .Take(post.limit);
            var total = query.Count();
            var rows = query.Select(q => new
            {
                q.Id,
                q.Name,
                VendorName = q.Vendor.Name,
                ProjectName = q.Project.Name
            }).ToList();
            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }
    }
}