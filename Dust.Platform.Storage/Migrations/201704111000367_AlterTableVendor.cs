namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableVendor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vendors", "Name", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AlterColumn("dbo.Vendors", "Susperintend", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AlterColumn("dbo.Vendors", "Mobile", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vendors", "Mobile", c => c.String(maxLength: 100, storeType: "nvarchar"));
            AlterColumn("dbo.Vendors", "Susperintend", c => c.String(maxLength: 100, storeType: "nvarchar"));
            AlterColumn("dbo.Vendors", "Name", c => c.String(maxLength: 100, storeType: "nvarchar"));
        }
    }
}
