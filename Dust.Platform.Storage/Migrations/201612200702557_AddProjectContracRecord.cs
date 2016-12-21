namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectContracRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustProjects", "ContractRecord", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KsDustProjects", "ContractRecord");
        }
    }
}
