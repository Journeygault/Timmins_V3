namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge_tables : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Job_Posting", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.Job_Posting",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        JobName = c.String(),
                    })
                .PrimaryKey(t => t.JobID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "JobID", "dbo.Job_Posting");
            DropIndex("dbo.Employees", new[] { "JobID" });
            DropTable("dbo.Job_Posting");
            DropTable("dbo.Employees");
        }
    }
}
