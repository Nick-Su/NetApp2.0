namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TactNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tacts", "TactNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tacts", "TactNumber");
        }
    }
}
