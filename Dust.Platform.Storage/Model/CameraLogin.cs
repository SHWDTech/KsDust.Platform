using System.Text;

namespace Dust.Platform.Storage.Model
{
    public class CameraLogin
    {
        public string DomainName { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public byte[] DomainBytes => Encoding.UTF8.GetBytes(DomainName);
    }
}
