using System.Collections.Generic;

namespace BaseLibrary.Entities.ModelEntities
{
    public class Country : BaseEntity
    {
        // One-to-Many Relationship
        public List<Manufacturer> Manufacturers { get; set; }
    }
}
