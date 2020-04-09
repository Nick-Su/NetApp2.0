namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TactsCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameId = c.Int(nullable: false),
                        TimeStart = c.DateTime(nullable: false),
                        TimeEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tacts", "GameId", "dbo.Games");
            DropIndex("dbo.Tacts", new[] { "GameId" });
            DropTable("dbo.Tacts");
        }
    }
}
