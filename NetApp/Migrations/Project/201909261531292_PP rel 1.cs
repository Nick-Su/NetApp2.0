namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PPrel1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Projects", name: "ProjectParticipant_Id", newName: "ProjectParticipantId");
            RenameIndex(table: "dbo.Projects", name: "IX_ProjectParticipant_Id", newName: "IX_ProjectParticipantId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Projects", name: "IX_ProjectParticipantId", newName: "IX_ProjectParticipant_Id");
            RenameColumn(table: "dbo.Projects", name: "ProjectParticipantId", newName: "ProjectParticipant_Id");
        }
    }
}
