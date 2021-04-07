namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class faqtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Faqs",
                c => new
                    {
                        FaqID = c.Int(nullable: false, identity: true),
                        FaqQuestion = c.String(),
                        FaqAnswer = c.String(),
                    })
                .PrimaryKey(t => t.FaqID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Faqs");
        }
    }
}
