namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelUpd7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tables", "GameId", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "GameId" });
            RenameColumn(table: "dbo.Tables", name: "GameId", newName: "Game_GameId");
            AlterColumn("dbo.Tables", "Game_GameId", c => c.Int());
            CreateIndex("dbo.Tables", "Game_GameId");
            AddForeignKey("dbo.Tables", "Game_GameId", "dbo.Games", "GameId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "Game_GameId", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "Game_GameId" });
            AlterColumn("dbo.Tables", "Game_GameId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Tables", name: "Game_GameId", newName: "GameId");
            CreateIndex("dbo.Tables", "GameId");
            AddForeignKey("dbo.Tables", "GameId", "dbo.Games", "GameId", cascadeDelete: true);
        }
    }
}
