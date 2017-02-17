using System.Collections.Generic;
using Dust.Platform.Web.Models.Home;

namespace Dust.Platform.Web.Models.Report
{
    public class ReportViewModel
    {
        public List<Nodes> MenuNodes { get; set; }
    }

    public class DayReportViewModel
    {
        
    }

    public class DeviceInstalled
    {
        public int Total { get; set; }

        public int Using { get; set; }

        public int Stoped { get; set; }
    }
}