namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelUpd9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tables", "GameId", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "GameId" });
            AddColumn("dbo.Games", "Table_TableId", c => c.Int());
            CreateIndex("dbo.Games", "Table_TableId");
            AddForeignKey("dbo.Games", "Table_TableId", "dbo.Tables", "TableId");
            DropColumn("dbo.Tables", "GameId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tables", "GameId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Games", "Table_TableId", "dbo.Tables");
            DropIndex("dbo.Games", new[] { "Table_TableId" });
            DropColumn("dbo.Games", "Table_TableId");
            CreateIndex("dbo.Tables", "GameId");
            AddForeignKey("dbo.Tables", "GameId", "dbo.Games", "GameId", cascadeDelete: true);
        }
    }
}
