namespace KlijentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class balancecheckdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "BalanceCheckDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            DropColumn("dbo.Companies", "Balance");
            DropColumn("dbo.Companies", "BalanceDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "BalanceDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AddColumn("dbo.Companies", "Balance", c => c.Double(nullable: false));
            DropColumn("dbo.Companies", "BalanceCheckDate");
        }
    }
}
