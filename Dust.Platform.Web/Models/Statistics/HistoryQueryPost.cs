using System;
using Dust.Platform.Storage.Model;
using Dust.Platform.Web.Models.Ajax;

namespace Dust.Platform.Web.Models.Statistics
{
    public class HistoryQueryPost
    {
        public Guid TargetId { get; set; }

        public string TargetName { get; set; }

        public AverageCategory ViewType { get; set; }

        public string FormId => $"historyQuery_{TargetId}";

        public string StartDateId => $"hqstart_{TargetId}";

        public string EndDateId => $"hqend_{TargetId}";

        public string ChartId => $"hqchart_{TargetId}";

        public string TableId => $"hqtable_{TargetId}";
    }

    public class HistoryQueryChartPost
    {
        public Guid Id { get; set; }

        public AverageCategory Type { get; set; }

        public AverageType DateType { get; set; }

        public MonitorDataValueType DataType { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}