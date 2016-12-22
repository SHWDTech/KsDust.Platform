// ReSharper disable InconsistentNaming

using System;

namespace Dust.Platform.Service.Models
{
    public class HistoryDataViewModel
    {
        public string date { get; set; }

        public double tsp { get; set; }

        public double pm25 { get; set; }

        public double pm100 { get; set; }

        public int rate { get; set; }
    }

    public class HistoryDataPostParams
    {
        public Guid device { get; set; }

        public int dataType { get; set; }
    }
}