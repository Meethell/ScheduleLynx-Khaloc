
namespace Console.Entities.ModelEntities
{
    public class ModelSpecificationGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModelId { get; set; }
        public bool IsBase { get; set; }
        public Model Model { get; set; }
    }
}
