namespace Client.Entities.ModelEntities
{
    public class ModelSpecification_File
    {
        public int Id { get; set; }
        public int ModelSpecificationId { get; set; }
        public ModelSpecification ModelSpecification { get; set; }
        public int ModelFileId { get; set; }
        public ModelFile ModelFile { get; set; }
    }
}
