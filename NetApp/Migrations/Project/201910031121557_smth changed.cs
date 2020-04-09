namespace NetApp.Migrations.Project
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smthchanged : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Games", newName: "__mig_tmp__0");
            RenameTable(name: "dbo.ProjectParticipants", newName: "Games");
            RenameTable(name: "__mig_tmp__0", newName: "Games1");
        }
        
        public override void Down()
        {
            RenameTable(name: "Games1", newName: "__mig_tmp__0");
            RenameTable(name: "dbo.Games", newName: "ProjectParticipants");
            RenameTable(name: "dbo.__mig_tmp__0", newName: "Games");
        }
    }
}
