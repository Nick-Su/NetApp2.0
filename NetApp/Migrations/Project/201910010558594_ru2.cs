namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ru2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Tables");
            DropColumn("dbo.Tables", "Id");
            AddColumn("dbo.Tables", "TableId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tables", "TableId");
           
        }
        
        public override void Down()
        {
           
            DropPrimaryKey("dbo.Tables");
            DropColumn("dbo.Tables", "TableId");
            AddColumn("dbo.Tables", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tables", "Id");
        }
    }
}
