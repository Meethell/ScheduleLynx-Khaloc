using System;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class InstrumentLog : BaseEntity
    {
        public DateTime LogDate { get; set; }
        public string Log { get; set; } = string.Empty;

        // Many-to-One Relationship
        public Instrument Instrument { get; set; }
        public int InstrumentId { get; set; }

        // Được tạo bởi
        [Required] public int UserId { get; set; }
    }
}
