namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableUpd5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Author", c => c.String());
            DropColumn("dbo.Games", "IdAuthor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "IdAuthor", c => c.String());
            DropColumn("dbo.Games", "Author");
        }
    }
}
