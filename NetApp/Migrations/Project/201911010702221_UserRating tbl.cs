namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRatingtbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRatings",
                c => new
                    {
                        UserRatingId = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        AuthorId = c.String(),
                    })
                .PrimaryKey(t => t.UserRatingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRatings");
        }
    }
}
