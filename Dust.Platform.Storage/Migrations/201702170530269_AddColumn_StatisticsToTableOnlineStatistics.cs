namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_StatisticsToTableOnlineStatistics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OnlineStatistics", "Statistics", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OnlineStatistics", "Statistics");
        }
    }
}
