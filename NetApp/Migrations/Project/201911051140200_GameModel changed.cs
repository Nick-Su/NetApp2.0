namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameModelchanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games1", "TransitionGroup", c => c.Int(nullable: false));
            AddColumn("dbo.Games1", "PlayerIntroductionTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games1", "PlayerIntroductionTime");
            DropColumn("dbo.Games1", "TransitionGroup");
        }
    }
}
