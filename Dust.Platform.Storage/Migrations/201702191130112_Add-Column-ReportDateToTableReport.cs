namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnReportDateToTableReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "ReportDate", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reports", "ReportDate");
        }
    }
}
