namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeviceVendorInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustDevices", "VendorId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KsDustDevices", "VendorId");
        }
    }
}
