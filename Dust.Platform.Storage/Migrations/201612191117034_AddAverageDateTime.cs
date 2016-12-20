namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAverageDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AverageMonitorDatas", "AverageDateTime", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AverageMonitorDatas", "AverageDateTime");
        }
    }
}
