using System;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TaskEntities
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public int TaskId { get; set; }
    }
}
