namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewed_appointment_4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "RequestDatetime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Appointments", "SentOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Appointments", "DecisionMadeOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "DecisionMadeOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Appointments", "SentOn", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "RequestDatetime", c => c.String(nullable: false));
        }
    }
}
