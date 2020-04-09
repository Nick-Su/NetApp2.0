namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CurrentTactGameTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "CurrentTact", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "CurrentTact");
        }
    }
}
