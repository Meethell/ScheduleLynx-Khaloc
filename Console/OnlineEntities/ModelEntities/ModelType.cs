using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class ModelType : BaseEntity
    {
        [Required] public bool IsConsumable { get; set; }

        // One-to-Many Relationship
        public List<Model> Models { get; set; }
    }
}
