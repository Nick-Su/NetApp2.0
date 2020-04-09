namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelUpd8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tables", "Game_GameId", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "Game_GameId" });
            RenameColumn(table: "dbo.Tables", name: "Game_GameId", newName: "GameId");
            AlterColumn("dbo.Tables", "GameId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tables", "GameId");
            AddForeignKey("dbo.Tables", "GameId", "dbo.Games", "GameId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "GameId", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "GameId" });
            AlterColumn("dbo.Tables", "GameId", c => c.Int());
            RenameColumn(table: "dbo.Tables", name: "GameId", newName: "Game_GameId");
            CreateIndex("dbo.Tables", "Game_GameId");
            AddForeignKey("dbo.Tables", "Game_GameId", "dbo.Games", "GameId");
        }
    }
}
