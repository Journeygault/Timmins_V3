namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donationsupdates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Donations", "FistName", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "PhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "Country", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "Zip", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "Province", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "CardName", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "CardType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Donations", "CardType", c => c.String());
            AlterColumn("dbo.Donations", "CardName", c => c.String());
            AlterColumn("dbo.Donations", "City", c => c.String());
            AlterColumn("dbo.Donations", "Province", c => c.String());
            AlterColumn("dbo.Donations", "Zip", c => c.String());
            AlterColumn("dbo.Donations", "Country", c => c.String());
            AlterColumn("dbo.Donations", "Address", c => c.String());
            AlterColumn("dbo.Donations", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Donations", "Email", c => c.String());
            AlterColumn("dbo.Donations", "LastName", c => c.String());
            AlterColumn("dbo.Donations", "FistName", c => c.String());
        }
    }
}
