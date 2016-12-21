namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlertContractRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KsDustProjects", "ContractRecord", c => c.String(unicode: false));
            DropColumn("dbo.KsDustProjects", "ContracRecord");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KsDustProjects", "ContracRecord", c => c.String(unicode: false));
            DropColumn("dbo.KsDustProjects", "ContractRecord");
        }
    }
}
