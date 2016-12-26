using System;
using System.Web.Http;
using Dust.Platform.Storage.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Service.Process;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/Map")]
    public class MapController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public MapController()
        {
            _ctx = new KsDustDbContext();
        }

        public HttpResponseMessage Get([FromUri]MapPostParams model)
        {
            var mapList = new List<DeviceMapViewModel>();
            var authedDevices = model.projectType == null
                ? this.CreateFilterProcess().GetAuthedDevices(null)
                : this.CreateFilterProcess().GetAuthedDevices(dev => dev.Project.ProjectType == model.projectType.Value);
            var devices = authedDevices
                    .Select(dev => new { id = dev.Id, name = dev.Name, longitude = dev.Longitude, latitude = dev.Latitude })
                    .ToList();

            foreach (var device in devices)
            {
                var checkTime = DateTime.Now.AddMinutes(-15);
                var last = _ctx.KsDustMonitorDatas
                    .Where(d => d.DeviceId == device.id && d.MonitorType == MonitorType.FifteenMin && d.UpdateTime > checkTime)
                    .OrderBy(dat => dat.UpdateTime)
                    .FirstOrDefault();

                if (last == null)
                {
                    mapList.Add(new DeviceMapViewModel
                    {
                        id = device.id.ToString(),
                        name = device.name,
                        longitude = device.longitude,
                        latitude = device.latitude,
                        isOnline = false
                    });
                }
                else
                {
                    mapList.Add(new DeviceMapViewModel
                    {
                        id = device.id.ToString(),
                        name = device.name,
                        time = last.UpdateTime,
                        tsp = last.ParticulateMatter,
                        pm25 = last.Pm25,
                        pm100 = last.Pm100,
                        rate = Helper.GetRate(last.ParticulateMatter),
                        longitude = device.longitude,
                        latitude = device.latitude,
                        isOnline = true
                    });
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, mapList);
        }
    }
}
