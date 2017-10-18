namespace KlijentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KompanijeDodate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Balance", c => c.Double(nullable: false));
            AddColumn("dbo.Companies", "MonthlyFee", c => c.Double(nullable: false));
            AddColumn("dbo.Companies", "Model", c => c.Double(nullable: false));
            AddColumn("dbo.Companies", "ReferenceNumber", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "ReferenceNumber");
            DropColumn("dbo.Companies", "Model");
            DropColumn("dbo.Companies", "MonthlyFee");
            DropColumn("dbo.Companies", "Balance");
        }
    }
}
