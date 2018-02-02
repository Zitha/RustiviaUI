namespace IntroductionMVC5.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArsloTrading : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArsloCustomers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        Address = c.String(),
                        TellNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArsloProfomas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfomaNumber = c.String(nullable: false),
                        UCRNumber = c.String(),
                        Date = c.DateTime(nullable: false),
                        Status = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArsloCustomers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.ArsloInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        Reference = c.String(),
                        InvoiceLocation = c.String(),
                        PointOfLoading = c.String(),
                        PointOfDelivery = c.String(),
                        User_Id = c.Int(),
                        ArsloProfoma_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArsloCustomers", t => t.User_Id)
                .ForeignKey("dbo.ArsloProfomas", t => t.ArsloProfoma_Id)
                .Index(t => t.User_Id)
                .Index(t => t.ArsloProfoma_Id);
            
            CreateTable(
                "dbo.ArsloInvoiceItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ArsloInvoice_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArsloInvoices", t => t.ArsloInvoice_Id)
                .Index(t => t.ArsloInvoice_Id);

            

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArsloInvoices", "ArsloProfoma_Id", "dbo.ArsloProfomas");
            DropForeignKey("dbo.ArsloInvoices", "User_Id", "dbo.ArsloCustomers");
            DropForeignKey("dbo.ArsloInvoiceItems", "ArsloInvoice_Id", "dbo.ArsloInvoices");
            DropForeignKey("dbo.ArsloProfomas", "Customer_Id", "dbo.ArsloCustomers");
            DropIndex("dbo.ArsloInvoiceItems", new[] { "ArsloInvoice_Id" });
            DropIndex("dbo.ArsloInvoices", new[] { "ArsloProfoma_Id" });
            DropIndex("dbo.ArsloInvoices", new[] { "User_Id" });
            DropIndex("dbo.ArsloProfomas", new[] { "Customer_Id" });
            DropTable("dbo.ArsloInvoiceItems");
            DropTable("dbo.ArsloInvoices");
            DropTable("dbo.ArsloProfomas");
            DropTable("dbo.ArsloCustomers");

        }
    }
}
