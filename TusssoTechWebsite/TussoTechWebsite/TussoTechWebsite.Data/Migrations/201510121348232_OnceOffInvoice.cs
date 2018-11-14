namespace TussoTechWebsite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnceOffInvoice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnceOffInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateSent = c.DateTime(nullable: false),
                        InvoiceNumber = c.String(),
                        Total = c.Double(nullable: false),
                        Description = c.String(),
                        Location = c.String(),
                        CustomerName = c.String(),
                        Status = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Items", "OnceOffInvoice_Id", c => c.Int());
            CreateIndex("dbo.Items", "OnceOffInvoice_Id");
            AddForeignKey("dbo.Items", "OnceOffInvoice_Id", "dbo.OnceOffInvoices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "OnceOffInvoice_Id", "dbo.OnceOffInvoices");
            DropIndex("dbo.Items", new[] { "OnceOffInvoice_Id" });
            DropColumn("dbo.Items", "OnceOffInvoice_Id");
            DropTable("dbo.OnceOffInvoices");
        }
    }
}
