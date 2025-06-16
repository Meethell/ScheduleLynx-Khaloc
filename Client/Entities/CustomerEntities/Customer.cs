using System.ComponentModel;

namespace Client.Entities.CustomerEntities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public int DealerId { get; set; }
        private Dealer _dealer;
        public Dealer Dealer
        {
            get => _dealer;
            set
            {
                _dealer = value;
                OnPropertyChanged(nameof(Dealer));
            }
        }

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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
