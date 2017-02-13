﻿using System.Collections.Generic;
using Dust.Platform.Storage.Model;

namespace Ks.Dust.Camera.MainControl.Storage
{
    public class CameraNodeStorage
    {
        public List<CameraNode> Nodes { get; set; }

        public List<CameraLogin> Logins { get; set; }
    }
}
