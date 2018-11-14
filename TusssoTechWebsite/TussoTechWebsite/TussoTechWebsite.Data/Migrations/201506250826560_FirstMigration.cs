namespace TussoTechWebsite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankStatements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateSent = c.DateTime(nullable: false),
                        AccountAmount = c.Double(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Contacts = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Location = c.String(),
                        Type = c.String(),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateSent = c.DateTime(nullable: false),
                        Type = c.String(),
                        PurchaseNumber = c.String(),
                        Total = c.Double(nullable: false),
                        Description = c.String(),
                        Location = c.String(),
                        Employee = c.String(),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Type = c.String(),
                        OutCome = c.String(),
                        Location = c.String(),
                        Customer_Id = c.Int(),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Contact = c.String(),
                        EmailAddress = c.String(),
                        VatNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateSent = c.DateTime(nullable: false),
                        InvoiceNumber = c.String(),
                        Total = c.Double(nullable: false),
                        Status = c.String(),
                        Description = c.String(),
                        Location = c.String(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resources", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.Resources", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Expenses", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.CompanyDocuments", "Company_Id", "dbo.Companies");
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            DropIndex("dbo.Resources", new[] { "Company_Id" });
            DropIndex("dbo.Resources", new[] { "Customer_Id" });
            DropIndex("dbo.Expenses", new[] { "Company_Id" });
            DropIndex("dbo.CompanyDocuments", new[] { "Company_Id" });
            DropTable("dbo.Invoices");
            DropTable("dbo.Customers");
            DropTable("dbo.Resources");
            DropTable("dbo.Expenses");
            DropTable("dbo.CompanyDocuments");
            DropTable("dbo.Companies");
            DropTable("dbo.BankStatements");
        }
    }
}
