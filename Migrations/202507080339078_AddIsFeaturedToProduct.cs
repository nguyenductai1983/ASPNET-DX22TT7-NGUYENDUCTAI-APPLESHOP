namespace AppleShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsFeaturedToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsFeatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsFeatured");
        }
    }
}
