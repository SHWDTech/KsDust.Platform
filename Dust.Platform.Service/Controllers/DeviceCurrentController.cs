using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/DeviceCurrent")]
    public class DeviceCurrentController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public DeviceCurrentController()
        {
            _ctx = new KsDustDbContext();
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]DeviceCurrentPostParams model)
        {
            var device = _ctx.KsDustDevices.Include("Project").Include("Project.Enterprise").Include("Vendor").FirstOrDefault(dev => dev.Id == model.device);
            if (device == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "设备不存在。");
            }
            var current = new DeviceCurrentViewModel
            {
                name = device.Name,
                address = device.Project.Address,
                vendor = device.Vendor.Name,
                enterprise = device.Project.Enterprise.Name,
                superintend = device.Project.SuperIntend,
                mobile = device.Project.Mobile,
                isOnline = device.IsOnline,
            };
            var lastData =
                _ctx.KsDustMonitorDatas.Where(
                        obj => obj.MonitorType == MonitorType.RealTime && obj.DeviceId == device.Id)
                    .OrderByDescending(item => item.UpdateTime).FirstOrDefault();

            if (lastData == null)
            {
                current.updatetime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                current.tsp = lastData.ParticulateMatter;
                current.pm25 = lastData.Pm25;
                current.pm100 = lastData.Pm100;
                current.noise = lastData.Noise;
                current.temperature = lastData.Temperature;
                current.humidity = lastData.Humidity;
                current.windspeed = lastData.WindSpeed;
                current.winddirection = lastData.WindDirection;
                current.updatetime = lastData.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            current.ipServerAddr =
                _ctx.SystemConfigurations.First(obj => obj.ConfigName == "CameraIpServer").ConfigValue;

            var camera = _ctx.KsDustCameras.FirstOrDefault(car => car.DeviceId == device.Id);
            if (camera == null) return Request.CreateResponse(HttpStatusCode.OK, current);
            current.serialNumber = camera.SerialNumber;
            current.userName = camera.UserName;
            current.password = camera.Password;

            return Request.CreateResponse(HttpStatusCode.OK, current);
        }
    }
}
