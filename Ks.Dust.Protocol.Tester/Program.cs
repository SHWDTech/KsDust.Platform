using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Report;
using Newtonsoft.Json;

namespace Ks.Dust.Protocol.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var report = new GeneralReportViewModel()
            {
                ReportTitle = "2017年1月",
                TotalInstalled = new DeviceInstalled { Total = 50, Using = 44, Stoped = 6},
                ConstructionSiteInstalled = new DeviceInstalled { Total = 35, Using = 33, Stoped = 2},
                MunicipalWorksInstalled = new DeviceInstalled { Total = 10, Using = 8, Stoped = 2},
                MixingPlantInstalled = new DeviceInstalled { Total = 5, Using = 3, Stoped = 2}
            };

            var db = new KsDustDbContext();
            var districts = db.Districts.ToList();
            var disInstalled = new List<DistrictDeviceInstalled>();
            var avgs = new List<DistrictAvg>();
            foreach (var district in districts)
            {
                var disIns = new DistrictDeviceInstalled
                {
                    DistrictName = district.Name,
                    ConstructionSiteInstalled = 4,
                    MunicipalWorksInstalled = 1,
                    MixingPlantInstalled = 0
                };
                disInstalled.Add(disIns);
                var avg = new DistrictAvg
                {
                    DistrictName = district.Name,
                    AveragePm = 0.567,
                    AveragePm25 = 0.433,
                    AveragePm100 = 0.486
                };
                avgs.Add(avg);
            }

            report.DistrictInstalleds = disInstalled;
            report.DistrictAvgs = avgs;
            var projects = db.KsDustProjects.Include("District").Include("Enterprise").Take(10).ToList();
            var top = new List<ProjectRank>();
            var tail = new List<ProjectRank>();
            foreach (var ksDustProject in projects)
            {
                var t = new ProjectRank
                {
                    DistrictName = ksDustProject.District.Name,
                    EnterpriseName = ksDustProject.Enterprise.Name,
                    Average = 0.543,
                    ProjectName = ksDustProject.Name,
                    Rank = "优"
                };
                var ta = new ProjectRank
                {
                    DistrictName = ksDustProject.District.Name,
                    EnterpriseName = ksDustProject.Enterprise.Name,
                    Average = 0.543,
                    ProjectName = ksDustProject.Name,
                    Rank = "优"
                };
                top.Add(t);
                tail.Add(ta);
            }
            report.ProjectTopRanks = top;
            report.ProjectTailRanks = tail;

            var json = JsonConvert.SerializeObject(report);
            using (var stream = new StreamWriter(File.OpenWrite(@"d:\mysql.json")))
            {
                stream.Write(json);
            }
        }
    }
}
