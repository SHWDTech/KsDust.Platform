namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAverageValues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AverageMonitorDatas", "Noise", c => c.Double(nullable: false));
            AddColumn("dbo.AverageMonitorDatas", "Temperature", c => c.Double(nullable: false));
            AddColumn("dbo.AverageMonitorDatas", "Humidity", c => c.Double(nullable: false));
            AddColumn("dbo.AverageMonitorDatas", "WindSpeed", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AverageMonitorDatas", "WindSpeed");
            DropColumn("dbo.AverageMonitorDatas", "Humidity");
            DropColumn("dbo.AverageMonitorDatas", "Temperature");
            DropColumn("dbo.AverageMonitorDatas", "Noise");
        }
    }
}
