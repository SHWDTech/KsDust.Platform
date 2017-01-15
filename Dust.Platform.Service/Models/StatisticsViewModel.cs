using System;

// ReSharper disable InconsistentNaming

namespace Dust.Platform.Service.Models
{
    public class StatisticsViewModel
    {
        public Guid id { get; set; }

        public string parentName { get; set; }

        public string name { get; set; }

        public double tsp { get; set; }

        public double pm25 { get; set; }

        public double pm100 { get; set; }

        public int rate { get; set; }
    }
}