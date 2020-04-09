namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablePlayersCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TablePlayers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        ProjectParticipantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tables", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TablePlayers", "TableId", "dbo.Tables");
            DropIndex("dbo.TablePlayers", new[] { "TableId" });
            DropTable("dbo.TablePlayers");
        }
    }
}
