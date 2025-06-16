using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TicketEntities
{
    public class TicketTag
    {
        public int Id { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public int TicketId { get; set; }
    }
}
