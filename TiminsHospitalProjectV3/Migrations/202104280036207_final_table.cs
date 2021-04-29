namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class final_table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobPostings", "Department_DepartmentId", "dbo.Departments");
            DropIndex("dbo.JobPostings", new[] { "Department_DepartmentId" });
            RenameColumn(table: "dbo.JobPostings", name: "Department_DepartmentId", newName: "DepartmentID");
            AlterColumn("dbo.Departments", "DepartmentName", c => c.String(nullable: false));
            AlterColumn("dbo.JobPostings", "JobCategory", c => c.String(nullable: false));
            AlterColumn("dbo.JobPostings", "JobTitle", c => c.String(nullable: false));
            AlterColumn("dbo.JobPostings", "JobDescription", c => c.String(nullable: false));
            AlterColumn("dbo.JobPostings", "JobLocation", c => c.String(nullable: false));
            AlterColumn("dbo.JobPostings", "PositionType", c => c.String(nullable: false));
            AlterColumn("dbo.JobPostings", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.JobPostings", "SalaryRange", c => c.String(nullable: false));
            AlterColumn("dbo.JobPostings", "DepartmentID", c => c.Int(nullable: false));
            CreateIndex("dbo.JobPostings", "DepartmentID");
            AddForeignKey("dbo.JobPostings", "DepartmentID", "dbo.Departments", "DepartmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobPostings", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.JobPostings", new[] { "DepartmentID" });
            AlterColumn("dbo.JobPostings", "DepartmentID", c => c.Int());
            AlterColumn("dbo.JobPostings", "SalaryRange", c => c.String());
            AlterColumn("dbo.JobPostings", "Email", c => c.String());
            AlterColumn("dbo.JobPostings", "PositionType", c => c.String());
            AlterColumn("dbo.JobPostings", "JobLocation", c => c.String());
            AlterColumn("dbo.JobPostings", "JobDescription", c => c.String());
            AlterColumn("dbo.JobPostings", "JobTitle", c => c.String());
            AlterColumn("dbo.JobPostings", "JobCategory", c => c.String());
            AlterColumn("dbo.Departments", "DepartmentName", c => c.String());
            RenameColumn(table: "dbo.JobPostings", name: "DepartmentID", newName: "Department_DepartmentId");
            CreateIndex("dbo.JobPostings", "Department_DepartmentId");
            AddForeignKey("dbo.JobPostings", "Department_DepartmentId", "dbo.Departments", "DepartmentId");
        }
    }
}
