using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using SHWDTech.Platform.Utility;
// ReSharper disable PossibleInvalidOperationException

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/Project")]
    [AllowAnonymous]
    public class ProjectController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        public ProjectController()
        {
            _ctx = new KsDustDbContext();
        }

        public HttpResponseMessage Post([FromBody]OuterProjectViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult(ModelState));
            }
            model.Trim();

            var district = _ctx.Districts.FirstOrDefault(d => d.Name == model.District);
            if (district == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"不存在此区县：{model.District}"));
            }

            if (_ctx.KsDustProjects.Any(prj => prj.ContractRecord == model.ContractRecord))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"合同备案号已经存在：{model.ContractRecord.Trim()}"));
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
            };

            foreach (var deviceNodeId in model.Devices)
            {
                var devId = deviceNodeId;
                var dev = _ctx.KsDustDevices.FirstOrDefault(de => de.NodeId == devId);
                if (dev == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"不存在的设备NODEID：{deviceNodeId}"));
                }
                if (dev.ProjectId != Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"设备已绑定其他工程，NODEID：{deviceNodeId}"));
                }
                project.VendorId = dev.VendorId;
                dev.ProjectId = project.Id;
            }
            project.Installed = true;
            _ctx.KsDustProjects.Add(project);

            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = $"{DateTime.Now: yyyyMMddHHmmss}";
                LogService.Instance.Error($"新增工程失败，错误号：{errorCode}", ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"新增工程失败，错误号：{errorCode}"));
            }
            return Request.CreateResponse(HttpStatusCode.Accepted, new OuterPlatformExecuteResult().Success());
        }

        public HttpResponseMessage Put([FromBody]OuterProjectViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult(ModelState));
            }
            model.Trim();

            var district = _ctx.Districts.FirstOrDefault(d => d.Name == model.District);
            if (district == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"不存在此区县：{model.District}"));
            }

            var project = _ctx.KsDustProjects.FirstOrDefault(obj => obj.ContractRecord == model.ContractRecord);
            if (project == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"不存的合同备案号：{model.District}"));
            }

            var enterprise = _ctx.Enterprises.FirstOrDefault(e => e.OuterId == model.EnterpriseId);
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

            project = new KsDustProject
            {
                Name = model.Project,
                CityArea = model.CityArea.Value,
                SuperIntend = model.Superintend,
                ContractRecord = model.ContractRecord,
                Address = model.Address,
                ConstructionUnit = model.ConstructionUnit,
                DistrictId = district.Id,
                EnterpriseId = enterprise.Id,
                OccupiedArea = model.OccupiedArea.Value,
                Floorage = model.Floorage.Value,
            };

            foreach (var deviceNodeId in model.Devices)
            {
                var devId = deviceNodeId;
                var dev = _ctx.KsDustDevices.FirstOrDefault(de => de.NodeId == devId);
                if (dev == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"不存在的设备NODEID：{deviceNodeId}"));
                }
                if (dev.ProjectId != Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"设备已绑定其他工程，NODEID：{deviceNodeId}"));
                }
                project.VendorId = dev.VendorId;
                dev.ProjectId = project.Id;
            }
            project.Installed = true;

            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = $"{DateTime.Now: yyyyMMddHHmmss}";
                LogService.Instance.Error($"修改工程信息失败，错误号：{errorCode}", ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult($"修改工程信息失败，错误号：{errorCode}"));
            }
            return Request.CreateResponse(HttpStatusCode.Accepted, new OuterPlatformExecuteResult().Success());
        }

        public HttpResponseMessage Delete([FromBody]ProjectDeleteParams model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new OuterPlatformExecuteResult(ModelState));
            }
            model.ContractRecord = model.ContractRecord.Trim();

            var project = _ctx.KsDustProjects.FirstOrDefault(p => p.ContractRecord == model.ContractRecord);
            if (project == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new OuterPlatformExecuteResult($"未找到合同备案号对应的工程。{model.ContractRecord}"));
            }
            project.Stopped = true;
            foreach (var device in _ctx.KsDustDevices.Where(dev => dev.ProjectId == project.Id))
            {
                device.ProjectId = Guid.Empty;
            }
            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = $"{DateTime.Now: yyyyMMddHHmmss}";
                LogService.Instance.Error($"停止工程失败，错误号：{errorCode}", ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"系统内部错误，错误号：{errorCode}");
            }

            return Request.CreateResponse(HttpStatusCode.OK, new OuterPlatformExecuteResult().Success());
        }
    }
}
