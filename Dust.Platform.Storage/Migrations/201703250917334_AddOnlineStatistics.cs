namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOnlineStatistics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OnlineStatistics", "TargetGuid", c => c.Guid(nullable: false));
            AddColumn("dbo.OnlineStatistics", "Category", c => c.Byte(nullable: false));
            AddColumn("dbo.OnlineStatistics", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.OnlineStatistics", "StatusType", c => c.Byte(nullable: false));
            AddColumn("dbo.OnlineStatistics", "Statistics", c => c.Double(nullable: false));
            CreateIndex("dbo.OnlineStatistics", new[] { "Category", "TargetGuid", "StatusType", "UpdateTime" }, unique: true, clustered: true, name: "Ix_ProjectType_Target_AverageType_DateTime");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OnlineStatistics", "Ix_ProjectType_Target_AverageType_DateTime");
            DropColumn("dbo.OnlineStatistics", "Statistics");
            DropColumn("dbo.OnlineStatistics", "StatusType");
            DropColumn("dbo.OnlineStatistics", "UpdateTime");
            DropColumn("dbo.OnlineStatistics", "Category");
            DropColumn("dbo.OnlineStatistics", "TargetGuid");
        }
    }
}
