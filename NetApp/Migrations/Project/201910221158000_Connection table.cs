namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Connectiontable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        ConnectionId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        RecieverId = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConnectionId)
                .ForeignKey("dbo.Games1", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Connections", "GameId", "dbo.Games1");
            DropIndex("dbo.Connections", new[] { "GameId" });
            DropTable("dbo.Connections");
        }
    }
}
