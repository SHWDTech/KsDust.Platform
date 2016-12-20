using System;
using System.Linq;
using System.Web.Http;
using Dust.Platform.Service.Models;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using SHWDTech.Platform.Utility;

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

        public OuterPlatformExecuteResult Post([FromBody]OuterProjectViewModel model )
        {
            var result = new OuterPlatformExecuteResult
            {
                Result = "failed"
            };

            if (model == null)
            {
                result.Message = "系统未收到任何信息。";
                return result;
            }

            if (string.IsNullOrWhiteSpace(model.ContracRecord))
            {
                result.Message = "未提供工程合同备案号。";
                return result;
            }

            if (_ctx.KsDustProjects.Any(prj => prj.ContracRecord == model.ContracRecord))
            {
                result.Message = "该工程已经存在";
                return result;
            }

            if (model.Devices == null || model.Devices.Length == 0)
            {
                result.Message = "未提供设备信息。";
                return result;
            }

            var district = _ctx.Districts.FirstOrDefault(d => d.Name == model.District);
            if (district == null)
            {
                result.Message = "所属区县不存在。";
                return result;
            }

            if (string.IsNullOrWhiteSpace(model.EnterpriseId))
            {
                result.Message = "未提供施工单位信息";
                return result;
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

            var project = new KsDustProject
            {
                Id = Guid.NewGuid(),
                Name = model.Project,
                ContracRecord = model.ContracRecord,
                Address = model.Address,
                ConstructionUnit = model.ConstructionUnit,
                DistrictId = district.Id,
                EnterpriseId = enterprise.Id,
                OccupiedArea = model.OccupiedArea,
                Floorage = model.Floorage,
            };

            foreach (var modelDevice in model.Devices)
            {
                var devId = Guid.Parse(modelDevice);
                var dev = _ctx.KsDustDevices.FirstOrDefault(de => de.Id == devId);
                if (dev == null)
                {
                    result.Message = "设备列表中包含不存在的设备。";
                    return result;
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
                result.Message = $"系统内部错误，错误号：{errorCode}";
                return result;
            }

            result.Result = "success";
            return result;
        }

        public OuterPlatformExecuteResult Delete([FromBody] string contractRecord)
        {
            var result = new OuterPlatformExecuteResult
            {
                Result = "failed"
            };

            var project = _ctx.KsDustProjects.FirstOrDefault(p => p.ContracRecord == contractRecord);
            if (project == null)
            {
                result.Message = "未找到合同备案号对应的工程。";
                return result;
            }
            project.Stopped = true;
            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var errorCode = $"{DateTime.Now: yyyyMMddHHmmss}";
                LogService.Instance.Error($"停止工程失败，错误号：{errorCode}", ex);
                result.Message = $"系统内部错误，错误号：{errorCode}";
                return result;
            }

            result.Result = "success";
            return result;
        }
    }
}
