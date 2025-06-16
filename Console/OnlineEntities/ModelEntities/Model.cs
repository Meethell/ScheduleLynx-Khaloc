using BaseLibrary.Entities.TicketEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class Model : BaseEntity
    {
        public string TradeName { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;

        // Có là VTTH không
        [Required] public bool IsConsumable { get; set; }
        public string Notes { get; set; } = string.Empty;

        // Gắn file ảnh vào Model
        public byte[] Image { get; set; }

        // Many-to-One Relationship
        public ModelType ModelType { get; set; }
        [Required] public int ModelTypeId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        [Required] public int ManufacturerId { get; set; }
        
        // One-to-Many Relationship
        public List<Instrument> Instruments { get; set; }
        public List<ModelFile> ModelFiles { get; set; }
        public List<ModelSpecificationGroup> ModelSpecificationGroups { get; set; }
        public List<Issue> Issues { get; set; }
        


    }
}
