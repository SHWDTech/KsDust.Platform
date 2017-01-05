// ReSharper disable InconsistentNaming

using System;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Service.Models
{
    public class DistrictAvgViewModel
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public int count { get; set; }

        public double tspAvg { get; set; }

        public double pm25 { get; set; }

        public double pm100 { get; set; }

        public int rate { get; set; }
    }

    public class DistrictAvgPostParams
    {
        public ProjectType? projectType { get; set; }

        public AverageType dataType { get; set; }
    }
}