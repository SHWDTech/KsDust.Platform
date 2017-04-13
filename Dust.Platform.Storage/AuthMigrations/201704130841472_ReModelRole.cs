namespace Dust.Platform.Storage.AuthMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReModelRole : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.DustRoles", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DustRoles", "Name", c => c.String(unicode: false));
        }
    }
}
