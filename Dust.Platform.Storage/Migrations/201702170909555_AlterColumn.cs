namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OnlineStatistics", "TargetGuid");
            DropColumn("dbo.OnlineStatistics", "Category");
            DropColumn("dbo.OnlineStatistics", "UpdateTime");
            DropColumn("dbo.OnlineStatistics", "StatusType");
            DropColumn("dbo.OnlineStatistics", "Statistics");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OnlineStatistics", "Statistics", c => c.Double(nullable: false));
            AddColumn("dbo.OnlineStatistics", "StatusType", c => c.Byte(nullable: false));
            AddColumn("dbo.OnlineStatistics", "UpdateTime", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.OnlineStatistics", "Category", c => c.Byte(nullable: false));
            AddColumn("dbo.OnlineStatistics", "TargetGuid", c => c.Guid(nullable: false));
        }
    }
}
