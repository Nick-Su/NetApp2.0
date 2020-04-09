namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableUpd4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "IdAuthor", c => c.String());
            DropColumn("dbo.Games", "AuthorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "AuthorId", c => c.String());
            DropColumn("dbo.Games", "IdAuthor");
        }
    }
}
