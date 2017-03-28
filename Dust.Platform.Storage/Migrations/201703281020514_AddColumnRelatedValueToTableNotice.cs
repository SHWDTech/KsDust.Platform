namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnRelatedValueToTableNotice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notices", "RelatedValue", c => c.String(maxLength: 36, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notices", "RelatedValue");
        }
    }
}
