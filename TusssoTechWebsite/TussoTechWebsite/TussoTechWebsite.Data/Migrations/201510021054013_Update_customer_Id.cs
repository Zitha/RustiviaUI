namespace TussoTechWebsite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_customer_Id : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Qoutations", "Customer_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Qoutations", "Customer_Id", c => c.String());
        }
    }
}
