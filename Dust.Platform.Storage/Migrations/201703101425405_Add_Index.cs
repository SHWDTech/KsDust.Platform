namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Index : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.KsDustAlarms", new[] { "DeviceId" });
            DropIndex("dbo.KsDustProjects", new[] { "DistrictId" });
            DropIndex("dbo.KsDustProjects", new[] { "EnterpriseId" });
            CreateIndex("dbo.AverageMonitorDatas", new[] { "ProjectType", "Category", "Type", "TargetId", "AverageDateTime" }, clustered: true, name: "IX_ProjectType_AverageCategory_AverageType_TargetId_UpdateTime");
            CreateIndex("dbo.DeviceMantanceRecords", new[] { "Device", "MantanceDateTime" }, clustered: true);
            CreateIndex("dbo.DeviceOnlineStatus", new[] { "DeviceGuid", "StatusType", "UpdateTime" }, clustered: true);
            CreateIndex("dbo.KsDustAlarms", new[] { "DeviceId", "AlarmDateTime" }, clustered: true, name: "IX_DeviceGuid_AlarmDateTime");
            CreateIndex("dbo.KsDustProjects", new[] { "ProjectType", "DistrictId", "EnterpriseId" }, clustered: true);
            CreateIndex("dbo.Reports", "ReportType", clustered: true);
            CreateIndex("dbo.SystemConfigurations", new[] { "ConfigType", "ConfigName" }, clustered: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.SystemConfigurations", new[] { "ConfigType", "ConfigName" });
            DropIndex("dbo.Reports", new[] { "ReportType" });
            DropIndex("dbo.KsDustProjects", new[] { "ProjectType", "DistrictId", "EnterpriseId" });
            DropIndex("dbo.KsDustAlarms", "IX_DeviceGuid_AlarmDateTime");
            DropIndex("dbo.DeviceOnlineStatus", new[] { "DeviceGuid", "StatusType", "UpdateTime" });
            DropIndex("dbo.DeviceMantanceRecords", new[] { "Device", "MantanceDateTime" });
            DropIndex("dbo.AverageMonitorDatas", "IX_ProjectType_AverageCategory_AverageType_TargetId_UpdateTime");
            CreateIndex("dbo.KsDustProjects", "EnterpriseId");
            CreateIndex("dbo.KsDustProjects", "DistrictId");
            CreateIndex("dbo.KsDustAlarms", "DeviceId");
        }
    }
}
