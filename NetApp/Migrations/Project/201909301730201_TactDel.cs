namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TactDel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tacts", "GameId", "dbo.Games");
            DropIndex("dbo.Tacts", new[] { "GameId" });
            DropTable("dbo.Tacts");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Tacts", "GameId");
            AddForeignKey("dbo.Tacts", "GameId", "dbo.Games", "Id", cascadeDelete: true);
        }
    }
}
