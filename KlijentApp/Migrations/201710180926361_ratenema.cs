namespace KlijentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratenema : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Companies", "MonthlyFee");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "MonthlyFee", c => c.Double(nullable: false));
        }
    }
}
