namespace IntroductionMVC5.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfomaToInvoice : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ArsloInvoices", name: "ArsloProfoma_Id", newName: "Profoma_Id");
            RenameIndex(table: "dbo.ArsloInvoices", name: "IX_ArsloProfoma_Id", newName: "IX_Profoma_Id");
            AddColumn("dbo.ArsloInvoiceItems", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArsloInvoiceItems", "TotalPrice");
            RenameIndex(table: "dbo.ArsloInvoices", name: "IX_Profoma_Id", newName: "IX_ArsloProfoma_Id");
            RenameColumn(table: "dbo.ArsloInvoices", name: "Profoma_Id", newName: "ArsloProfoma_Id");
        }
    }
}
