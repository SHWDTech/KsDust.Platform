namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableUserEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRelatedEntities",
                c => new
                    {
                        User = c.Guid(nullable: false),
                        Entity = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User, t.Entity });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRelatedEntities");
        }
    }
}
