namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelUpd6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Author", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "Author");
        }
    }
}
