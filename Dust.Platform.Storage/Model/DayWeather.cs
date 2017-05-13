using System;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class DayWeather : LongModel
    {
        public DateTime Date { get; set; }

        public string DayText { get; set; }

        public string DayCode { get; set; }

        public string NightText { get; set; }

        public string NightCode { get; set; }

        public double TemperatureHigh { get; set; }

        public double TemperatureLow { get; set; }

        public string WindDirection { get; set; }

        public double WindDirectionDegree { get; set; }

        public double WindSpeed { get; set; }

        public double WindScale { get; set; }
    }
}
