using System;
using Dust.Platform.Storage.Model;
// ReSharper disable InconsistentNaming

namespace Dust.Platform.Service.Models
{
    public class DistrictDetailViewModel
    {
        public string districtName { get; set; }

        public string name { get; set; }

        public double tsp { get; set; }

        public double pm25 { get; set; }

        public double pm100 { get; set; }

        public int rate { get; set; }
    }

    public class DistrictDetailPostParams
    {
        public ProjectType ProjectType { get; set; }

        public Guid District { get; set; }
    }
}