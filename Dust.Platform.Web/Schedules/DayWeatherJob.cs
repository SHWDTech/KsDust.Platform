using System;
using System.IO;
using System.Net;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Setting;
using Newtonsoft.Json;
using Quartz;
using SHWDTech.Platform.Utility;

namespace Dust.Platform.Web.Schedules
{
    public class DayWeatherJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var request = WebRequest.Create("https://api.seniverse.com/v3/weather/daily.json?key=rpvxjeffnzncbdks&location=shanghai&language=zh-Hans&unit=c&start=1&days=1");
                request.Method = "GET";
                var response = request.GetResponse();
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    var jsonStr = stream.ReadToEnd();
                    var info = JsonConvert.DeserializeObject<DayWeatherInfo>(jsonStr);
                    var dayweather = new DayWeather
                    {
                        Date = DateTime.Now,
                        DayText = info.results[0].daily[0].text_day,
                        DayCode = info.results[0].daily[0].code_day,
                        NightText = info.results[0].daily[0].text_night,
                        NightCode = info.results[0].daily[0].code_night,
                        TemperatureHigh = info.results[0].daily[0].high,
                        TemperatureLow = info.results[0].daily[0].low,
                        WindDirection = info.results[0].daily[0].wind_direction,
                        WindDirectionDegree = info.results[0].daily[0].wind_direction_degree,
                        WindSpeed = info.results[0].daily[0].wind_speed,
                        WindScale = info.results[0].daily[0].wind_scale
                    };
                    var ctx = new KsDustDbContext();
                    ctx.DayWeathers.Add(dayweather);
                    ctx.SaveChanges();
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("Update Weather Failed", ex);
            }
        }
    }
}