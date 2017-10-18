namespace KlijentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Kompanije : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Clients", newName: "Transactions");
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyDescription = c.String(),
                        Comment = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            AddColumn("dbo.Transactions", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Transactions", "CompanyId");
            AddForeignKey("dbo.Transactions", "CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: true);
            DropColumn("dbo.Transactions", "CompanyDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "CompanyDescription", c => c.String());
            DropForeignKey("dbo.Transactions", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Transactions", new[] { "CompanyId" });
            DropColumn("dbo.Transactions", "CompanyId");
            DropTable("dbo.Companies");
            RenameTable(name: "dbo.Transactions", newName: "Clients");
        }
    }
}
