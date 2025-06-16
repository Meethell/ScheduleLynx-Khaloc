using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class ModelSpecificationGroup : BaseEntity
    {
        [Required] public bool IsBase { get; set; }

        // Many-to-One Relationship
        public Model Model { get; set; }
        [Required] public int ModelId { get; set; }

        // One-to-Many Relationship
        public List<ModelSpecification> ModelSpecifications { get; set; }
    }
}
