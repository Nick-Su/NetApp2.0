namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PPaddcolumns : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectParticipants", "Game_Id", "dbo.Games");
            DropIndex("dbo.ProjectParticipants", new[] { "Game_Id" });
            RenameColumn(table: "dbo.ProjectParticipants", name: "Game_Id", newName: "GameId");
            AddColumn("dbo.ProjectParticipants", "ProjectId", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectParticipants", "GameId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectParticipants", "GameId");
            AddForeignKey("dbo.ProjectParticipants", "GameId", "dbo.Games", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectParticipants", "GameId", "dbo.Games");
            DropIndex("dbo.ProjectParticipants", new[] { "GameId" });
            AlterColumn("dbo.ProjectParticipants", "GameId", c => c.Int());
            DropColumn("dbo.ProjectParticipants", "ProjectId");
            RenameColumn(table: "dbo.ProjectParticipants", name: "GameId", newName: "Game_Id");
            CreateIndex("dbo.ProjectParticipants", "Game_Id");
            AddForeignKey("dbo.ProjectParticipants", "Game_Id", "dbo.Games", "Id");
        }
    }
}
