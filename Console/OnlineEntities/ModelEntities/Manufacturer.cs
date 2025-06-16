
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class Manufacturer : BaseEntity
    {
        public string Descriptions { get; set; } = string.Empty;


        // Many-to-One Relationship
        public Country Country { get; set;}
        [Required] public int CountryId { get; set; }
        

        // One-to-Many Relationship
        public List<Model> Models { get; set; }

    }
}
