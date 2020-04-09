namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelUpd10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Games", "Table_TableId", "dbo.Tables");
            DropForeignKey("dbo.Tacts", "GameId", "dbo.Games");
            DropIndex("dbo.Games", new[] { "Table_TableId" });
            DropIndex("dbo.Tacts", new[] { "GameId" });
            AddColumn("dbo.Tables", "GameId", c => c.Int(nullable: false));
            DropColumn("dbo.Games", "Table_TableId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "Table_TableId", c => c.Int());
            DropColumn("dbo.Tables", "GameId");
            CreateIndex("dbo.Tacts", "GameId");
            CreateIndex("dbo.Games", "Table_TableId");
            AddForeignKey("dbo.Tacts", "GameId", "dbo.Games", "GameId", cascadeDelete: true);
            AddForeignKey("dbo.Games", "Table_TableId", "dbo.Tables", "TableId");
        }
    }
}
