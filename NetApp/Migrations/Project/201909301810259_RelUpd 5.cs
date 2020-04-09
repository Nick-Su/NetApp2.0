namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelUpd5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        TableId = c.Int(nullable: false, identity: true),
                        TableNumber = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TableId)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "GameId", "dbo.Games");
            DropIndex("dbo.Tables", new[] { "GameId" });
            DropTable("dbo.Tables");
        }
    }
}
