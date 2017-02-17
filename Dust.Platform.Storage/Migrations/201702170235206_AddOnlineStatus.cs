namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOnlineStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceOnlineStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DeviceGuid = c.Guid(nullable: false),
                        IsOnline = c.Boolean(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        StatusType = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OnlineStatistics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TargetGuid = c.Guid(nullable: false),
                        Category = c.Byte(nullable: false),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        StatusType = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OnlineStatistics");
            DropTable("dbo.DeviceOnlineStatus");
        }
    }
}
