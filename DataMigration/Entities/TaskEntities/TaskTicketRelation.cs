using DataMigration.Entities.TicketEntities;

namespace DataMigration.Entities.TaskEntities
{
    public class TaskTicketRelation
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public AppTask Task { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
