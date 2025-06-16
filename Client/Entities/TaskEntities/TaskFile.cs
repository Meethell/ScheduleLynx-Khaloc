namespace Client.Entities.TaskEntities
{
    public class TaskFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string SafeFileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }

        // Foreign key to Task
        public int TaskId { get; set; }
        public AppTask Task { get; set; }
    }
}
