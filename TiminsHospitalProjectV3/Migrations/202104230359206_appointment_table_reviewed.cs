namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointment_table_reviewed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "RequestDatetime", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "Subject", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "Message", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "SentOn", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "SentOn", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Appointments", "Message", c => c.String(nullable: false, maxLength: 2000));
            AlterColumn("dbo.Appointments", "Subject", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Appointments", "RequestDatetime", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
