namespace Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixExpense : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Expenses", "Count", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Expenses", "Total");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Expenses", "Count", c => c.Int(nullable: false));
        }
    }
}
