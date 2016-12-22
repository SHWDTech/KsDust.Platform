namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyColumnNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustProjects", "SuperIntend", c => c.String(maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.KsDustMonitorDatas", "Temperature", c => c.Double(nullable: false));
            DropColumn("dbo.KsDustMonitorDatas", "Templeture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KsDustMonitorDatas", "Templeture", c => c.Double(nullable: false));
            DropColumn("dbo.KsDustMonitorDatas", "Temperature");
            DropColumn("dbo.KsDustProjects", "SuperIntend");
        }
    }
}
