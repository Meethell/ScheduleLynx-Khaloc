namespace Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExpense : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        TicketId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketServiceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tickets", "TicketServiceTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "TicketServiceTypeId");
            AddForeignKey("dbo.Tickets", "TicketServiceTypeId", "dbo.TicketServiceTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TicketServiceTypeId", "dbo.TicketServiceTypes");
            DropIndex("dbo.Tickets", new[] { "TicketServiceTypeId" });
            DropColumn("dbo.Tickets", "TicketServiceTypeId");
            DropTable("dbo.TicketServiceTypes");
            DropTable("dbo.Expenses");
        }
    }
}
