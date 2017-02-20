namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AverageMonitorDatas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProjectType = c.Byte(nullable: false),
                        TargetId = c.Guid(nullable: false),
                        Category = c.Byte(nullable: false),
                        Type = c.Byte(nullable: false),
                        ParticulateMatter = c.Double(nullable: false),
                        Pm25 = c.Double(nullable: false),
                        Pm100 = c.Double(nullable: false),
                        Noise = c.Double(nullable: false),
                        Temperature = c.Double(nullable: false),
                        Humidity = c.Double(nullable: false),
                        WindSpeed = c.Double(nullable: false),
                        AverageDateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeviceOnlineStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DeviceGuid = c.Guid(nullable: false),
                        IsOnline = c.Boolean(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        StatusType = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Enterprises",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                        Mobile = c.String(maxLength: 100, storeType: "nvarchar"),
                        OuterId = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KsDustAlarms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeviceId = c.Guid(nullable: false),
                        AlarmValue = c.Double(nullable: false),
                        AlarmDateTime = c.DateTime(nullable: false, precision: 0),
                        IsReaded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KsDustDevices", t => t.DeviceId, cascadeDelete: true)
                .Index(t => t.DeviceId);
            
            CreateTable(
                "dbo.KsDustDevices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NodeId = c.String(maxLength: 100, storeType: "nvarchar"),
                        ProjectId = c.Guid(nullable: false),
                        VendorId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                        Longitude = c.String(unicode: false),
                        Latitude = c.String(unicode: false),
                        IsOnline = c.Boolean(nullable: false),
                        InstallDateTime = c.DateTime(nullable: false, precision: 0),
                        StartDateTime = c.DateTime(nullable: false, precision: 0),
                        LastMaintenance = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KsDustProjects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.KsDustProjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProjectType = c.Byte(nullable: false),
                        DistrictId = c.Guid(nullable: false),
                        EnterpriseId = c.Guid(nullable: false),
                        VendorId = c.Guid(nullable: false),
                        CityArea = c.Byte(nullable: false),
                        Name = c.String(maxLength: 400, storeType: "nvarchar"),
                        ContractRecord = c.String(nullable: false, unicode: false),
                        Address = c.String(maxLength: 400, storeType: "nvarchar"),
                        ConstructionUnit = c.String(maxLength: 100, storeType: "nvarchar"),
                        SuperIntend = c.String(maxLength: 100, storeType: "nvarchar"),
                        Mobile = c.String(maxLength: 100, storeType: "nvarchar"),
                        OccupiedArea = c.Double(nullable: false),
                        Floorage = c.Double(nullable: false),
                        Installed = c.Boolean(nullable: false),
                        Audited = c.Boolean(nullable: false),
                        Stopped = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .ForeignKey("dbo.Enterprises", t => t.EnterpriseId, cascadeDelete: true)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.DistrictId)
                .Index(t => t.EnterpriseId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                        Susperintend = c.String(maxLength: 100, storeType: "nvarchar"),
                        Mobile = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KsDustCameras",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeviceId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                        SerialNumber = c.String(maxLength: 100, storeType: "nvarchar"),
                        UserName = c.String(maxLength: 100, storeType: "nvarchar"),
                        Password = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KsDustDevices", t => t.DeviceId, cascadeDelete: true)
                .Index(t => t.DeviceId);
            
            CreateTable(
                "dbo.KsDustMonitorDatas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProjectType = c.Byte(nullable: false),
                        MonitorType = c.Byte(nullable: false),
                        DistrictId = c.Guid(nullable: false),
                        EnterpriseId = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        DeviceId = c.Guid(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        ParticulateMatter = c.Double(nullable: false),
                        Pm25 = c.Double(nullable: false),
                        Pm100 = c.Double(nullable: false),
                        Noise = c.Double(nullable: false),
                        Temperature = c.Double(nullable: false),
                        Humidity = c.Double(nullable: false),
                        WindSpeed = c.Double(nullable: false),
                        WindDirection = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KsDustProjects", t => t.DeviceId, cascadeDelete: true)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .ForeignKey("dbo.Enterprises", t => t.EnterpriseId, cascadeDelete: true)
                .ForeignKey("dbo.KsDustProjects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => new { t.MonitorType, t.DistrictId, t.EnterpriseId, t.ProjectId, t.DeviceId, t.UpdateTime }, name: "Ix_MonitorType_DistrictId_EnterpriseId_ProjectId_DeviceId_UpdateTime");
            
            CreateTable(
                "dbo.OnlineStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ReportDataJson = c.String(unicode: false, storeType: "text"),
                        ReportType = c.Byte(nullable: false),
                        ReportDate = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SystemConfigurations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConfigType = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        ConfigName = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        ConfigValue = c.String(nullable: false, maxLength: 8000, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRelatedEntities",
                c => new
                    {
                        User = c.Guid(nullable: false),
                        Entity = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User, t.Entity });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KsDustMonitorDatas", "ProjectId", "dbo.KsDustProjects");
            DropForeignKey("dbo.KsDustMonitorDatas", "EnterpriseId", "dbo.Enterprises");
            DropForeignKey("dbo.KsDustMonitorDatas", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.KsDustMonitorDatas", "DeviceId", "dbo.KsDustProjects");
            DropForeignKey("dbo.KsDustCameras", "DeviceId", "dbo.KsDustDevices");
            DropForeignKey("dbo.KsDustAlarms", "DeviceId", "dbo.KsDustDevices");
            DropForeignKey("dbo.KsDustDevices", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.KsDustDevices", "ProjectId", "dbo.KsDustProjects");
            DropForeignKey("dbo.KsDustProjects", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.KsDustProjects", "EnterpriseId", "dbo.Enterprises");
            DropForeignKey("dbo.KsDustProjects", "DistrictId", "dbo.Districts");
            DropIndex("dbo.KsDustMonitorDatas", "Ix_MonitorType_DistrictId_EnterpriseId_ProjectId_DeviceId_UpdateTime");
            DropIndex("dbo.KsDustCameras", new[] { "DeviceId" });
            DropIndex("dbo.KsDustProjects", new[] { "VendorId" });
            DropIndex("dbo.KsDustProjects", new[] { "EnterpriseId" });
            DropIndex("dbo.KsDustProjects", new[] { "DistrictId" });
            DropIndex("dbo.KsDustDevices", new[] { "VendorId" });
            DropIndex("dbo.KsDustDevices", new[] { "ProjectId" });
            DropIndex("dbo.KsDustAlarms", new[] { "DeviceId" });
            DropTable("dbo.UserRelatedEntities");
            DropTable("dbo.SystemConfigurations");
            DropTable("dbo.Reports");
            DropTable("dbo.OnlineStatistics");
            DropTable("dbo.KsDustMonitorDatas");
            DropTable("dbo.KsDustCameras");
            DropTable("dbo.Vendors");
            DropTable("dbo.KsDustProjects");
            DropTable("dbo.KsDustDevices");
            DropTable("dbo.KsDustAlarms");
            DropTable("dbo.Enterprises");
            DropTable("dbo.Districts");
            DropTable("dbo.DeviceOnlineStatus");
            DropTable("dbo.AverageMonitorDatas");
        }
    }
}
