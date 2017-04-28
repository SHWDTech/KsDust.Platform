using System;
using System.Web.Mvc;
using Dust.Platform.Web.Models.Ajax;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Account;
using SHWDTech.Platform.Utility.ExtensionMethod;
using SHWDTech.Platform.Utility.Serialize;

namespace Dust.Platform.Web.Controllers
{
    public class AjaxController : Controller
    {
        private readonly KsDustDbContext _ctx;

        public AjaxController()
        {
            _ctx = new KsDustDbContext();
        }

        // GET: Ajax
        public ActionResult MonitorData(MonitorDataPost model)
        {
            var ret = new List<MonitorChartData>();
            var query = _ctx.AverageMonitorDatas.Where(obj => obj.Category == model.tType && obj.Type == model.dCate);
            if (model.target != Guid.Empty)
            {
                query = query.Where(obj => obj.TargetId == model.target);
            }

            query = query.OrderByDescending(obj => obj.AverageDateTime).Take(model.count);
            switch (model.dType)
            {
                case MonitorDataValueType.Pm:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.ParticulateMatter, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.Pm25:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Pm25, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.Pm100:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Pm100, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.Noise:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Noise, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.Temperature:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Temperature, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.Humidity:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.Humidity, date = obj.AverageDateTime }).ToList());
                    break;
                case MonitorDataValueType.WindSPeed:
                    ret.AddRange(query.Select(obj => new MonitorChartData { value = obj.WindSpeed, date = obj.AverageDateTime }).ToList());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Json(new
            {
                success = true,
                data = ret.Select(obj => new { obj.value, date = obj.date.ToString("yyyy-MM-dd HH:mm") }).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Devices(DevicePostData model)
        {
            var devices = new List<Guid>();
            switch (model.viewType)
            {
                case AverageCategory.WholeCity:
                    devices.AddRange(_ctx.KsDustDevices.Select(obj => obj.Id).ToList());
                    break;
                case AverageCategory.District:
                    devices.AddRange(_ctx.KsDustDevices.Where(obj => obj.Project.DistrictId == model.targetId).Select(dev => dev.Id).ToList());
                    break;
                case AverageCategory.Enterprise:
                    devices.AddRange(_ctx.KsDustDevices.Where(obj => obj.Project.EnterpriseId == model.targetId).Select(dev => dev.Id).ToList());
                    break;
                case AverageCategory.Project:
                    devices.AddRange(_ctx.KsDustDevices.Where(obj => obj.ProjectId == model.targetId).Select(dev => dev.Id).ToList());
                    break;
                case AverageCategory.Device:
                    devices.AddRange(_ctx.KsDustDevices.Where(obj => obj.Id == model.targetId).Select(dev => dev.Id).ToList());
                    break;
            }

            return Json(devices, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeviceStatus(List<Guid> deviceList)
        {
            if (deviceList == null || deviceList.Count <= 0) return null;
            var ret = new List<DeviceCurrentStatus>();
            var devLocal =
                _ctx.KsDustDevices.Where(obj => deviceList.Contains(obj.Id))
                    .Select(dev => new { dev.Id, dev.Name, dev.Longitude, dev.Latitude }).ToList();
            foreach (var dev in devLocal)
            {
                var last =
                    _ctx.KsDustMonitorDatas.Where(obj => obj.DeviceId == dev.Id)
                        .OrderByDescending(item => item.UpdateTime)
                        .FirstOrDefault();
                ret.Add(new DeviceCurrentStatus
                {
                    name = dev.Name,
                    lat = dev.Latitude,
                    lon = dev.Longitude,
                    pm = last?.ParticulateMatter ?? 0,
                    noise = last?.Noise ?? 0,
                    time = last?.UpdateTime.ToString("yyyy-MM-dd HH:mm") ?? "无数据",
                    status = last == null ? Models.Ajax.DeviceStatus.OffLine : GetDeviceStatus(last.ParticulateMatter, last.UpdateTime)
                });
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        private DeviceStatus GetDeviceStatus(double pm, DateTime time)
        {
            if ((DateTime.Now - time).TotalMinutes > 15) return Models.Ajax.DeviceStatus.OffLine;
            if (pm < 0.4) return Models.Ajax.DeviceStatus.Good;
            return pm < 1 ? Models.Ajax.DeviceStatus.Alarm : Models.Ajax.DeviceStatus.Bad;
        }

        [HttpGet]
        public ActionResult CameraLogin(Guid devId)
        {
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            var key = _ctx.SystemConfigurations.First(
                    cfg => cfg.ConfigType == "RsaKeys" && cfg.ConfigName == "RsaPrivate").ConfigValue;
            rsaCryptoServiceProvider.FromXmlString(key);

            var camera = _ctx.KsDustCameras.FirstOrDefault(obj => obj.DeviceId == devId);
            if (camera == null) return Json(new
            {
                success = false
            }, JsonRequestBehavior.AllowGet);
            var cameraLogin = new CameraLogin
            {
                SerialNumber = camera.SerialNumber,
                User = camera.UserName,
                IpServerAddr = _ctx.SystemConfigurations.First(obj => obj.ConfigName == "CameraIpServer").ConfigValue,
                Password = camera.Password
            };

            var encryptString = rsaCryptoServiceProvider.EncryptString(XmlSerializerHelper.Serialize(cameraLogin));

            return Json(new
            {
                success = true,
                par = Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptString))
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClientNotices(NoticeClientType clientType)
        {
            var userId = Guid.Parse(((DustPrincipal)User).Id);
            var unReadCount = _ctx.UserClientNotices.Count(n => n.User == userId && n.NoticeClientType == clientType && !n.IsReaded);
            return Json(new
            {
                unReadCount
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RoleEntity(Guid roleId)
        {
            var repo = new AuthRepository();
            var role = repo.GetDustRole(roleId);
            var entities = new List<SelectListItem>();
            var hasEntitys = false;
            var labelContent = string.Empty;
            if (role.DisplayName == "工程管理员")
            {
                hasEntitys = true;
                labelContent = "所属工程";
                entities.AddRange(_ctx.KsDustProjects.Where(obj => obj.Id != Guid.Empty).Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }));
            }
            else if (role.DisplayName == "区县管理员")
            {
                hasEntitys = true;
                labelContent = "所属区县";
                entities.AddRange(_ctx.Districts.Where(obj => obj.Id != Guid.Empty).Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }));
            }

            return Json(new
            {
                hasEntitys,
                labelContent,
                entities = entities.Select(e => new
                {
                    id = e.Value,
                    text = e.Text
                })
            }, JsonRequestBehavior.AllowGet);
        }
    }
}