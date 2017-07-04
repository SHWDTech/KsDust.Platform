namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExceedphotoThumb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExceedPhotoes", "PhotoThumbPath", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExceedPhotoes", "PhotoThumbPath");
        }
    }
}
