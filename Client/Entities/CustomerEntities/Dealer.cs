using System.Collections.Generic;

namespace Client.Entities.CustomerEntities
{
    public class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; } = string.Empty;
        
        // Phương thức sao chép
        public Dealer Clone()
        {
            return new Dealer
            {
                Id = this.Id,
                Name = this.Name,
                Address = this.Address,
                Notes = this.Notes
            };
        }
    }
}
