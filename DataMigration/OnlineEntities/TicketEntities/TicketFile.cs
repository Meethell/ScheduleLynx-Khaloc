using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TicketEntities
{
    public class TicketFile : BaseEntity
    {
        public string SafeFileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        // Many-to-One Relationship
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
