namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsforignkeyinttostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NewsItems", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NewsItems", "UserID", c => c.Int(nullable: false));
        }
    }
}
