namespace TiminsHospitalProjectV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedEventHasOCcuredToEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventHasOcured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "EventHasOcured");
        }
    }
}
