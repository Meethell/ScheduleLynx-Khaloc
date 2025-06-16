namespace DataMigration.Entities.CustomerEntities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public int DealerId { get; set; }
        public Dealer Dealer { get; set; }

        // Phương thức sao chép
        public Customer Clone()
        {
            return new Customer
            {
                Id = this.Id,
                Name = this.Name,
                Address = this.Address,
                DealerId = this.DealerId,
                Dealer = this.Dealer,
                Notes = this.Notes
            };
        }
    }
}
