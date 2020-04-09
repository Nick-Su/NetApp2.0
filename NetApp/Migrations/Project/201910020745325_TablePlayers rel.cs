namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablePlayersrel : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TablePlayers", "TableId");
            AddForeignKey("dbo.TablePlayers", "TableId", "dbo.Tables", "TableId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TablePlayers", "TableId", "dbo.Tables");
            DropIndex("dbo.TablePlayers", new[] { "TableId" });
        }
    }
}
