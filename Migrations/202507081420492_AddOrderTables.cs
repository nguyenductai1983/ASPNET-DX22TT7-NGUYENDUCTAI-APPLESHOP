namespace AppleShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        OrderDate = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShipName = c.String(nullable: false),
                        ShipAddress = c.String(nullable: false),
                        ShipPhoneNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
        }
    }
}
