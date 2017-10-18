namespace KlijentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BalanceDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "BalanceDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "BalanceDate");
        }
    }
}
