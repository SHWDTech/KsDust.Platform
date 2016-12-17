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
        private readonly List<KsDustMonitorData> _monitorDats = new List<KsDustMonitorData>();

        private DateTime _lastUpdateTime = DateTime.MinValue;

        private readonly TimeSpan _executeInteval = TimeSpan.FromMinutes(2);

        private bool _isUpdating;

        public string BusinessName { get; } = Properties.Resource.BusinessName;

        public void RunHandler(IProtocolPackage package)
        {
            if (!(package is IProtocolPackage<string>)) return;
            var ksPackage = (IProtocolPackage<string>)package;
            var ksDevice = ((KsDustClientSource)ksPackage.ClientSource).Device;
            var monitorData = new KsDustMonitorData
            {
                DeviceId = Guid.Parse(ksPackage.DeviceNodeId),
                ProjectId = ksDevice.ProjectId,
                EnterpriseId = ksDevice.Project.EnterpriseId,
                DistrictId = ksDevice.Project.DistrictId,
                ParticulateMatter = double.Parse(ksPackage.GetDataValueString(Properties.Resource.TSP)),
                Pm25 = double.Parse(ksPackage.GetDataValueString(Properties.Resource.PM25)),
                Pm100 = double.Parse(ksPackage.GetDataValueString(Properties.Resource.PM100)),
                Noise = double.Parse(ksPackage.GetDataValueString(Properties.Resource.NOISE)),
                Templeture = double.Parse(ksPackage.GetDataValueString(Properties.Resource.TEMPERATURE)),
                Humidity = double.Parse(ksPackage.GetDataValueString(Properties.Resource.HUMIDITY)),
                WindSpeed = double.Parse(ksPackage.GetDataValueString(Properties.Resource.WINDSPEED)),
                WindDirection = int.Parse(ksPackage.GetDataValueString(Properties.Resource.WINDDIRECTION))
            };
            lock (_monitorDats)
            {
                _monitorDats.Add(monitorData);
            }
            RunStorage();
        }

        private void RunStorage()
        {
            if ((_monitorDats.Count > 1000 || DateTime.Now - _lastUpdateTime > _executeInteval) && !_isUpdating)
            {
                UpdateToDatabase();
            }
        }

        private void UpdateToDatabase()
        {
            _isUpdating = true;
            lock (_monitorDats)
            {
                var ctx = new KsDustDbContext();
                ctx.KsDustMonitorDatas.AddRange(_monitorDats);
                ctx.SaveChanges();
                _lastUpdateTime = DateTime.Now;
            }
            _isUpdating = false;
        }
    }
}
