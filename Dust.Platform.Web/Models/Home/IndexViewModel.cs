using System.Collections.Generic;

namespace Dust.Platform.Web.Models.Home
{
    public class IndexViewModel
    {
        public List<DistrictInfo> DistrictInfos { get; set; } = new List<DistrictInfo>();

        public List<Enterprises> Enterpriseses { get; set; } = new List<Enterprises>();

        public List<VendorInfo> VendorInfos { get; set; } = new List<VendorInfo>();

        public List<DistrictRank> DistrictRanks { get; set; } = new List<DistrictRank>();

        public List<ProjectRank> ProjectRanks { get; set; } = new List<ProjectRank>();
    }

    public class DistrictInfo
    {
        public string DistrictName { get; set; }

        public int ProjectsCount { get; set; }

        public int ProjectsInstalled { get; set; }

        public string InstallPercentage { get; set; }
    }

    public class Enterprises
    {
        public string EnterpriseName { get; set; }

        public int DevicesCount { get; set; }

        public int OnlineCount { get; set; }

        public int OfflineCount { get; set; }

        public string OnlinePercentage { get; set; }
    }

    public class VendorInfo
    {
        public string VendroName { get; set; }

        public string Susperintend { get; set; }

        public string Mobile { get; set; }
    }

    public class DistrictRank
    {
        public string DistrictName { get; set; }

        public double CurrentTsp { get; set; }
    }

    public class ProjectRank
    {
        public string ProjectName { get; set; }

        public double CurrentTsp { get; set; }
    }
}