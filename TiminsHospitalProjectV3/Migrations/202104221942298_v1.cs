namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "EmployeeFirstname", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "EmployeeLastname", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "EmployeePhone", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "EmployeeEmail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "EmployeeEmail", c => c.String());
            AlterColumn("dbo.Employees", "EmployeePhone", c => c.String());
            AlterColumn("dbo.Employees", "EmployeeLastname", c => c.String());
            AlterColumn("dbo.Employees", "EmployeeFirstname", c => c.String());
        }
    }
}
