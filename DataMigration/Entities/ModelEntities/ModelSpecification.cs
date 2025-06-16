namespace DataMigration.Entities.ModelEntities
{
    public class ModelSpecification
    {
        public int Id { get; set; }
        public string Specification { get; set; }
        public string QuoteLine { get; set; }
        public string ReferenceLine { get; set; }
        public ModelFile ModelFile { get; set; }
        public int ModelFileId { get; set; }
    
        public ModelSpecificationGroup ModelSpecificationGroup { get; set; }
        public int ModelSpecificationGroupId { get; set; }
        public bool IsKeySpecification { get; set; }
        public Model Model { get; set; }
        public int ModelId { get; set; }
        public string Notes { get; set; }
    }
}
