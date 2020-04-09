namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRatingrenamefield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRatings", "UserId", c => c.String());
            DropColumn("dbo.UserRatings", "AuthorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRatings", "AuthorId", c => c.String());
            DropColumn("dbo.UserRatings", "UserId");
        }
    }
}
