namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.JobPostings",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        JobCategory = c.String(),
                        JobTitle = c.String(),
                        JobDescription = c.String(),
                        JobLocation = c.String(),
                        PositionType = c.String(),
                        Email = c.String(),
                        SalaryRange = c.String(),
                        DatePosted = c.String(),
                        Department_DepartmentId = c.Int(),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentId)
                .Index(t => t.Department_DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobPostings", "Department_DepartmentId", "dbo.Departments");
            DropIndex("dbo.JobPostings", new[] { "Department_DepartmentId" });
            DropTable("dbo.JobPostings");
            DropTable("dbo.Departments");
        }
    }
}
