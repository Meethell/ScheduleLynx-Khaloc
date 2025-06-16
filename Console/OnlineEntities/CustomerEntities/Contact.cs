using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.CustomerEntities
{
    public class Contact : BaseEntity
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        // Many-to-One Relationship
        public Customer Customer { get; set; }
        [Required] public int CustomerId { get; set; }
    }
}
