using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TicketEntities
{
    public class TicketType : BaseEntity
    {
        public string Color { get; set; } = string.Empty;

        // One-to-Many Relationship
        public List<Ticket> Tickets { get; set; }
    }
}
