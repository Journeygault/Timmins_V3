namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userinticket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tickets", "UserID");
            AddForeignKey("dbo.Tickets", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Tickets", new[] { "UserID" });
            DropColumn("dbo.Tickets", "UserID");
        }
    }
}
