using System;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TicketEntities
{
    public class TicketLog : BaseEntity
    {
        public DateTime LogDate { get; set; }
        public string Log { get; set; } = string.Empty;

        // Many-to-One Relationship
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        //Được tạo bởi
        public int UserId { get; set; }
    }
}
