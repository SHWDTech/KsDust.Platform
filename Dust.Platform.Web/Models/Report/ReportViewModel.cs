using System.Collections.Generic;
using Dust.Platform.Storage.Model;
using Dust.Platform.Web.Models.Home;
// ReSharper disable InconsistentNaming

namespace Dust.Platform.Web.Models.Report
{
    public class ReportViewModel
    {
        public List<Nodes> MenuNodes { get; set; }
    }

    public class OnlineStatisticsDatesParam
    {
        public AverageCategory ObjectType { get; set; }

        public AverageType DateType { get; set; }
    }

    public class OnlineStatisticsReportItem
    {
        public string TargetName { get; set; }

        public string OnlineRank { get; set; }

        public string DateTime { get; set; }
    }

    public class OnlineStatisticsReport
    {
        public string ReportTitle { get; set; }

        public string FileName { get; set; }

        public List<OnlineStatisticsReportItem> Items { get; set; }
    }

    public class AvgRankReportItem
    {
        public int Rank { get; set; }

        public string ProjectName { get; set; }

        public string EnterpriseName { get; set; }

        public double AvgPm { get; set; }
    }

    public class AvgRankReport
    {
        public string ReportTitle { get; set; }

        public string FileName { get; set; }

        public List<AvgRankReportItem> Items { get; set; }
    }
}