namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableUpd2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tables", "IdGame", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tables", "IdGame");
        }
    }
}
