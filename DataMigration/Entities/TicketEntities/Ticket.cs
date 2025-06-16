using System;
using DataMigration.Entities.ModelEntities;

namespace DataMigration.Entities.TicketEntities
{
    public class Ticket
    {
        // Properties related to ticket information
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

        // Properties related to ticket type
        public TicketType Type { get; set; }
        public int TicketTypeId { get; set; }

        // Properties related to ticket priority
        public TicketPriority Priority { get; set; }
        public int PriorityId { get; set; }

        // Properties related to ticket status
        public TicketStatus Status { get; set; }
        public int TicketStatusId { get; set; }

        // Properties related to ticket dates
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateDue { get; set; }

        // Properties related to ticket associations
        public Instrument Instrument { get; set; }
        public int InstrumentId { get; set; }

        // Properties related to Issue
        public Issue Issue { get; set; }
        public int IssueId { get; set; }

        // Additional properties
        public bool Overdue { get; set; }
    }
}
