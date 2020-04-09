namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrashEvrythingMtM : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "ProjectParticipant_Id", "dbo.ProjectParticipants");
            DropIndex("dbo.Projects", new[] { "ProjectParticipant_Id" });
            CreateTable(
                "dbo.ProjectGames",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Game_GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Game_GameId })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.Game_GameId, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Game_GameId);
            
            DropColumn("dbo.Projects", "ProjectParticipant_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "ProjectParticipant_Id", c => c.Int());
            DropForeignKey("dbo.ProjectGames", "Game_GameId", "dbo.Games");
            DropForeignKey("dbo.ProjectGames", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectGames", new[] { "Game_GameId" });
            DropIndex("dbo.ProjectGames", new[] { "Project_Id" });
            DropTable("dbo.ProjectGames");
            CreateIndex("dbo.Projects", "ProjectParticipant_Id");
            AddForeignKey("dbo.Projects", "ProjectParticipant_Id", "dbo.ProjectParticipants", "Id");
        }
    }
}
