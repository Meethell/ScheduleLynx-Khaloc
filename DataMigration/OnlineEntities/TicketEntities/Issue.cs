using BaseLibrary.Entities.ModelEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BaseLibrary.Entities.TicketEntities
{
    public class Issue : BaseEntity
    {
        public string IssueDescription { get; set; } = string.Empty;
        public string IssueSolution { get; set; } = string.Empty;
        public string IssueCause { get; set; } = string.Empty;

        // Many-to-One Relationship
        public Model Model { get; set; }
        [Required] public int ModelId { get; set; }

        // One-to-Many Relationship
        public List<Ticket> Tickets { get; set; }
        
    }
}
