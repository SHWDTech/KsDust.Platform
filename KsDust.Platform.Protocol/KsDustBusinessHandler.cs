using System;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding.Generics;

namespace KsDust.Platform.Protocol
{
    public class KsDustBusinessHandler : IBuinessHandler
    {
        public string BusinessName { get; } = Properties.Resource.BusinessName;

        public void RunHandler(IProtocolPackage package)
        {
            if (!(package is IProtocolPackage<string>)) return;
            var ksPackage = (IProtocolPackage<string>)package;
            if (ksPackage.Command.CommandCategory == "TimingAutoReport")
            {
                ParseMonitorData(ksPackage);
            }
        }

        private void ParseMonitorData(IProtocolPackage<string> package)
        {
            var ksDevice = ((KsDustClientSource)package.ClientSource).Device;
            if (!package.Command.DeliverParams.Contains("Store")) return;
            var monitorData = new KsDustMonitorData
            {
                ProjectType = ksDevice.Project.ProjectType,
                DeviceId = Guid.Parse(package.ClientSource.ClientIdentity),
                ProjectId = ksDevice.ProjectId,
                EnterpriseId = ksDevice.Project.EnterpriseId,
                DistrictId = ksDevice.Project.DistrictId,
                ParticulateMatter = double.Parse(package.GetDataValueString(Properties.Resource.TSP)),
                Pm25 = double.Parse(package.GetDataValueString(Properties.Resource.PM25)),
                Pm100 = double.Parse(package.GetDataValueString(Properties.Resource.PM100)),
                Noise = double.Parse(package.GetDataValueString(Properties.Resource.NOISE)),
                Templeture = double.Parse(package.GetDataValueString(Properties.Resource.TEMPERATURE)),
                Humidity = double.Parse(package.GetDataValueString(Properties.Resource.HUMIDITY)),
                WindSpeed = double.Parse(package.GetDataValueString(Properties.Resource.WINDSPEED)),
                WindDirection = (int)double.Parse(package.GetDataValueString(Properties.Resource.WINDDIRECTION)),
                MonitorType = ParseMonitorType(package["CmdByte"].ComponentValue),
                UpdateTime = DateTime.Now
            };
            var ctx = new KsDustDbContext();
            ctx.KsDustMonitorDatas.Add(monitorData);
            ctx.SaveChanges();
            ctx.Dispose();
        }

        private static MonitorType ParseMonitorType(string code)
        {
            switch (code)
            {
                case "2011":
                    return MonitorType.RealTime;
                case "2051":
                    return MonitorType.FifteenMin;
                case "2061":
                    return MonitorType.HourAvg;
                default:
                    return MonitorType.FifteenMin;
            }
        }
    }
}
