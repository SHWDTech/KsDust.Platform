using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace Dust.Platform.Web.Models.Setting
{
    public class DayWeatherInfo
    {
        public List<InfoDetails> results { get; set; }
    }

    public class InfoDetails
    {
        public DayWeatherLocation location { get; set; }

        public List<DayWeatherDaily> daily { get; set; }

        public DateTime last_update { get; set; }
    }

    public class DayWeatherLocation
    {
        public string id { get; set; }

        public string name { get; set; }

        public string country { get; set; }

        public string path { get; set; }

        public string timezone { get; set; }

        public string timezone_offset { get; set; }
    }

    public class DayWeatherDaily
    {
        public DateTime date { get; set; }

        public string text_day { get; set; }

        public string code_day { get; set; }

        public string text_night { get; set; }

        public string code_night { get; set; }

        public double high { get; set; }

        public double low { get; set; }

        public string precip { get; set; }

        public string wind_direction { get; set; }

        public double wind_direction_degree { get; set; }

        public double wind_speed { get; set; }

        public double wind_scale { get; set; }
    }
}