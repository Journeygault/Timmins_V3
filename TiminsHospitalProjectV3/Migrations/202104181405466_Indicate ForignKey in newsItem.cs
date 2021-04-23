namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndicateForignKeyinnewsItem : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NewsItems", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.NewsItems", "UserID");
            AddForeignKey("dbo.NewsItems", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsItems", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.NewsItems", new[] { "UserID" });
            AlterColumn("dbo.NewsItems", "UserID", c => c.String());
        }
    }
}
