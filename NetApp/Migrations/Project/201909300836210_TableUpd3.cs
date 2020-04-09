namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableUpd3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tables", "Game_Id", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "Game_Id" });
            RenameColumn(table: "dbo.Tables", name: "Game_Id", newName: "GameId");
            AlterColumn("dbo.Tables", "GameId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tables", "GameId");
            AddForeignKey("dbo.Tables", "GameId", "dbo.Games", "Id", cascadeDelete: true);
            DropColumn("dbo.Tables", "IdGame");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tables", "IdGame", c => c.Int(nullable: false));
            DropForeignKey("dbo.Tables", "GameId", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "GameId" });
            AlterColumn("dbo.Tables", "GameId", c => c.Int());
            RenameColumn(table: "dbo.Tables", name: "GameId", newName: "Game_Id");
            CreateIndex("dbo.Tables", "Game_Id");
            AddForeignKey("dbo.Tables", "Game_Id", "dbo.Games", "Id");
        }
    }
}
