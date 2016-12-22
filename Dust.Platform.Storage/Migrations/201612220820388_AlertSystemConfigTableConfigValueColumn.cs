namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlertSystemConfigTableConfigValueColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SystemConfigurations", "ConfigValue", c => c.String(nullable: false, maxLength: 8000, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SystemConfigurations", "ConfigValue", c => c.String(nullable: false, maxLength: 2000, storeType: "nvarchar"));
        }
    }
}
