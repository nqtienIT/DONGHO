namespace CuaHangDongHo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Slug = c.String(nullable: false),
                    ParentId = c.Int(nullable: false),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Contacts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FullName = c.String(nullable: false, maxLength: 255),
                    Email = c.String(nullable: false, maxLength: 255),
                    Phone = c.String(nullable: false, maxLength: 15),
                    Title = c.String(nullable: false, maxLength: 255),
                    Detail = c.String(nullable: false, unicode: false, storeType: "text"),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Menus",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                    Link = c.String(nullable: false, maxLength: 255),
                    ParentId = c.Int(nullable: false),
                    Type = c.String(maxLength: 255),
                    TableId = c.Int(nullable: false),
                    Orders = c.Int(nullable: false),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.OrderDetails",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    OrderId = c.Int(nullable: false),
                    ProductId = c.Int(nullable: false),
                    Price = c.Double(nullable: false),
                    Quantity = c.Int(nullable: false),
                    Amount = c.Double(nullable: false),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Orders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CustomerId = c.Int(nullable: false),
                    CreateDate = c.DateTime(nullable: false, storeType: "date"),
                    ExportDate = c.DateTime(nullable: false, storeType: "date"),
                    DeliveryAddress = c.String(maxLength: 255),
                    DeliveryName = c.String(maxLength: 255),
                    Status = c.Int(),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Posts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CateId = c.Int(nullable: false),
                    Title = c.String(nullable: false, maxLength: 255),
                    Slug = c.String(nullable: false, maxLength: 255),
                    Detail = c.String(nullable: false),
                    Img = c.String(nullable: false, maxLength: 255),
                    Metakey = c.String(nullable: false, maxLength: 255),
                    Metadesc = c.String(nullable: false, maxLength: 255),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Products",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CateId = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 255),
                    Slug = c.String(nullable: false, maxLength: 255),
                    Img = c.String(nullable: false, maxLength: 255),
                    Detail = c.String(nullable: false),
                    Number = c.Int(nullable: false),
                    Price = c.Double(nullable: false),
                    PriceSale = c.Double(),
                    Metakey = c.String(maxLength: 255),
                    Metadesc = c.String(maxLength: 255),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Sliders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                    Link = c.String(nullable: false, maxLength: 255),
                    Position = c.String(nullable: false, maxLength: 255),
                    Img = c.String(nullable: false, maxLength: 255),
                    Orders = c.Int(nullable: false),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Topics",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                    Slug = c.String(nullable: false, maxLength: 255),
                    ParentId = c.Int(),
                    Orders = c.Int(),
                    Metakey = c.String(maxLength: 255),
                    Metadesc = c.String(maxLength: 255),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FullName = c.String(nullable: false, maxLength: 255),
                    UserName = c.String(nullable: false, maxLength: 255),
                    Password = c.String(nullable: false, maxLength: 255),
                    Email = c.String(nullable: false, maxLength: 255),
                    Gender = c.Int(nullable: false),
                    Phone = c.String(nullable: false, maxLength: 255),
                    Img = c.String(maxLength: 255),
                    Access = c.Int(nullable: false),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Brands",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 255),
                    Img = c.String(nullable: false, maxLength: 255),
                    Description = c.String(nullable: true, maxLength: 255),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Pictures",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ProductId = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 255),
                    Created_at = c.DateTime(),
                    Updated_at = c.DateTime(),
                    Status = c.Int(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Topics");
            DropTable("dbo.Sliders");
            DropTable("dbo.Products");
            DropTable("dbo.Posts");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Menus");
            DropTable("dbo.Contacts");
            DropTable("dbo.Categories");
        }
    }
}
