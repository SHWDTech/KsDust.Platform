namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustProjects", "ProjectType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KsDustProjects", "ProjectType");
        }
    }
}
