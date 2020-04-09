namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelUpd3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tacts",
                c => new
                    {
                        TactId = c.Int(nullable: false, identity: true),
                        TimeStart = c.DateTime(nullable: false),
                        TimeEnd = c.DateTime(nullable: false),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TactId)
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
