namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointments_v7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "JobID", "dbo.Job_Posting");
            DropIndex("dbo.Employees", new[] { "JobID" });
            DropTable("dbo.Employees");
            DropTable("dbo.Job_Posting");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Job_Posting",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        JobName = c.String(),
                    })
                .PrimaryKey(t => t.JobID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        EmployeeFirstname = c.String(),
                        EmployeeLastname = c.String(),
                        EmployeePhone = c.String(),
                        EmployeeEmail = c.String(),
                        EmployeeAddress = c.String(),
                        EmployeeDes = c.String(),
                        EmployeeHasPic = c.Boolean(nullable: false),
                        EmployeeResume = c.String(),
                        JobID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
            CreateIndex("dbo.Employees", "JobID");
            AddForeignKey("dbo.Employees", "JobID", "dbo.Job_Posting", "JobID", cascadeDelete: true);
        }
    }
}
