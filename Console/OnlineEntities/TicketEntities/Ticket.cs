using BaseLibrary.Entities.ModelEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities.TicketEntities
{
    public class Ticket : BaseEntity
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateDue { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Overdue { get; set; }
        [Required] public int UserId { get; set; }


        // Many-to-One Relationship
        public TicketType Type { get; set; }
        [Required] public int TicketTypeId { get; set; }
        public TicketPriority Priority { get; set; }
        [Required] public int PriorityId { get; set; }
        public TicketStatus Status { get; set; }
        [Required] public int TicketStatusId { get; set; }
        public Instrument Instrument { get; set; }
        [Required] public int InstrumentId { get; set; }
        public Issue Issue { get; set; }
        [Required] public int IssueId { get; set; }

        // One-to-Many Relationship
        public List<TicketLog> TicketLogs { get; set; }
        public List<TicketFile> TicketFiles { get; set; }
    }
}
