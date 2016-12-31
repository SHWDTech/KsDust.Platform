using System;
using System.Collections.Generic;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Web.Models.Home
{
    public class MonitorContentPost
    {
        public Guid TargetId { get; set; }

        public string TargetName { get; set; }

        public string MapContainer => $"{Prefix}{TargetId}mapContainer";

        public string Chart => $"{Prefix}{TargetId}chart";

        public string Selecter => $"{Prefix}{TargetId}Selecter";

        public string Prefix { get; set; }

        public string Title { get; set; }

        public List<AverageMonitorData> MonitorDatas { get; set; }

        public List<DistrictInfo> DistrictInfos { get; set; }

        public List<DistrictStatus> DistrictStatuses { get; set; }

        public KsDustProject Project { get; set; }

        public KsDustDevice Device { get; set; }

        public AverageCategory  ViewType { get; set; }
    }

    public class DistrictStatus
    {
        public string ProjectName { get; set; }

        public int DevicesCount { get; set; }

        public double TotalOccupiedArea { get; set; }

        public double TotalFloorage { get; set; }
    }
}