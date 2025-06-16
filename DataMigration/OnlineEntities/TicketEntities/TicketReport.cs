using System;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TicketEntities
{
    public class TicketReport : BaseEntity
    {
        public int Week { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Report { get; set; } = string.Empty;
    }
}
