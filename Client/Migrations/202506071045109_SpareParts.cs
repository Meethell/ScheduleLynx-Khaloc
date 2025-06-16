namespace Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpareParts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SpareParts", "ModelId", "dbo.Models");
            DropIndex("dbo.SpareParts", new[] { "ModelId" });
            AddColumn("dbo.SpareParts", "PartNumber", c => c.String());
            AddColumn("dbo.SpareParts", "ManufacturerId", c => c.Int(nullable: false));
            AddColumn("dbo.TicketSpareParts", "SerialNumber", c => c.String());
            CreateIndex("dbo.SpareParts", "ManufacturerId");
            CreateIndex("dbo.TicketSpareParts", "SparePartId");
            AddForeignKey("dbo.SpareParts", "ManufacturerId", "dbo.Manufacturers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TicketSpareParts", "SparePartId", "dbo.SpareParts", "Id", cascadeDelete: true);
            DropColumn("dbo.SpareParts", "ModelId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpareParts", "ModelId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TicketSpareParts", "SparePartId", "dbo.SpareParts");
            DropForeignKey("dbo.SpareParts", "ManufacturerId", "dbo.Manufacturers");
            DropIndex("dbo.TicketSpareParts", new[] { "SparePartId" });
            DropIndex("dbo.SpareParts", new[] { "ManufacturerId" });
            DropColumn("dbo.TicketSpareParts", "SerialNumber");
            DropColumn("dbo.SpareParts", "ManufacturerId");
            DropColumn("dbo.SpareParts", "PartNumber");
            CreateIndex("dbo.SpareParts", "ModelId");
            AddForeignKey("dbo.SpareParts", "ModelId", "dbo.Models", "Id", cascadeDelete: true);
        }
    }
}
