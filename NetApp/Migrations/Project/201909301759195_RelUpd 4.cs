namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelUpd4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TablePlayers", "TableId", "dbo.Tables");
            DropIndex("dbo.TablePlayers", new[] { "TableId" });
            DropTable("dbo.Tables");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.TablePlayers", "TableId");
            AddForeignKey("dbo.TablePlayers", "TableId", "dbo.Tables", "Id", cascadeDelete: true);
        }
    }
}
