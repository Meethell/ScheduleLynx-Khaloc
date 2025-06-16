namespace Client.Entities.TaskEntities
{
    public class Task_File
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public AppTask Task { get; set; }
        public int FileId { get; set; }
        public TaskFile File { get; set; }
    }
}
