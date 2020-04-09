namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deletetimestampfromtacts : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tacts", "TimeStart");
            DropColumn("dbo.Tacts", "TimeEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tacts", "TimeEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tacts", "TimeStart", c => c.DateTime(nullable: false));
        }
    }
}
