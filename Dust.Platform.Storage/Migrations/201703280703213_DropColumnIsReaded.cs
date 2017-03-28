namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropColumnIsReaded : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.KsDustAlarms", "IsReaded");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KsDustAlarms", "IsReaded", c => c.Boolean(nullable: false));
        }
    }
}
