using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TaskEntities
{
    public class TaskFile : BaseEntity
    {
        public string SafeFileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        // Many-to-One Relationship
        [Required] public int TaskId { get; set; }
        public AppTask Task { get; set; }

        // Được tạo bởi
        [Required] public int UserId { get; set; }
    }
}
