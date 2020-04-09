namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TactsUpdRel : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Tacts", "GameId");
            AddForeignKey("dbo.Tacts", "GameId", "dbo.Games", "GameId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tacts", "GameId", "dbo.Games");
            DropIndex("dbo.Tacts", new[] { "GameId" });
        }
    }
}
