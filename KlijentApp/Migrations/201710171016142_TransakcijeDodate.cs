namespace KlijentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransakcijeDodate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Amount", c => c.Double(nullable: false));
            AddColumn("dbo.Transactions", "Contact", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Contact");
            DropColumn("dbo.Transactions", "Amount");
        }
    }
}
