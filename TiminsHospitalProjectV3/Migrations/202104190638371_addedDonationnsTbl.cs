namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDonationnsTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        DonationID = c.Int(nullable: false, identity: true),
                        FistName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        Country = c.String(),
                        Zip = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        CardName = c.String(),
                        CardNumber = c.String(),
                        ExpiryDate = c.DateTime(nullable: false),
                        CVV = c.Int(nullable: false),
                        CardType = c.String(),
                        CompanyName = c.String(),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonationID)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "EventId", "dbo.Events");
            DropIndex("dbo.Donations", new[] { "EventId" });
            DropTable("dbo.Donations");
        }
    }
}
