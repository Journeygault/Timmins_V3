namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billstable : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "dbo.BillPayments",
                c => new
                    {
                        BillPaymentID = c.Int(nullable: false, identity: true),
                        CardType = c.String(),
                        CardNumber = c.Int(nullable: false),
                        ExpiryDate = c.String(),
                        CVV = c.Int(nullable: false),
                        DatePaid = c.DateTime(nullable: false),
                        AmountPaid = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BillPaymentID);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        BillID = c.Int(nullable: false, identity: true),
                        DateIssued = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        Breakdown = c.String(),
                    })
                .PrimaryKey(t => t.BillID);
            */
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bills");
            DropTable("dbo.BillPayments");
        }
    }
}
