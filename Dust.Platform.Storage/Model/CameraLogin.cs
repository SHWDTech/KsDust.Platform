using System;
using System.Text;
using Newtonsoft.Json;

namespace Dust.Platform.Storage.Model
{
    public class CameraLogin
    {
        public string SerialNumber { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public Guid DeviceGuid { get; set; }

        public string IpServerAddr { get; set; }

        [JsonIgnore]
        public byte[] DomainBytes => Encoding.UTF8.GetBytes(SerialNumber);
    }
}
