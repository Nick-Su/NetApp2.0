namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblProjectChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "AuthorId", c => c.String());
            AddColumn("dbo.Projects", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Date");
            DropColumn("dbo.Projects", "AuthorId");
        }
    }
}
