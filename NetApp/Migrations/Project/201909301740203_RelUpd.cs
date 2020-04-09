namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelUpd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tables", "Game_Id", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "Game_Id" });
            DropColumn("dbo.Tables", "Game_Id");
            DropTable("dbo.Games");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameAuthor = c.String(),
                        NumPlayers = c.Int(nullable: false),
                        NumTables = c.Int(nullable: false),
                        NumTacts = c.Int(nullable: false),
                        TactDuration = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        Code = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tables", "Game_Id", c => c.Int());
            CreateIndex("dbo.Tables", "Game_Id");
            AddForeignKey("dbo.Tables", "Game_Id", "dbo.Games", "Id");
        }
    }
}
