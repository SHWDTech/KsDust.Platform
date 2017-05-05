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
}