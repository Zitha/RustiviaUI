namespace TussoTechWebsite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Invoice_Id", c => c.Int());
            CreateIndex("dbo.Items", "Invoice_Id");
            AddForeignKey("dbo.Items", "Invoice_Id", "dbo.Invoices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Invoice_Id", "dbo.Invoices");
            DropIndex("dbo.Items", new[] { "Invoice_Id" });
            DropColumn("dbo.Items", "Invoice_Id");
        }
    }
}
