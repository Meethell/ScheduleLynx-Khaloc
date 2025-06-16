using DataMigration.Entities.TicketEntities;
using DataMigration.Entities.TaskEntities;
using DataMigration.Entities.CustomerEntities;
using DataMigration.Entities.ModelEntities;
using DataMigration.Entities.UserEntity;
using System.Data.Entity;

namespace DataMigration.Data
{
    public class OfflineDbContext : DbContext
    {
        public OfflineDbContext() : base("name=OfflineConnectionString")
        {
            
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
        public DbSet<LastWeekPendingTicket> lastWeekPendingTickets { get; set; }
        public DbSet<LastWeekTicket> lastWeekTickets { get; set; }

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
       
    }
}
