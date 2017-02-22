using System.Collections.Generic;
using Dust.Platform.Web.Models.Home;
// ReSharper disable InconsistentNaming

namespace Dust.Platform.Web.Models.Report
{
    public class ReportViewModel
    {
        public List<Nodes> MenuNodes { get; set; }
    }

    public class GeneralReportViewModel
    {
        public string ReportTitle { get; set; }

        public DeviceInstalled TotalInstalled { get; set; }

        public DeviceInstalled ConstructionSiteInstalled { get; set; }

        public DeviceInstalled MunicipalWorksInstalled { get; set; }

        public DeviceInstalled MixingPlantInstalled { get; set; }

        public List<DistrictDeviceInstalled> DistrictInstalleds { get; set; }

        public List<DistrictAvg> DistrictAvgs { get; set; }

        public List<ProjectRank> ProjectTopRanks { get; set; }

        public List<ProjectRank> ProjectTailRanks { get; set; }

        public ReportBarChartOption BarChartOption { get; set; }
    }

    public class DeviceInstalled
    {
        public int Total { get; set; }

        public int Using { get; set; }

        public int Stoped { get; set; }
    }

    public class DistrictDeviceInstalled : DeviceInstalled
    {
        /// <summary>
        /// 区县名称
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// 建筑工地在用
        /// </summary>
        public int ConstructionSiteInstalled { get; set; }

        /// <summary>
        /// 拌合站在用
        /// </summary>
        public int MunicipalWorksInstalled { get; set; }

        /// <summary>
        /// 市政工地在用
        /// </summary>
        public int MixingPlantInstalled { get; set; }
    }

    public class DistrictAvg
    {
        public string DistrictName { get; set; }

        public double AveragePm { get; set; }

        public double AveragePm25 { get; set; }

        public double AveragePm100 { get; set; }
    }

    public class ProjectRank
    {
        public string ProjectName { get; set; }

        public string EnterpriseName { get; set; }

        public string DistrictName { get; set; }

        public string Rank { get; set; }

        public double Average { get; set; }
    }

    public class ReportBarChartOption
    {
        public string title { get; set; }

        public List<string> xAxis { get; set; } = new List<string>();

        public string yAxisName { get; set; }

        public List<BarOptionSeries> series { get; set; } = new List<BarOptionSeries>();
    }

    public class BarOptionSeries
    {
        public string name { get; set; }

        public string type { get; set; }

        public List<double> data { get; set; } = new List<double>();
    }
}