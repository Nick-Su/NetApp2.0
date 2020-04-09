namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationsystemConReqisReadprops : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConnectionRequests", "SenderIsRead", c => c.Int(nullable: false));
            AddColumn("dbo.ConnectionRequests", "RecieverIsRead", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConnectionRequests", "RecieverIsRead");
            DropColumn("dbo.ConnectionRequests", "SenderIsRead");
        }
    }
}
