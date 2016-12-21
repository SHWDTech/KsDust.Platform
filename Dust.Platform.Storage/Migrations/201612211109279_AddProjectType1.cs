namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectType1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AverageMonitorDatas", "ProjectType", c => c.Byte(nullable: false));
            AddColumn("dbo.KsDustMonitorDatas", "ProjectType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KsDustMonitorDatas", "ProjectType");
            DropColumn("dbo.AverageMonitorDatas", "ProjectType");
        }
    }
}
