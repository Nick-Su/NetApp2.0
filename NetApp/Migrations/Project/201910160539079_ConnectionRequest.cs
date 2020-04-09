namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectionRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConnectionRequests",
                c => new
                    {
                        ConnectionRequestId = c.Int(nullable: false, identity: true),
                        ConnectionType = c.String(),
                        SenderResourceRequest = c.String(),
                        SenderGivenBenefit = c.String(),
                        SenderProjectId = c.Int(nullable: false),
                        RecieverGetResource = c.String(),
                        RecieverGetBenefit = c.String(),
                        RecieverProjectId = c.Int(nullable: false),
                        IsApproved = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConnectionRequestId)
                .ForeignKey("dbo.Games1", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConnectionRequests", "GameId", "dbo.Games1");
            DropIndex("dbo.ConnectionRequests", new[] { "GameId" });
            DropTable("dbo.ConnectionRequests");
        }
    }
}
