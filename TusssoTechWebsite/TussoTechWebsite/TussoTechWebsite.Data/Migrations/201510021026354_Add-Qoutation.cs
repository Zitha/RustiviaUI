namespace TussoTechWebsite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQoutation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Qoutations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateSent = c.DateTime(nullable: false),
                        QoutationNumber = c.String(),
                        Total = c.Double(nullable: false),
                        Description = c.String(),
                        Location = c.String(),
                        CustomerName = c.String(),
                        Address = c.String(),
                        Customer_Id = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Items", "Qoutation_Id", c => c.Int());
            CreateIndex("dbo.Items", "Qoutation_Id");
            AddForeignKey("dbo.Items", "Qoutation_Id", "dbo.Qoutations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Qoutation_Id", "dbo.Qoutations");
            DropIndex("dbo.Items", new[] { "Qoutation_Id" });
            DropColumn("dbo.Items", "Qoutation_Id");
            DropTable("dbo.Qoutations");
        }
    }
}
