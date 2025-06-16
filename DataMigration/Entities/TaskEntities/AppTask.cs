using System;


namespace DataMigration.Entities.TaskEntities
{
    public class AppTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateDue { get; set; }
        public bool Overdue { get; set; }
        public AppTaskStatus Status { get; set; }
        public int TaskStatusId { get; set; }
        public TaskPriority Priority { get; set; }
        public int PriorityId { get; set; }
        public TaskType Type { get; set; }
        public int TaskTypeId { get; set; }
        public int UserId { get; set; }
    }
}
