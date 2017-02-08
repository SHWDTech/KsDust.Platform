using System;
using System.Collections.Generic;

namespace Dust.Platform.Storage.Model
{
    public class CameraNode
    {
        private List<CameraLogin> _cameraLogins;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<CameraNode> Children { get; set; }

        public List<CameraLogin> GetCamreaLogins()
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

        public void LoadCameraLogins(List<CameraLogin> cameraLogins)
        {
            cameraLogins.AddRange(GetCamreaLogins());
        }
    }
}