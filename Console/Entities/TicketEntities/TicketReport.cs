using System;

namespace Console.Entities.TicketEntities
{
    public class TicketReport
    {
        public string Id { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
