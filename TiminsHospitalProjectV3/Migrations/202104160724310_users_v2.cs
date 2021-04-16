namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class users_v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "LatstName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LatstName", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "LastName");
        }
    }
}
