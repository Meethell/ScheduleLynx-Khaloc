using System;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TicketEntities
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public int TicketId { get; set; }
    }
}
