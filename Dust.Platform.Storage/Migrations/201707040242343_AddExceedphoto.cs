namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExceedphoto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExceedPhotoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AlarmGuid = c.Guid(nullable: false),
                        PhotoPath = c.String(unicode: false),
                        Comment = c.String(unicode: false),
                        UploadDateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExceedPhotoes");
        }
    }
}
