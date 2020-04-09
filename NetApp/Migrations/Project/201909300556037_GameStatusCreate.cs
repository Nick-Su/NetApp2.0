namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameStatusCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "Status");
        }
    }
}
