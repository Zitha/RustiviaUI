namespace TussoTechWebsite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Invoices", new[] { "Customer_Id1" });
            DropColumn("dbo.Invoices", "Customer_Id");
            RenameColumn(table: "dbo.Invoices", name: "Customer_Id1", newName: "Customer_Id");
            AlterColumn("dbo.Invoices", "Customer_Id", c => c.Int());
            CreateIndex("dbo.Invoices", "Customer_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            AlterColumn("dbo.Invoices", "Customer_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Invoices", name: "Customer_Id", newName: "Customer_Id1");
            AddColumn("dbo.Invoices", "Customer_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Invoices", "Customer_Id1");
        }
    }
}
