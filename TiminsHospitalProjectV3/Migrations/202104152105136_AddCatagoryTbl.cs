namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCatagoryTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            AddColumn("dbo.Faqs", "CategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Faqs", "CategoryID");
            AddForeignKey("dbo.Faqs", "CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Faqs", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Faqs", new[] { "CategoryID" });
            DropColumn("dbo.Faqs", "CategoryID");
            DropTable("dbo.Categories");
        }
    }
}
