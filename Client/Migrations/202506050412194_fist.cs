namespace Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        Role = c.String(),
                        Notes = c.String(),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Notes = c.String(),
                        DealerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dealers", t => t.DealerId, cascadeDelete: true)
                .Index(t => t.DealerId);
            
            CreateTable(
                "dbo.Dealers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InstrumentLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        LogDate = c.DateTime(nullable: false),
                        InstrumentId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instruments", t => t.InstrumentId, cascadeDelete: true)
                .Index(t => t.InstrumentId);
            
            CreateTable(
                "dbo.Instruments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        InstallDate = c.DateTime(nullable: false),
                        SerialNumber = c.String(),
                        LastPmDate = c.DateTime(nullable: false),
                        NextPmDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Location = c.String(),
                        ModelId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Models", t => t.ModelId, cascadeDelete: true)
                .ForeignKey("dbo.InstrumentStatus", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.ModelId)
                .Index(t => t.StatusId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Models",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TradeName = c.String(),
                        Descriptions = c.String(),
                        ModelTypeId = c.Int(nullable: false),
                        ManufacturerId = c.Int(nullable: false),
                        ImagePath = c.String(),
                        Image = c.Binary(),
                        IsConsumable = c.Boolean(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Manufacturers", t => t.ManufacturerId, cascadeDelete: true)
                .ForeignKey("dbo.ModelTypes", t => t.ModelTypeId, cascadeDelete: true)
                .Index(t => t.ModelTypeId)
                .Index(t => t.ManufacturerId);
            
            CreateTable(
                "dbo.Manufacturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CountryId = c.Int(nullable: false),
                        Descriptions = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.ModelTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        IsConsumable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InstrumentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Issues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ModelId = c.Int(nullable: false),
                        IssueDescription = c.String(),
                        IssueSolution = c.String(),
                        IssueCause = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Models", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.LastWeekPendingTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(nullable: false),
                        TicketReportId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .ForeignKey("dbo.TicketReports", t => t.TicketReportId, cascadeDelete: true)
                .Index(t => t.TicketId)
                .Index(t => t.TicketReportId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketNumber = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Solution = c.String(),
                        Cause = c.String(),
                        Conclusion = c.String(),
                        UserId = c.Int(nullable: false),
                        TicketTypeId = c.Int(nullable: false),
                        PriorityId = c.Int(nullable: false),
                        TicketStatusId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        DateDue = c.DateTime(nullable: false),
                        InstrumentId = c.Int(nullable: false),
                        IssueId = c.Int(nullable: false),
                        Overdue = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instruments", t => t.InstrumentId, cascadeDelete: true)
                .ForeignKey("dbo.Issues", t => t.IssueId, cascadeDelete: false)
                .ForeignKey("dbo.TicketPriorities", t => t.PriorityId, cascadeDelete: true)
                .ForeignKey("dbo.TicketStatus", t => t.TicketStatusId, cascadeDelete: true)
                .ForeignKey("dbo.TicketTypes", t => t.TicketTypeId, cascadeDelete: true)
                .Index(t => t.TicketTypeId)
                .Index(t => t.PriorityId)
                .Index(t => t.TicketStatusId)
                .Index(t => t.InstrumentId)
                .Index(t => t.IssueId);
            
            CreateTable(
                "dbo.TicketPriorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Level = c.Int(nullable: false),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Week = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LastWeekTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(nullable: false),
                        TicketReportId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .ForeignKey("dbo.TicketReports", t => t.TicketReportId, cascadeDelete: true)
                .Index(t => t.TicketId)
                .Index(t => t.TicketReportId);
            
            CreateTable(
                "dbo.ModelFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        SafeFileName = c.String(),
                        FilePath = c.String(),
                        FileType = c.String(),
                        FileCode = c.String(),
                        FileExtension = c.String(),
                        ModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Models", t => t.ModelId, cascadeDelete: false)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.ModelGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentModelId = c.Int(nullable: false),
                        ChildModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Models", t => t.ChildModelId, cascadeDelete: false)
                .ForeignKey("dbo.Models", t => t.ParentModelId, cascadeDelete: false)
                .Index(t => t.ParentModelId)
                .Index(t => t.ChildModelId);
            
            CreateTable(
                "dbo.ModelSpecificationGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModelId = c.Int(nullable: false),
                        IsBase = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Models", t => t.ModelId, cascadeDelete: false)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.ModelSpecifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Specification = c.String(),
                        QuoteLine = c.String(),
                        ReferenceLine = c.String(),
                        ModelFileId = c.Int(nullable: false),
                        ModelSpecificationGroupId = c.Int(nullable: false),
                        IsKeySpecification = c.Boolean(nullable: false),
                        ModelId = c.Int(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Models", t => t.ModelId, cascadeDelete: true)
                .ForeignKey("dbo.ModelFiles", t => t.ModelFileId, cascadeDelete: true)
                .ForeignKey("dbo.ModelSpecificationGroups", t => t.ModelSpecificationGroupId, cascadeDelete: true)
                .Index(t => t.ModelFileId)
                .Index(t => t.ModelSpecificationGroupId)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.SpareParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        QuantityInStock = c.Int(nullable: false),
                        ModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Models", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.SystemRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        SafeFileName = c.String(),
                        FileExtension = c.String(),
                        FilePath = c.String(),
                        TaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppTasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId);
            
            CreateTable(
                "dbo.AppTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        DateDue = c.DateTime(nullable: false),
                        Overdue = c.Boolean(nullable: false),
                        TaskStatusId = c.Int(nullable: false),
                        PriorityId = c.Int(nullable: false),
                        TaskTypeId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskPriorities", t => t.PriorityId, cascadeDelete: true)
                .ForeignKey("dbo.AppTaskStatus", t => t.Status_Id)
                .ForeignKey("dbo.TaskTypes", t => t.TaskTypeId, cascadeDelete: true)
                .Index(t => t.PriorityId)
                .Index(t => t.TaskTypeId)
                .Index(t => t.Status_Id);
            
            CreateTable(
                "dbo.TaskPriorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Level = c.Int(nullable: false),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppTaskStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskId = c.Int(nullable: false),
                        Log = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        AppTask_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppTasks", t => t.AppTask_Id)
                .Index(t => t.AppTask_Id);
            
            CreateTable(
                "dbo.TaskTicketRelations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskId = c.Int(nullable: false),
                        TicketId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppTasks", t => t.TaskId, cascadeDelete: true)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TaskId)
                .Index(t => t.TicketId);
            
            CreateTable(
                "dbo.TicketFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        SafeFileName = c.String(),
                        FileExtension = c.String(),
                        FilePath = c.String(),
                        TicketId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId);
            
            CreateTable(
                "dbo.TicketLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Log = c.String(),
                        TicketId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        LogDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .Index(t => t.TicketId);
            
            CreateTable(
                "dbo.TicketSpareParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(nullable: false),
                        SparePartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TicketId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                        IsActivated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLogs", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TicketFiles", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TaskTicketRelations", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.TaskTicketRelations", "TaskId", "dbo.AppTasks");
            DropForeignKey("dbo.TaskLogs", "AppTask_Id", "dbo.AppTasks");
            DropForeignKey("dbo.TaskFiles", "TaskId", "dbo.AppTasks");
            DropForeignKey("dbo.AppTasks", "TaskTypeId", "dbo.TaskTypes");
            DropForeignKey("dbo.AppTasks", "Status_Id", "dbo.AppTaskStatus");
            DropForeignKey("dbo.AppTasks", "PriorityId", "dbo.TaskPriorities");
            DropForeignKey("dbo.SpareParts", "ModelId", "dbo.Models");
            DropForeignKey("dbo.ModelSpecifications", "ModelSpecificationGroupId", "dbo.ModelSpecificationGroups");
            DropForeignKey("dbo.ModelSpecifications", "ModelFileId", "dbo.ModelFiles");
            DropForeignKey("dbo.ModelSpecifications", "ModelId", "dbo.Models");
            DropForeignKey("dbo.ModelSpecificationGroups", "ModelId", "dbo.Models");
            DropForeignKey("dbo.ModelGroups", "ParentModelId", "dbo.Models");
            DropForeignKey("dbo.ModelGroups", "ChildModelId", "dbo.Models");
            DropForeignKey("dbo.ModelFiles", "ModelId", "dbo.Models");
            DropForeignKey("dbo.LastWeekTickets", "TicketReportId", "dbo.TicketReports");
            DropForeignKey("dbo.LastWeekTickets", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.LastWeekPendingTickets", "TicketReportId", "dbo.TicketReports");
            DropForeignKey("dbo.LastWeekPendingTickets", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "TicketTypeId", "dbo.TicketTypes");
            DropForeignKey("dbo.Tickets", "TicketStatusId", "dbo.TicketStatus");
            DropForeignKey("dbo.Tickets", "PriorityId", "dbo.TicketPriorities");
            DropForeignKey("dbo.Tickets", "IssueId", "dbo.Issues");
            DropForeignKey("dbo.Tickets", "InstrumentId", "dbo.Instruments");
            DropForeignKey("dbo.Issues", "ModelId", "dbo.Models");
            DropForeignKey("dbo.InstrumentLogs", "InstrumentId", "dbo.Instruments");
            DropForeignKey("dbo.Instruments", "StatusId", "dbo.InstrumentStatus");
            DropForeignKey("dbo.Instruments", "ModelId", "dbo.Models");
            DropForeignKey("dbo.Models", "ModelTypeId", "dbo.ModelTypes");
            DropForeignKey("dbo.Models", "ManufacturerId", "dbo.Manufacturers");
            DropForeignKey("dbo.Manufacturers", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Instruments", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Contacts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "DealerId", "dbo.Dealers");
            DropIndex("dbo.TicketLogs", new[] { "TicketId" });
            DropIndex("dbo.TicketFiles", new[] { "TicketId" });
            DropIndex("dbo.TaskTicketRelations", new[] { "TicketId" });
            DropIndex("dbo.TaskTicketRelations", new[] { "TaskId" });
            DropIndex("dbo.TaskLogs", new[] { "AppTask_Id" });
            DropIndex("dbo.AppTasks", new[] { "Status_Id" });
            DropIndex("dbo.AppTasks", new[] { "TaskTypeId" });
            DropIndex("dbo.AppTasks", new[] { "PriorityId" });
            DropIndex("dbo.TaskFiles", new[] { "TaskId" });
            DropIndex("dbo.SpareParts", new[] { "ModelId" });
            DropIndex("dbo.ModelSpecifications", new[] { "ModelId" });
            DropIndex("dbo.ModelSpecifications", new[] { "ModelSpecificationGroupId" });
            DropIndex("dbo.ModelSpecifications", new[] { "ModelFileId" });
            DropIndex("dbo.ModelSpecificationGroups", new[] { "ModelId" });
            DropIndex("dbo.ModelGroups", new[] { "ChildModelId" });
            DropIndex("dbo.ModelGroups", new[] { "ParentModelId" });
            DropIndex("dbo.ModelFiles", new[] { "ModelId" });
            DropIndex("dbo.LastWeekTickets", new[] { "TicketReportId" });
            DropIndex("dbo.LastWeekTickets", new[] { "TicketId" });
            DropIndex("dbo.Tickets", new[] { "IssueId" });
            DropIndex("dbo.Tickets", new[] { "InstrumentId" });
            DropIndex("dbo.Tickets", new[] { "TicketStatusId" });
            DropIndex("dbo.Tickets", new[] { "PriorityId" });
            DropIndex("dbo.Tickets", new[] { "TicketTypeId" });
            DropIndex("dbo.LastWeekPendingTickets", new[] { "TicketReportId" });
            DropIndex("dbo.LastWeekPendingTickets", new[] { "TicketId" });
            DropIndex("dbo.Issues", new[] { "ModelId" });
            DropIndex("dbo.Manufacturers", new[] { "CountryId" });
            DropIndex("dbo.Models", new[] { "ManufacturerId" });
            DropIndex("dbo.Models", new[] { "ModelTypeId" });
            DropIndex("dbo.Instruments", new[] { "CustomerId" });
            DropIndex("dbo.Instruments", new[] { "StatusId" });
            DropIndex("dbo.Instruments", new[] { "ModelId" });
            DropIndex("dbo.InstrumentLogs", new[] { "InstrumentId" });
            DropIndex("dbo.Customers", new[] { "DealerId" });
            DropIndex("dbo.Contacts", new[] { "CustomerId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.TicketTags");
            DropTable("dbo.TicketSpareParts");
            DropTable("dbo.TicketLogs");
            DropTable("dbo.TicketFiles");
            DropTable("dbo.TaskTicketRelations");
            DropTable("dbo.TaskLogs");
            DropTable("dbo.TaskTypes");
            DropTable("dbo.AppTaskStatus");
            DropTable("dbo.TaskPriorities");
            DropTable("dbo.AppTasks");
            DropTable("dbo.TaskFiles");
            DropTable("dbo.SystemRoles");
            DropTable("dbo.SpareParts");
            DropTable("dbo.ModelSpecifications");
            DropTable("dbo.ModelSpecificationGroups");
            DropTable("dbo.ModelGroups");
            DropTable("dbo.ModelFiles");
            DropTable("dbo.LastWeekTickets");
            DropTable("dbo.TicketReports");
            DropTable("dbo.TicketTypes");
            DropTable("dbo.TicketStatus");
            DropTable("dbo.TicketPriorities");
            DropTable("dbo.Tickets");
            DropTable("dbo.LastWeekPendingTickets");
            DropTable("dbo.Issues");
            DropTable("dbo.InstrumentStatus");
            DropTable("dbo.ModelTypes");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Models");
            DropTable("dbo.Instruments");
            DropTable("dbo.InstrumentLogs");
            DropTable("dbo.Countries");
            DropTable("dbo.Dealers");
            DropTable("dbo.Customers");
            DropTable("dbo.Contacts");
        }
    }
}
