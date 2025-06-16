using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TaskEntities
{
    public class TaskRelation
    {
        public int Id { get; set; }
        [Required] public int ParentTaskId { get; set; }
        public AppTask ParentTask { get; set; }
        [Required] public int ChildTaskId { get; set; }
        public AppTask ChildTask { get; set; }
    }
}
