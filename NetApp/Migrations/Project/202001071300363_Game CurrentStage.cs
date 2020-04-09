namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameCurrentStage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games1", "CurrentStage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games1", "CurrentStage");
        }
    }
}
