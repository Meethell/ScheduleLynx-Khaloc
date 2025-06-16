using BaseLibrary.Entities.TicketEntities;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TaskEntities
{
    public class TaskTicketRelation
    {
        public int Id { get; set; }
        [Required] public int TaskId { get; set; }
        public AppTask Task { get; set; }

        [Required] public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
