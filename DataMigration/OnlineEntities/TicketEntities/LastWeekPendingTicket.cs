using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TicketEntities
{
    public class LastWeekPendingTicket
    {
        public int Id { get; set; }
        [Required] public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        [Required] public int TicketReportId { get; set; }
        public TicketReport TicketReport { get; set; }
    }
}
