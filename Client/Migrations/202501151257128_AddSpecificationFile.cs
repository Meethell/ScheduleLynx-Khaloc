namespace Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpecificationFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModelSpecification_File",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelSpecificationId = c.Int(nullable: false),
                        ModelFileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ModelFiles", t => t.ModelFileId, cascadeDelete: true)
                .ForeignKey("dbo.ModelSpecifications", t => t.ModelSpecificationId, cascadeDelete: true)
                .Index(t => t.ModelSpecificationId)
                .Index(t => t.ModelFileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModelSpecification_File", "ModelSpecificationId", "dbo.ModelSpecifications");
            DropForeignKey("dbo.ModelSpecification_File", "ModelFileId", "dbo.ModelFiles");
            DropIndex("dbo.ModelSpecification_File", new[] { "ModelFileId" });
            DropIndex("dbo.ModelSpecification_File", new[] { "ModelSpecificationId" });
            DropTable("dbo.ModelSpecification_File");
        }
    }
}
