namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyCameraPropertyName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustCameras", "SerialNumber", c => c.String(maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.KsDustCameras", "ConnectName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KsDustCameras", "ConnectName", c => c.String(maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.KsDustCameras", "SerialNumber");
        }
    }
}
