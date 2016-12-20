namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEnterpriseOuterId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Enterprises", "OuterId", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Enterprises", "OuterId");
        }
    }
}
