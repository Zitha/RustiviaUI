namespace IntroductionMVC5.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArsloProfomas", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArsloProfomas", "Location");
        }
    }
}
