namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTables2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobPostings", "DatePosted", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobPostings", "DatePosted", c => c.String());
        }
    }
}
