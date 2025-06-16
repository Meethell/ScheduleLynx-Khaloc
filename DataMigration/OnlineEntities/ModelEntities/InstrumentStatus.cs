using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class InstrumentStatus : BaseEntity
    {
        public string Color { get; set; } = string.Empty;

        // One-to-Many Relationship
        public List<Instrument> Instruments { get; set; }
    }
}
