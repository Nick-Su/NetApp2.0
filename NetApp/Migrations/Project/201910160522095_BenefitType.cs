namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenefitType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BenefitTypes",
                c => new
                    {
                        BenefitTypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BenefitTypeId)
                .ForeignKey("dbo.Games1", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BenefitTypes", "GameId", "dbo.Games1");
            DropIndex("dbo.BenefitTypes", new[] { "GameId" });
            DropTable("dbo.BenefitTypes");
        }
    }
}
