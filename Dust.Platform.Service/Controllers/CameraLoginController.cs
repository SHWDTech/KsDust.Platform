using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using SHWDTech.Platform.Utility.ExtensionMethod;
using SHWDTech.Platform.Utility.Serialize;

namespace Dust.Platform.Service.Controllers
{
    [RoutePrefix("api/CameraLogin")]
    public class CameraLoginController : ApiController
    {
        private readonly KsDustDbContext _ctx;

        private readonly RSACryptoServiceProvider _rsaCryptoServiceProvider = new RSACryptoServiceProvider();

        public CameraLoginController()
        {
            _ctx = new KsDustDbContext();
            _rsaCryptoServiceProvider.FromXmlString(BasicConfig.RsaPrivateKey);
        }

        [AllowAnonymous]
        public HttpResponseMessage Get([FromUri]Guid cameraId)
        {
            var camera = _ctx.KsDustCameras.FirstOrDefault(obj => obj.Id == cameraId);
            if (camera == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "未找到此摄像头信息。");
            var cameraLogin = new CameraLogin
            {
                SerialNumber = camera.SerialNumber,
                User = camera.UserName,
                IpServerAddr = _ctx.SystemConfigurations.First(obj => obj.ConfigName == "CameraIpServer").ConfigValue,
                Password = camera.Password
            };

            var encryptString = _rsaCryptoServiceProvider.EncryptString(XmlSerializerHelper.Serialize(cameraLogin));

            return Request.CreateResponse(HttpStatusCode.OK, Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptString)));
        }
    }
}
