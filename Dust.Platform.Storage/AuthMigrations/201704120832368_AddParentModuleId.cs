namespace Dust.Platform.Storage.AuthMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParentModuleId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "ParentModuleId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "ParentModuleId");
        }
    }
}
