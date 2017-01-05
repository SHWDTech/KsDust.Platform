using System;
using Dust.Platform.Storage.Model;

// ReSharper disable InconsistentNaming

namespace Dust.Platform.Service.Models
{
    public class DeviceMapViewModel
    {
        public string id { get; set; }

        public string name { get; set; }

        public DateTime time { get; set; }

        public double tsp { get; set; }

        public double pm25 { get; set; }

        public double pm100 { get; set; }

        public int rate { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }

        public bool isOnline { get; set; }
    }

    public class MapPostParams
    {
        public ProjectType? projectType { get; set; }

        public Guid? district { get; set; }

        public Guid? enterprise { get; set; }

        public Guid? project { get; set; }

        public Guid? device { get; set; }
    }
}