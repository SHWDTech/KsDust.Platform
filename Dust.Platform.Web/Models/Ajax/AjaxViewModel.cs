using System;
using Dust.Platform.Storage.Model;
// ReSharper disable InconsistentNaming

namespace Dust.Platform.Web.Models.Ajax
{
    public class MonitorDataPost
    {
        public Guid target { get; set; }

        public AverageCategory tType { get; set; }

        public AverageType dCate { get; set; }

        public MonitorDataValueType dType { get; set; }

        public int count { get; set; }
    }

    public class MonitorChartData
    {
        public double value { get; set; }

        public DateTime date { get; set; }
    }

    public enum MonitorDataValueType : byte
    {
        Pm = 0x00,

        Pm25 = 0x01,

        Pm100 = 0x02,

        Noise = 0x03,

        Temperature = 0x04,

        Humidity = 0x05,

        WindSPeed = 0x06
    }

    public class DevicePostData
    {
        public AverageCategory viewType { get; set; }

        public Guid targetId { get; set; }
    }

    public class DeviceCurrentStatus
    {
        public string name { get; set; }

        public double pm { get; set; }

        public double noise { get; set; }

        public DeviceStatus status { get; set; }

        public string lat { get; set; }

        public string lon { get; set; }

        public string time { get; set; }
    }

    public enum DeviceStatus : byte
    {
        OffLine = 0x00,

        Good = 0x01,

        Alarm = 0x02,

        Bad = 0x03
    }
}