namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebillmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillDtoes",
                c => new
                    {
                        BillID = c.Int(nullable: false, identity: true),
                        DateIssued = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        Breakdown = c.String(),
                    })
                .PrimaryKey(t => t.BillID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BillDtoes");
        }
    }
}
