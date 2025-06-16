using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class ModelSpecification : BaseEntity
    {
        public string QuoteLine { get; set; } = string.Empty;
        public string ReferenceLine { get; set; } = string.Empty;
        [Required] public bool IsKeySpecification { get; set; }
        public string Notes { get; set; } = string.Empty;
        
        // Many-to-One Relationship
        public ModelFile ModelFile { get; set; }
        [Required] public int ModelFileId { get; set; }
        public ModelSpecificationGroup ModelSpecificationGroup { get; set; }
        [Required] public int ModelSpecificationGroupId { get; set; }

    }
}
