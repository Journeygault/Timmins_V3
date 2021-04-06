namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewsItemTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsItems",
                c => new
                    {
                        NewsItemID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        NewsBody = c.String(),
                        NewItemDate = c.DateTime(nullable: false),
                        NewsItemHasPic = c.Boolean(nullable: false),
                        NewsItemPicExtension = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NewsItemID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NewsItems");
        }
    }
}
