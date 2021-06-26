namespace FlightPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Airports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Country = c.String(),
                        City = c.String(),
                        AirportName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Flights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Carrier = c.String(),
                        DepartureTime = c.String(),
                        ArrivalTime = c.String(),
                        From_Id = c.Int(),
                        To_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Airports", t => t.From_Id)
                .ForeignKey("dbo.Airports", t => t.To_Id)
                .Index(t => t.From_Id)
                .Index(t => t.To_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Flights", "To_Id", "dbo.Airports");
            DropForeignKey("dbo.Flights", "From_Id", "dbo.Airports");
            DropIndex("dbo.Flights", new[] { "To_Id" });
            DropIndex("dbo.Flights", new[] { "From_Id" });
            DropTable("dbo.Flights");
            DropTable("dbo.Airports");
        }
    }
}
