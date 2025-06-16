using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TaskEntities
{
    public class TaskTag
    {
        public int Id { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public int TaskId { get; set; }
    }
}
