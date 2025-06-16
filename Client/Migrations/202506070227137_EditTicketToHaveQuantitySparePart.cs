namespace Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTicketToHaveQuantitySparePart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketSpareParts", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketSpareParts", "Quantity");
        }
    }
}
