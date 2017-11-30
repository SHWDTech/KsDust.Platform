namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVenderoShortCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "ShortCode", c => c.String(nullable: false, maxLength: 4, storeType: "nvarchar", defaultValueSql:""));
            CreateIndex("dbo.Vendors", "ShortCode", unique: true, name: "Ix_ShortCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Vendors", "Ix_ShortCode");
            DropColumn("dbo.Vendors", "ShortCode");
        }
    }
}
