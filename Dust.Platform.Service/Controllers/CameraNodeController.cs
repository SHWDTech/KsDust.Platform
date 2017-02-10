using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/CameraNode")]
    public class CameraNodeController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public CameraNodeController()
        {
            _ctx = new KsDustDbContext();
        }

        public HttpResponseMessage Get()
        {
            var cameraNodes = new List<CameraNode>();
            foreach (var district in _ctx.Districts.Where(obj => obj.Id != Guid.Empty).ToList())
            {
                var disNodes = new CameraNode
                {
                    Id = district.Id,
                    Name = district.Name,
                    Category = AverageCategory.District
                };
                var devices = _ctx.KsDustDevices.Include("Project")
                    .Include("Project.District")
                    .Include("Project.Enterprise")
                    .Where(dev => dev.Project.DistrictId == district.Id && dev.Project.Id != Guid.Empty && !dev.Project.Stopped).ToList();
                var ents = devices.Select(dev => dev.Project.Enterprise).Distinct().ToList();
                if (ents.Any())
                {
                    disNodes.Children = new List<CameraNode>();
                }
                foreach (var ent in ents)
                {
                    var entNode = new CameraNode
                    {
                        Id = ent.Id,
                        Name = ent.Name,
                        Category = AverageCategory.Enterprise,
                        Children = new List<CameraNode>()
                    };
                    foreach (var prj in devices.Where(obj => obj.Project.EnterpriseId == ent.Id).Select(dev => dev.Project).Distinct().ToList())
                    {
                        var prjNode = new CameraNode
                        {
                            Id = prj.Id,
                            Name = $"{prj.Name}({prj.ContractRecord})",
                            Category = AverageCategory.Project,
                            Children = new List<CameraNode>()
                        };

                        foreach (var dev in devices.Where(obj => obj.ProjectId == prj.Id).ToList())
                        {
                            prjNode.Children.Add(new CameraNode
                            {
                                Id = dev.Id,
                                Name = dev.Name,
                                Category = AverageCategory.Device
                            });
                        }

                        entNode.Children.Add(prjNode);
                    }

                    disNodes.Children.Add(entNode);
                }

                cameraNodes.Add(disNodes);
            }

            var logins = _ctx.KsDustCameras.Select(obj => new CameraLogin()
            {
                SerialNumber = obj.SerialNumber,
                DeviceGuid = obj.DeviceId,
                User = obj.UserName,
                Password = obj.Password
            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new { Nodes = cameraNodes , Logins = logins});
        }
    }
}
