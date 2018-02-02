namespace IntroductionMVC5.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArsloItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArsloProfomaItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ArsloProfoma_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArsloProfomas", t => t.ArsloProfoma_Id)
                .Index(t => t.ArsloProfoma_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArsloProfomaItems", "ArsloProfoma_Id", "dbo.ArsloProfomas");
            DropIndex("dbo.ArsloProfomaItems", new[] { "ArsloProfoma_Id" });
            DropTable("dbo.ArsloProfomaItems");
        }
    }
}
