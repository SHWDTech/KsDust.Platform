namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvgMonitorData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AverageMonitorDatas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TargetId = c.Guid(nullable: false),
                        Category = c.Byte(nullable: false),
                        Type = c.Byte(nullable: false),
                        ParticulateMatter = c.Double(nullable: false),
                        Pm25 = c.Double(nullable: false),
                        Pm100 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AverageMonitorDatas");
        }
    }
}
