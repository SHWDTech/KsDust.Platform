using System.Collections.Generic;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Service.Models
{
    public class CameraNodes
    {
        private List<CameraLogin> _cameraLogins;

        public string Name { get; set; }

        public List<CameraNodes> Children { get; set; }

        public List<CameraLogin> CamreaLogins
        {
            get
            {
                if (_cameraLogins != null) return _cameraLogins;
                _cameraLogins = new List<CameraLogin>();
                if (Children == null || Children.Count <= 0) return _cameraLogins;
                foreach (var child in Children)
                {
                    child.LoadCameraLogins(_cameraLogins);
                }

                return _cameraLogins;
            }
        }

        public void LoadCameraLogins(List<CameraLogin> cameraLogins)
        {
            cameraLogins.AddRange(CamreaLogins);
        }
    }
}