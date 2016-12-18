using System;
using System.Collections.Generic;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding.Generics;

namespace KsDust.Platform.Protocol
{
    public class KsDustBusinessHandler : IBuinessHandler
    {
        /// <summary>
        /// 等待存储的监测数据
        /// </summary>
        private readonly List<KsDustMonitorData> _monitorDats = new List<KsDustMonitorData>();

        /// <summary>
        /// 最后一次执行存储操作的时间
        /// </summary>
        private DateTime _lastUpdateTime = DateTime.MinValue;

        /// <summary>
        /// 存储操作最长间隔
        /// </summary>
        private readonly TimeSpan _executeInteval = TimeSpan.FromMinutes(2);

        /// <summary>
        /// 标识是否正在执行存储操作
        /// </summary>
        private bool _isUpdating;

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
            var monitorData = new KsDustMonitorData
            {
                DeviceId = Guid.Parse(package.DeviceNodeId),
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
                WindDirection = int.Parse(package.GetDataValueString(Properties.Resource.WINDDIRECTION)),
                MonitorType = ParseMonitorType(package["CmdCode"].ComponentValue)
            };
            if (!package.Command.DeliverParams.Contains("Store")) return;
            lock (_monitorDats)
            {
                _monitorDats.Add(monitorData);
            }
            RunStorage();
        }

        /// <summary>
        /// 执行存储操作
        /// </summary>
        private void RunStorage()
        {
            if ((_monitorDats.Count > 1000 || DateTime.Now - _lastUpdateTime > _executeInteval) && !_isUpdating)
            {
                UpdateToDatabase();
            }
        }

        /// <summary>
        /// 更新至服务器
        /// </summary>
        private void UpdateToDatabase()
        {
            _isUpdating = true;
            lock (_monitorDats)
            {
                var ctx = new KsDustDbContext();
                ctx.KsDustMonitorDatas.AddRange(_monitorDats);
                ctx.SaveChanges();
                _lastUpdateTime = DateTime.Now;
                _monitorDats.Clear();
            }
            _isUpdating = false;
        }

        private MonitorType ParseMonitorType(string code)
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
