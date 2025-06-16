using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TaskEntities
{
    public class AppTask : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateDue { get; set; }
        [Required] public bool Overdue { get; set; }

        // Many-to-One Relationship
        public AppTaskStatus Status { get; set; }
        [Required] public int TaskStatusId { get; set; }
        public TaskPriority Priority { get; set; }
        [Required] public int PriorityId { get; set; }
        public TaskType Type { get; set; }
        [Required] public int TaskTypeId { get; set; }

        // One-to-Many Relationship
        public List<TaskFile> TaskFiles { get; set; }
        public List<TaskLog> TaskLogs { get; set; }

        // Được tạo bởi User
        [Required] public int UserId { get; set; }
    }
}
