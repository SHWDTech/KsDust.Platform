namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeviceVendorForeignKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.KsDustDevices", "VendorId");
            AddForeignKey("dbo.KsDustDevices", "VendorId", "dbo.Vendors", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KsDustDevices", "VendorId", "dbo.Vendors");
            DropIndex("dbo.KsDustDevices", new[] { "VendorId" });
        }
    }
}
