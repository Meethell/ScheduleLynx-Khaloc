using System;

namespace Client.Entities.TicketEntities
{
    public class TicketReport
    {
        public int Id { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
