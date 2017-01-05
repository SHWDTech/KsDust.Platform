// ReSharper disable InconsistentNaming

using System;
using Dust.Platform.Storage.Model;
using Dust.Platform.Web.Models.Ajax;

namespace Dust.Platform.Web.Models.Table
{
    public class TablePost
    {

        public int offset { get; set; }

        public int limit { get; set; }

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
}