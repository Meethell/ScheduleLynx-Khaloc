namespace Client.Entities.ModelEntities
{
    public class ModelFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string SafeFileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string FileCode { get; set; }
        public string FileExtension { get; set; }
        public Model Model { get; set; }
        public int ModelId { get; set; }
    }
}
