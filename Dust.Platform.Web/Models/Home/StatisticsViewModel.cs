using System;
using System.Collections.Generic;
using Dust.Platform.Storage.Model;
using Dust.Platform.Web.Models.Ajax;

namespace Dust.Platform.Web.Models.Home
{
    public class StatisticsViewModel
    {
        public List<Nodes> MenuNodes { get; set; }
    }

    public class HistoryViewModel
    {
        public string Title { get; set; }

        public string Uuid { get; set; }

        public string FormId => $"historyRank_{Uuid}";

        public string ChartId => $"chart_{Uuid}";

        public string StartDateId => $"start_{Uuid}";

        public string EndDateId => $"end_{Uuid}";

        public string TableId => $"table_{Uuid}";

        public int Type { get; set; }
    }

    public class HistoryRankPost
    {
        public int Type { get; set; }

        public string Uuid { get; set; }
    }

    public class HistoryRankChartPost
    {
        public AverageCategory Type { get; set; }

        public MonitorDataValueType DataType { get; set; }

        public AverageType DateType { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}