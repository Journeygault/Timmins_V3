namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forignKey : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Events", "UserID");
            AddForeignKey("dbo.Events", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "UserID" });
            AlterColumn("dbo.Events", "UserID", c => c.Int(nullable: false));
        }
    }
}
