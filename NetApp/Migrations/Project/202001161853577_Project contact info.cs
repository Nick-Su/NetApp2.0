namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Projectcontactinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Contact", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Contact");
        }
    }
}
