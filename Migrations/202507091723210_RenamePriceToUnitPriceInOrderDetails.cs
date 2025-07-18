namespace AppleShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamePriceToUnitPriceInOrderDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.OrderDetails", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.OrderDetails", "UnitPrice");
        }
    }
}
