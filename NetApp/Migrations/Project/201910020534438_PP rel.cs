namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PPrel : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ProjectParticipants", "GameId");
            AddForeignKey("dbo.ProjectParticipants", "GameId", "dbo.Games", "GameId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectParticipants", "GameId", "dbo.Games");
            DropIndex("dbo.ProjectParticipants", new[] { "GameId" });
        }
    }
}
