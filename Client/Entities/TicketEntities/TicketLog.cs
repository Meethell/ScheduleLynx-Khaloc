using System;

namespace Client.Entities.TicketEntities
{
    public class TicketLog
    {
        public int Id { get; set; }
        public string Log { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int UserId { get; set; }
        public DateTime LogDate { get; set; }
    }
}
