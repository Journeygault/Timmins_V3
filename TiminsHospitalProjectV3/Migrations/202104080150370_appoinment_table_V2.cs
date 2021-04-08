namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appoinment_table_V2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PatientID = c.String(maxLength: 128),
                        PhysicianID = c.String(maxLength: 128),
                        RequestDatetime = c.DateTime(nullable: false),
                        Subject = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        SentOn = c.DateTime(nullable: false),
                        DecisionMadeOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.PhysicianID)
                .ForeignKey("dbo.AspNetUsers", t => t.PatientID)
                .Index(t => t.PatientID)
                .Index(t => t.PhysicianID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "PatientID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "PhysicianID", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "PhysicianID" });
            DropIndex("dbo.Appointments", new[] { "PatientID" });
            DropTable("dbo.Appointments");
        }
    }
}
