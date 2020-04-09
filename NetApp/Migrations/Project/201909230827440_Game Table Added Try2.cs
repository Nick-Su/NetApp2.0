namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameTableAddedTry2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.String(),
                        NumPlayers = c.Int(nullable: false),
                        NumTables = c.Int(nullable: false),
                        NumTacts = c.Int(nullable: false),
                        TactDuration = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        Code = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Games");
        }
    }
}
