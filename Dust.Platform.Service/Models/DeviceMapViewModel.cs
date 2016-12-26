using System;
using System.ComponentModel.DataAnnotations;
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

        public string longitude { get; set; }

        public string latitude { get; set; }

        public bool isOnline { get; set; }
    }

    public class MapPostParams
    {
        public ProjectType? projectType { get; set; }
    }
}