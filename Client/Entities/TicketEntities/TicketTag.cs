using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entities.TicketEntities
{
    public class TicketTag
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TicketId { get; set; }
    }
}
