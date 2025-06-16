using Client.Entities.ModelEntities;

namespace Client.Entities.TicketEntities
{
    public class TicketSpareParts
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int SparePartId { get; set; }
        public SpareParts SparePart { get; set; }
        public int Quantity { get; set; }
        public string SerialNumber { get; set; }

    }
}
