namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewed_appointment_class : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Appointments", "ApplicationUser_Id");
            AddForeignKey("dbo.Appointments", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Appointments", "ApplicationUser_Id");
        }
    }
}
