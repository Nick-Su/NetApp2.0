namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PPfuckadd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectParticipants", "GameId", "dbo.Games");
            DropIndex("dbo.ProjectParticipants", new[] { "GameId" });
            RenameColumn(table: "dbo.Projects", name: "ProjectParticipantId", newName: "ProjectParticipant_Id");
            RenameIndex(table: "dbo.Projects", name: "IX_ProjectParticipantId", newName: "IX_ProjectParticipant_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Projects", name: "IX_ProjectParticipant_Id", newName: "IX_ProjectParticipantId");
            RenameColumn(table: "dbo.Projects", name: "ProjectParticipant_Id", newName: "ProjectParticipantId");
            CreateIndex("dbo.ProjectParticipants", "GameId");
            AddForeignKey("dbo.ProjectParticipants", "GameId", "dbo.Games", "Id", cascadeDelete: true);
        }
    }
}
