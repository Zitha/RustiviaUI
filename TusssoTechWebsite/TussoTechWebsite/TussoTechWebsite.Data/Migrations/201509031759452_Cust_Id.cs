namespace TussoTechWebsite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cust_Id : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            AddColumn("dbo.Invoices", "Customer_Id1", c => c.Int());
            AlterColumn("dbo.Invoices", "Customer_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Invoices", "Customer_Id1");
            AddForeignKey("dbo.Invoices", "Customer_Id1", "dbo.Customers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "Customer_Id1", "dbo.Customers");
            DropIndex("dbo.Invoices", new[] { "Customer_Id1" });
            AlterColumn("dbo.Invoices", "Customer_Id", c => c.Int());
            DropColumn("dbo.Invoices", "Customer_Id1");
            CreateIndex("dbo.Invoices", "Customer_Id");
            AddForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
