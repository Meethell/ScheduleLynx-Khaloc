using System;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TaskEntities
{
    public class TaskLog : BaseEntity
    {
        public DateTime LogDate { get; set; }
        public string Log { get; set; } = string.Empty;


        // Many-to-One Relationship
        [Required] public int TaskId { get; set; }
        public AppTask AppTask { get; set; }

        // Được tạo bởi
        [Required] public int UserId { get; set; }
    }
}
