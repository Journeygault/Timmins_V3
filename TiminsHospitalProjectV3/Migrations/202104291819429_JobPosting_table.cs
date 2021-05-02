namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobPosting_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.JobPostings",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        JobCategory = c.String(nullable: false),
                        JobTitle = c.String(nullable: false),
                        JobDescription = c.String(nullable: false),
                        JobLocation = c.String(nullable: false),
                        PositionType = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        SalaryRange = c.String(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobPostings", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.JobPostings", new[] { "DepartmentID" });
            DropTable("dbo.JobPostings");
            DropTable("dbo.Departments");
        }
    }
}
