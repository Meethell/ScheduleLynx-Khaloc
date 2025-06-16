using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TicketEntities
{
    public class TicketPriority : BaseEntity
    {
        public int Level { get; set; }
        public string Color { get; set; } = string.Empty;

        // One-to-Many Relationship
        public List<Ticket> Tickets { get; set; }
    }
}
