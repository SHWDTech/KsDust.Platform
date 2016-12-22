namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSystemConfigTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemConfigurations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConfigType = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        ConfigName = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        ConfigValue = c.String(nullable: false, maxLength: 2000, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemConfigurations");
        }
    }
}
