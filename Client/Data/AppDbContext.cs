using Client.Entities.TicketEntities;
using Client.Entities.TaskEntities;
using Client.Entities.CustomerEntities;
using Client.Entities.ModelEntities;
using Client.Entities.UserEntity;
using System.Data.Entity;
using Client.Entities.ExpenseEntities;

namespace Client.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }
        // Ticket entities
        public DbSet<Issue> Issues { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketFile> TicketFiles { get; set; }
        public DbSet<TicketLog> TicketLogs { get; set; }
        public DbSet<TicketReport> TicketReports { get; set; }
        public DbSet<LastWeekPendingTicket> LastWeekPendingTickets { get; set; }
        public DbSet<LastWeekTicket> LastWeekTickets { get; set; }
        public DbSet<TicketTag> TicketTags { get; set; }
        public DbSet<TicketSpareParts> TicketSpareParts { get; set; }
        public DbSet<TicketServiceType> TicketServiceTypes { get; set; }

        // Model entities
        public DbSet<Country> Countries { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<ModelType> ModelTypes { get; set; }
        public DbSet<InstrumentStatus> InstrumentStatuses { get; set; }
        public DbSet<InstrumentLog> InstrumentLogs { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<ModelGroup> ModelGroups { get; set; }
        public DbSet<ModelSpecification> ModelSpecifications { get; set; }
        public DbSet<ModelFile> ModelFiles { get; set; }
        public DbSet<ModelSpecificationGroup> ModelSpecificationGroups { get; set; }
        public DbSet<SpareParts> SpareParts { get; set; }

        // User entities
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }

        // Customer entities
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dealer> Dealers { get; set; }

        // Task entities
        public DbSet<AppTaskStatus> TaskStatuses { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<TaskFile> TaskFiles { get; set; }
        public DbSet<AppTask> Tasks { get; set; }
        public DbSet<TaskPriority> TaskPriorities { get; set; }
        public DbSet<TaskTicketRelation> TaskTicketRelations { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }

        // Expense entities
        public DbSet<Expense> Expenses { get; set; }

    }
}
