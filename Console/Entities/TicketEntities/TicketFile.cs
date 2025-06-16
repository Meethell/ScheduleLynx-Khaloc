namespace Console.Entities.TicketEntities
{
    public class TicketFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string SafeFileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }

        // Foreign key to Ticket
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
