using BaseLibrary.Entities.ModelEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.CustomerEntities
{
    public class Customer : BaseEntity
    {
        public string Address { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        // Many-to-One Relationship
        public Dealer Dealer { get; set; }
        [Required] public int DealerId { get; set; }

        // One-to-Many Relationship
        public List<Contact> Contacts { get; set; }
        public List<Instrument> Instruments { get; set; }
    }
}
