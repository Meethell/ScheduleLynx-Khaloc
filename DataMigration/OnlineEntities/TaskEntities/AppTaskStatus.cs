using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TaskEntities
{
    public class AppTaskStatus : BaseEntity
    {
        public string Color { get; set; } = string.Empty;

        // One-to-Many Relationship
        public List<AppTask> AppTasks { get; set; }
    }
}
