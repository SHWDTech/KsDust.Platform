namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContractRecordRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KsDustProjects", "ContractRecord", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KsDustProjects", "ContractRecord", c => c.String(unicode: false));
        }
    }
}
