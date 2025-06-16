using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class ModelFile : BaseEntity
    {
        public string SafeFileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string FileCode { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;

        // Many-to-One Relationship
        public Model Model { get; set; }
        [Required] public int ModelId { get; set; }

        // One-to-Many Relationship
        public List<ModelSpecification> ModelSpecifications { get; set; }

        // Được thêm bởi User
        [Required] public int UserId { get; set; }
    }
}
