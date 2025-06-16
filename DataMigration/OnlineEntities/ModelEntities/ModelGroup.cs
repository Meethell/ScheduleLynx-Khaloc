using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class ModelGroup
    {
        public int Id { get; set; }
        public Model ParentModel { get; set; }
        [Required] public int ParentModelId { get; set; }
        public Model ChildModel { get; set; }
        [Required] public int ChildModelId { get; set; }
    }
}
