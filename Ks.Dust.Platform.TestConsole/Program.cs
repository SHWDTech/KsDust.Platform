using System;
using System.Collections.Generic;
using Dust.Platform.Storage.Model;
using Dust.Platform.Storage.Repository;

namespace Ks.Dust.Platform.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null) return;
            TestData();
        }

        static void TestData()
        {
            var ctx = new KsDustDbContext();
            var disa = new District
            {
                Id = Guid.Empty,
                Name = "未审核"
            };
            var disb = new District
            {
                Id = Guid.NewGuid(),
                Name = "淀山湖镇"
            };
            var disc = new District
            {
                Id = Guid.NewGuid(),
                Name = "玉山镇"
            };
            var disd = new District
            {
                Id = Guid.NewGuid(),
                Name = "巴城镇"
            };
            var dise = new District
            {
                Id = Guid.NewGuid(),
                Name = "花桥镇"
            };
            var disf = new District
            {
                Id = Guid.NewGuid(),
                Name = "锦溪镇"
            };
            var disg = new District
            {
                Id = Guid.NewGuid(),
                Name = "周市镇"
            };
            var dish = new District
            {
                Id = Guid.NewGuid(),
                Name = "千灯镇"
            };
            var disi = new District
            {
                Id = Guid.NewGuid(),
                Name = "周庄镇"
            };
            var disj = new District
            {
                Id = Guid.NewGuid(),
                Name = "陆家镇"
            };
            var disk = new District
            {
                Id = Guid.NewGuid(),
                Name = "张浦镇"
            };
            ctx.Districts.AddRange(new List<District>
            {
                disa,disb,disc,disd,dise,disf,disg,dish,disi,disj,disk
            });

            var enta = new Enterprise
            {
                Id = Guid.NewGuid(),
                Mobile = "13500000000",
                Name = "昆山市第一施工单位"
            };
            var entb = new Enterprise
            {
                Id = Guid.NewGuid(),
                Mobile = "13600000000",
                Name = "昆山市第二施工单位"
            };
            var entc = new Enterprise
            {
                Id = Guid.NewGuid(),
                Mobile = "13700000000",
                Name = "昆山市第三施工单位"
            };
            var entd = new Enterprise
            {
                Id = Guid.NewGuid(),
                Mobile = "13800000000",
                Name = "昆山市第四施工单位"
            };
            ctx.Enterprises.AddRange(new List<Enterprise>
            {
                enta,entb,entc,entd
            });

            var vena = new Vendor
            {
                Id = Guid.NewGuid(),
                Mobile = "13510000000",
                Name = "昆山市A供应商",
                Susperintend = "赵经理"
            };
            var venb = new Vendor
            {
                Id = Guid.NewGuid(),
                Mobile = "13520000000",
                Name = "昆山市A供应商",
                Susperintend = "钱经理"
            };
            var venc = new Vendor
            {
                Id = Guid.NewGuid(),
                Mobile = "13530000000",
                Name = "昆山市A供应商",
                Susperintend = "孙经理"
            };
            var vend = new Vendor
            {
                Id = Guid.NewGuid(),
                Mobile = "13540000000",
                Name = "昆山市A供应商",
                Susperintend = "李经理"
            };
            ctx.Vendors.AddRange(new List<Vendor>
            {
                vena,venb,venc,vend
            });

            var prja = new KsDustProject
            {
                Id = Guid.NewGuid(),
                Address = "昆山市第一街道",
                ConstructionUnit = "昆山市第一建设集团",
                District = disb,
                Enterprise = enta,
                Vendor = vena,
                Floorage = 40000.00,
                OccupiedArea = 40000.00
            };
            var prjb = new KsDustProject
            {
                Id = Guid.NewGuid(),
                Address = "昆山市第二街道",
                ConstructionUnit = "昆山市第二建设集团",
                District = disc,
                Enterprise = entb,
                Vendor = venb,
                Floorage = 40000.00,
                OccupiedArea = 40000.00
            };
            var prjc = new KsDustProject
            {
                Id = Guid.NewGuid(),
                Address = "昆山市第三街道",
                ConstructionUnit = "昆山市第三建设集团",
                District = disd,
                Enterprise = entc,
                Vendor = venc,
                Floorage = 40000.00,
                OccupiedArea = 40000.00
            };
            var prjd = new KsDustProject
            {
                Id = Guid.NewGuid(),
                Address = "昆山市第四街道",
                ConstructionUnit = "昆山市第四建设集团",
                District = dise,
                Enterprise = entd,
                Vendor = vend,
                Floorage = 40000.00,
                OccupiedArea = 40000.00
            };
            ctx.KsDustProjects.AddRange(new List<KsDustProject>
            {
                prja,prjb,prjc,prjd
            });

            var deva = new KsDustDevice
            {
                Id = Guid.NewGuid(),
                NodeId = "KSHBZBWCOM0001",
                Project = prja,
                Name = "昆山第一套设备",
                Longitude = "121.3262547",
                Latitude = "49.2651245",
                IsOnline = true,
                InstallDateTime = DateTime.Parse("2016-12-15"),
                StartDateTime = DateTime.Parse("2016-12-15"),
                LastMaintenance = DateTime.Parse("2016-12-15")
            };
            var devb = new KsDustDevice
            {
                Id = Guid.NewGuid(),
                NodeId = "KSHBZBWCOM0001",
                Project = prjb,
                Name = "昆山第二套设备",
                Longitude = "121.6521436254172651245",
                IsOnline = true,
                InstallDateTime = DateTime.Parse("2016-12-15"),
                StartDateTime = DateTime.Parse("2016-12-15"),
                LastMaintenance = DateTime.Parse("2016-12-15")
            };
            var devc = new KsDustDevice
            {
                Id = Guid.NewGuid(),
                NodeId = "KSHBZBWCOM0003",
                Project = prjc,
                Name = "昆山第三套设备",
                Longitude = "121.7662547",
                Latitude = "49.8651245",
                IsOnline = true,
                InstallDateTime = DateTime.Parse("2016-12-15"),
                StartDateTime = DateTime.Parse("2016-12-15"),
                LastMaintenance = DateTime.Parse("2016-12-15")
            };
            var devd = new KsDustDevice
            {
                Id = Guid.NewGuid(),
                NodeId = "KSHBZBWCOM0004",
                Project = prja,
                Name = "昆山第四套设备",
                Longitude = "121.6251458",
                Latitude = "49.2514856",
                IsOnline = true,
                InstallDateTime = DateTime.Parse("2016-12-15"),
                StartDateTime = DateTime.Parse("2016-12-15"),
                LastMaintenance = DateTime.Parse("2016-12-15")
            };

            ctx.KsDustDevices.AddRange(new List<KsDustDevice>
            {
                deva,
                devb,
                devc,
                devd
            });
            ctx.SaveChanges();
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
