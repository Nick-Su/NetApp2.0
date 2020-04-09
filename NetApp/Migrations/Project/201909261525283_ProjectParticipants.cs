namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectParticipants : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectParticipants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
            AddColumn("dbo.Projects", "ProjectParticipant_Id", c => c.Int());
            CreateIndex("dbo.Projects", "ProjectParticipant_Id");
            AddForeignKey("dbo.Projects", "ProjectParticipant_Id", "dbo.ProjectParticipants", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ProjectParticipant_Id", "dbo.ProjectParticipants");
            DropForeignKey("dbo.ProjectParticipants", "Game_Id", "dbo.Games");
            DropIndex("dbo.Projects", new[] { "ProjectParticipant_Id" });
            DropIndex("dbo.ProjectParticipants", new[] { "Game_Id" });
            DropColumn("dbo.Projects", "ProjectParticipant_Id");
            DropTable("dbo.ProjectParticipants");
        }
    }
}
