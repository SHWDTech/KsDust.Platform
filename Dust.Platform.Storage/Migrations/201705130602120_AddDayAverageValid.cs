namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDayAverageValid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AverageMonitorDatas", "IsWeaterValid", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AverageMonitorDatas", "IsWeaterValid");
        }
    }
}
