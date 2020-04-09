namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectionType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConnectionTypes",
                c => new
                    {
                        ConnectionTypeId = c.Int(nullable: false, identity: true),
                        Decription = c.String(),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConnectionTypeId)
                .ForeignKey("dbo.Games1", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
            AddColumn("dbo.Games1", "IsOnlyGivenConTypes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConnectionTypes", "GameId", "dbo.Games1");
            DropIndex("dbo.ConnectionTypes", new[] { "GameId" });
            DropColumn("dbo.Games1", "IsOnlyGivenConTypes");
            DropTable("dbo.ConnectionTypes");
        }
    }
}
