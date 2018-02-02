namespace IntroductionMVC5.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfomaValidation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ArsloProfomas", "Customer_Id", "dbo.ArsloCustomers");
            DropIndex("dbo.ArsloProfomas", new[] { "Customer_Id" });
            AlterColumn("dbo.ArsloProfomas", "Customer_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.ArsloProfomas", "Customer_Id");
            AddForeignKey("dbo.ArsloProfomas", "Customer_Id", "dbo.ArsloCustomers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArsloProfomas", "Customer_Id", "dbo.ArsloCustomers");
            DropIndex("dbo.ArsloProfomas", new[] { "Customer_Id" });
            AlterColumn("dbo.ArsloProfomas", "Customer_Id", c => c.Int());
            CreateIndex("dbo.ArsloProfomas", "Customer_Id");
            AddForeignKey("dbo.ArsloProfomas", "Customer_Id", "dbo.ArsloCustomers", "Id");
        }
    }
}
