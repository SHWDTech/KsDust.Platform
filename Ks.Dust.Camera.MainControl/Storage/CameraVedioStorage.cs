using System;

namespace Ks.Dust.Camera.MainControl.Storage
{
    public class CameraVedioStorage
    {
        public Guid DeviceGuid { get; set; }

        public string FileName { get; set; }

        public string FileDirectory { get; set; }

        public string FileFullPathWithName => $"{FileDirectory}\\{FileName}";
    }
}
