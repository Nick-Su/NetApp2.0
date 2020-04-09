namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Games1", "IsOnlyGivenConTypes", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Games1", "IsOnlyGivenNeeds", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Games1", "IsOnlyGivenBenefits", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Games1", "IsOnlyGivenBenefits", c => c.Int(nullable: false));
            AlterColumn("dbo.Games1", "IsOnlyGivenNeeds", c => c.Int(nullable: false));
            AlterColumn("dbo.Games1", "IsOnlyGivenConTypes", c => c.Int(nullable: false));
        }
    }
}
