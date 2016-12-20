namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectAudited : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustProjects", "Audited", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KsDustProjects", "Audited");
        }
    }
}
