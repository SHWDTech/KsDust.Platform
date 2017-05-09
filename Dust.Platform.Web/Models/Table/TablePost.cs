// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using Dust.Platform.Storage.Model;
using Dust.Platform.Web.Models.Ajax;
using Dust.Platform.Web.Models.Setting;

namespace Dust.Platform.Web.Models.Table
{
    public class TablePost
    {

        public int offset { get; set; }

        public int limit { get; set; }

        public string sort { get; set; }

        public string order { get; set; }

        public string act { get; set; }

        public string title { get; set; }
    }

    public class TotalProjectsTablePost : TablePost
    {
        public Guid? district { get; set; }

        public Guid? enterprise { get; set; }
    }

    public class HistoryTablePost : TablePost
    {
        public AverageCategory type { get; set; }

        public MonitorDataValueType dataType { get; set; }

        public AverageType DateType { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }
    }

    public class HistoryQueryTablePost : TablePost
    {
        public AverageCategory type { get; set; }

        public Guid id { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }
    }

    public class HistoryStatsTablePost : TablePost
    {
        public AverageCategory type { get; set; }

        public AverageType dataType { get; set; }

        public Guid id { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }
    }

    public class DevMantanceTablePost : TablePost
    {
        public MantanceStatus? MantanceStatus { get; set; }
    }

    public class DeviceMantanceRecordTablePost : TablePost
    {
        public Guid DeviceGuid { get; set; }
    }

    public class DevicePreviewTablePost : TablePost
    {
        public Guid? VendorGuid { get; set; }
    }

    public class AvgReportTablePost : TablePost
    {
        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public int percent { get; set; }
    }

    public class DeviceHistoryDataTablePost : TablePost
    {
        public Guid devideGuid { get; set; }
    }

    public class MessageTablePost : TablePost
    {
        public NoticeType? Type { get; set; }

        public bool? Status { get; set; }
    }

    public class OnlineStatisticsRankTablePost : TablePost
    {
        public AverageType StatusType { get; set; }

        public AverageCategory Category { get; set; }

        public long DateTimeTicks { get; set; }

        public DateTime UpdateTime => new DateTime(DateTimeTicks);
    }
}