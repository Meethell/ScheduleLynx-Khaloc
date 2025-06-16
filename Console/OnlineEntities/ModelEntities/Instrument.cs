using BaseLibrary.Entities.CustomerEntities;
using BaseLibrary.Entities.TicketEntities;
using BaseLibrary.Entities.UserEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.ModelEntities
{
    public class Instrument : BaseEntity
    {
        public DateTime InstallDate { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime LastPmDate { get; set; }
        public DateTime NextPmDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        // Many-to-One Relationship
        public Model Model { get; set; }
        [Required] public int ModelId { get; set; }
        public InstrumentStatus Status { get; set; }
        [Required] public int StatusId { get; set; }
        public Customer Customer { get; set; }
        [Required] public int CustomerId { get; set; }

        // One-to-Many Relationship
        public List<InstrumentLog> InstrumentLogs { get; set; }
        public List<Ticket> Tickets { get; set; }

        // Được tạo bởi
        [Required] public int UserId { get; set; }
    }
}
