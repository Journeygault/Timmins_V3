namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewmodelupdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReviewDtoes",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        ReviewDate = c.DateTime(nullable: false),
                        ReviewRating = c.Int(nullable: false),
                        ReviewContent = c.String(),
                    })
                .PrimaryKey(t => t.ReviewID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ReviewDtoes");
        }
    }
}
