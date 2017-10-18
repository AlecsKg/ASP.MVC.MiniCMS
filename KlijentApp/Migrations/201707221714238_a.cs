namespace KlijentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "NumberOfDocTypesOut", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "XmlOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "PdfOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "NumberOfDocTypesIn", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "XmlIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "PdfIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "OtherFormatIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "OtherFormatOut", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "NeededConversion", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "UpdatedOn", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AddColumn("dbo.Transactions", "In", c => c.Boolean(nullable: false));
            AddColumn("dbo.Transactions", "Out", c => c.Boolean(nullable: false));
            DropColumn("dbo.Transactions", "NumberOfDocTypes");
            DropColumn("dbo.Transactions", "Xml");
            DropColumn("dbo.Transactions", "Pdf");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "Pdf", c => c.Boolean(nullable: false));
            AddColumn("dbo.Transactions", "Xml", c => c.Boolean(nullable: false));
            AddColumn("dbo.Transactions", "NumberOfDocTypes", c => c.Int(nullable: false));
            DropColumn("dbo.Transactions", "Out");
            DropColumn("dbo.Transactions", "In");
            DropColumn("dbo.Companies", "UpdatedOn");
            DropColumn("dbo.Companies", "NeededConversion");
            DropColumn("dbo.Companies", "OtherFormatOut");
            DropColumn("dbo.Companies", "OtherFormatIn");
            DropColumn("dbo.Companies", "PdfIn");
            DropColumn("dbo.Companies", "XmlIn");
            DropColumn("dbo.Companies", "NumberOfDocTypesIn");
            DropColumn("dbo.Companies", "PdfOut");
            DropColumn("dbo.Companies", "XmlOut");
            DropColumn("dbo.Companies", "NumberOfDocTypesOut");
        }
    }
}
