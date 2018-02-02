namespace IntroductionMVC5.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateArsloCustomer : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ArsloInvoices", name: "User_Id", newName: "Customer_Id");
            RenameIndex(table: "dbo.ArsloInvoices", name: "IX_User_Id", newName: "IX_Customer_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ArsloInvoices", name: "IX_Customer_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.ArsloInvoices", name: "Customer_Id", newName: "User_Id");
        }
    }
}
