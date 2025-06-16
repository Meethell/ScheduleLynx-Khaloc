using System;

namespace Console.Entities.TaskEntities
{
    public class TaskLog
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public AppTask AppTask { get; set; }
        public string Log { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
    }
}
