namespace Client.Entities.ModelEntities
{
    public class ModelGroup
    {
        public int Id { get; set; }
        public Model ParentModel { get; set; }
        public int ParentModelId { get; set; }
        public Model ChildModel { get; set; }
        public int ChildModelId { get; set; }
    }
}
