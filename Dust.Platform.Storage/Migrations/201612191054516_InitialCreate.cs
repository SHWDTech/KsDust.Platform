namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
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
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.KsDustProjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DistrictId = c.Guid(nullable: false),
                        EnterpriseId = c.Guid(nullable: false),
                        VendorId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 400, storeType: "nvarchar"),
                        Address = c.String(maxLength: 400, storeType: "nvarchar"),
                        ConstructionUnit = c.String(maxLength: 100, storeType: "nvarchar"),
                        Mobile = c.String(maxLength: 100, storeType: "nvarchar"),
                        OccupiedArea = c.Double(nullable: false),
                        Floorage = c.Double(nullable: false),
                        Installed = c.Boolean(nullable: false),
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
                        ConnectName = c.String(maxLength: 100, storeType: "nvarchar"),
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
                        Templeture = c.Double(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KsDustMonitorDatas", "ProjectId", "dbo.KsDustProjects");
            DropForeignKey("dbo.KsDustMonitorDatas", "EnterpriseId", "dbo.Enterprises");
            DropForeignKey("dbo.KsDustMonitorDatas", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.KsDustMonitorDatas", "DeviceId", "dbo.KsDustProjects");
            DropForeignKey("dbo.KsDustCameras", "DeviceId", "dbo.KsDustDevices");
            DropForeignKey("dbo.KsDustAlarms", "DeviceId", "dbo.KsDustDevices");
            DropForeignKey("dbo.KsDustDevices", "ProjectId", "dbo.KsDustProjects");
            DropForeignKey("dbo.KsDustProjects", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.KsDustProjects", "EnterpriseId", "dbo.Enterprises");
            DropForeignKey("dbo.KsDustProjects", "DistrictId", "dbo.Districts");
            DropIndex("dbo.KsDustMonitorDatas", "Ix_MonitorType_DistrictId_EnterpriseId_ProjectId_DeviceId_UpdateTime");
            DropIndex("dbo.KsDustCameras", new[] { "DeviceId" });
            DropIndex("dbo.KsDustProjects", new[] { "VendorId" });
            DropIndex("dbo.KsDustProjects", new[] { "EnterpriseId" });
            DropIndex("dbo.KsDustProjects", new[] { "DistrictId" });
            DropIndex("dbo.KsDustDevices", new[] { "ProjectId" });
            DropIndex("dbo.KsDustAlarms", new[] { "DeviceId" });
            DropTable("dbo.KsDustMonitorDatas");
            DropTable("dbo.KsDustCameras");
            DropTable("dbo.Vendors");
            DropTable("dbo.KsDustProjects");
            DropTable("dbo.KsDustDevices");
            DropTable("dbo.KsDustAlarms");
            DropTable("dbo.Enterprises");
            DropTable("dbo.Districts");
        }
    }
}
