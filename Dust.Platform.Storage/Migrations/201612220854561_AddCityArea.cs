namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCityArea : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustProjects", "CityArea", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KsDustProjects", "CityArea");
        }
    }
}
