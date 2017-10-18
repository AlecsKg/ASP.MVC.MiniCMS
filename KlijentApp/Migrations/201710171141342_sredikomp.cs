namespace KlijentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sredikomp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Mod", c => c.String());
            AddColumn("dbo.Companies", "RefNumber", c => c.String());
            DropColumn("dbo.Companies", "Model");
            DropColumn("dbo.Companies", "ReferenceNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "ReferenceNumber", c => c.Double(nullable: false));
            AddColumn("dbo.Companies", "Model", c => c.Double(nullable: false));
            DropColumn("dbo.Companies", "RefNumber");
            DropColumn("dbo.Companies", "Mod");
        }
    }
}
