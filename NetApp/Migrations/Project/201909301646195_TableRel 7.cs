namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableRel7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tables", "GameId", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "GameId" });
            RenameColumn(table: "dbo.Tables", name: "GameId", newName: "Game_Id");
            AddColumn("dbo.Games", "GameAuthor", c => c.String());
            AlterColumn("dbo.Tables", "Game_Id", c => c.Int());
            CreateIndex("dbo.Tables", "Game_Id");
            AddForeignKey("dbo.Tables", "Game_Id", "dbo.Games", "Id");
            DropColumn("dbo.Games", "Author");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "Author", c => c.String());
            DropForeignKey("dbo.Tables", "Game_Id", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "Game_Id" });
            AlterColumn("dbo.Tables", "Game_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Games", "GameAuthor");
            RenameColumn(table: "dbo.Tables", name: "Game_Id", newName: "GameId");
            CreateIndex("dbo.Tables", "GameId");
            AddForeignKey("dbo.Tables", "GameId", "dbo.Games", "Id", cascadeDelete: true);
        }
    }
}
