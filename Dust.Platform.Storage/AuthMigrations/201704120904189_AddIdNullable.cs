namespace Dust.Platform.Storage.AuthMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdNullable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "Controller", c => c.String(unicode: false));
            AddColumn("dbo.Modules", "Action", c => c.String(unicode: false));
            AlterColumn("dbo.DustPermissions", "ParentPermissionId", c => c.Guid());
            AlterColumn("dbo.Modules", "ParentModuleId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Modules", "ParentModuleId", c => c.Guid(nullable: false));
            AlterColumn("dbo.DustPermissions", "ParentPermissionId", c => c.Guid(nullable: false));
            DropColumn("dbo.Modules", "Action");
            DropColumn("dbo.Modules", "Controller");
        }
    }
}
