namespace Dust.Platform.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDayWeather : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DayWeathers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        DayText = c.String(unicode: false),
                        DayCode = c.String(unicode: false),
                        NightText = c.String(unicode: false),
                        NightCode = c.String(unicode: false),
                        TemperatureHigh = c.Double(nullable: false),
                        TemperatureLow = c.Double(nullable: false),
                        WindDirection = c.String(unicode: false),
                        WindDirectionDegree = c.Double(nullable: false),
                        WindSpeed = c.Double(nullable: false),
                        WindScale = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DayWeathers");
        }
    }
}
