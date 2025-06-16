namespace Client.Entities.TicketEntities
{
    public class LastWeekTicket
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketReportId { get; set; }
        public TicketReport TicketReport { get; set; }
    }
}
