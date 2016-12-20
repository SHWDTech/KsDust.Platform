namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectStopped : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustProjects", "Stopped", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KsDustProjects", "Stopped");
        }
    }
}
