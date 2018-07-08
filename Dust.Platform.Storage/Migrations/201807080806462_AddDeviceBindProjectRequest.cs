namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeviceBindProjectRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustDevices", "ProjectBindRequest", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KsDustDevices", "ProjectBindRequest");
        }
    }
}
