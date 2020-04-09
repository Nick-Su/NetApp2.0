namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GamesTimetoaproverequests : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games1", "TimeToAproveRequests", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games1", "TimeToAproveRequests");
        }
    }
}
