using System;
using System.Collections.Generic;

namespace Dust.Platform.Storage.Model
{
    public class CameraNode
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public AverageCategory Category { get; set; }

        public List<CameraNode> Children { get; set; }
    }
}