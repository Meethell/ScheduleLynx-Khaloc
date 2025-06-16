using System;

namespace Client.Entities.ModelEntities
{
    public class InstrumentLog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime LogDate { get; set; }
        public Instrument Instrument { get; set; }
        public int InstrumentId { get; set; }
        public int UserId { get; set; }
    }
}
