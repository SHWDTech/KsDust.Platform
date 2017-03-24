namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Notice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NoticeType = c.Byte(nullable: false),
                        NoticeDateTime = c.DateTime(nullable: false, precision: 0),
                        Title = c.String(maxLength: 200, storeType: "nvarchar"),
                        Content = c.String(maxLength: 8000, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserClientNotices",
                c => new
                    {
                        User = c.Guid(nullable: false),
                        NoticeClientType = c.Byte(nullable: false),
                        Notice = c.Long(nullable: false),
                        IsReaded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.User, t.NoticeClientType, t.Notice });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserClientNotices");
            DropTable("dbo.Notices");
        }
    }
}
