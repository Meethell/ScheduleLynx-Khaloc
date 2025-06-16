namespace Console.Entities.TicketEntities
{
    public class LastWeekTicket
    {
        public string Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketReportId { get; set; }
        public TicketReport TicketReport { get; set; }
    }
}
