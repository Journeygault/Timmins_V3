namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donationTypeChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Donations", "CardNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Donations", "CardNumber", c => c.String());
        }
    }
}
