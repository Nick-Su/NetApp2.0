namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NeedType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NeedTypes",
                c => new
                    {
                        NeedTypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NeedTypeId)
                .ForeignKey("dbo.Games1", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
            AddColumn("dbo.Games1", "IsOnlyGivenNeeds", c => c.Int(nullable: false));
            AddColumn("dbo.Games1", "IsOnlyGivenBenefits", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NeedTypes", "GameId", "dbo.Games1");
            DropIndex("dbo.NeedTypes", new[] { "GameId" });
            DropColumn("dbo.Games1", "IsOnlyGivenBenefits");
            DropColumn("dbo.Games1", "IsOnlyGivenNeeds");
            DropTable("dbo.NeedTypes");
        }
    }
}
