using System.Collections.Generic;

namespace BaseLibrary.Entities.CustomerEntities
{
    public class Dealer : BaseEntity
    {
        public string Address { get; set; } = string.Empty;

        // One-to-Many Relationship
        public List<Customer> Customers { get; set; }

    }
}
