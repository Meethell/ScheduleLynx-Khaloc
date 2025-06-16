

namespace DataMigration.Entities.ModelEntities
{
    public class Model { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string Descriptions { get; set; }
       
        public ModelType ModelType { get; set;}
        public int ModelTypeId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int ManufacturerId { get; set; }
        // Gắn file ảnh vào Model
        public string ImagePath { get; set; }
        public byte[] Image { get; set; }
        // Có là VTTH không
        public bool IsConsumable { get; set; }
        public string Notes { get; set; }

        
    }
}
