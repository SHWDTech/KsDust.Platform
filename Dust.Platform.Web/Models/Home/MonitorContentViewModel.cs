using System;
using System.Collections.Generic;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Web.Models.Home
{
    public class MonitorContentPost
    {
        public Guid TargetId { get; set; }

        public string TargetName { get; set; }

        public string MapContainer => $"{TargetName}mapContainer";

        public string Chart => $"{TargetName}chart";

        public string Selecter => $"{TargetName}Selecter";

        public string Title { get; set; }

        public List<AverageMonitorData> MonitorDatas { get; set; }

        public List<DistrictInfo> DistrictInfos { get; set; }

        public AverageCategory  ViewType { get; set; }
    }
}