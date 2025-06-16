using System.ComponentModel;

namespace Client.Entities.ExpenseEntities
{
    public class Expense : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int Id { get; set; }
        public string Name { get; set; }
        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        private decimal _count;
        public decimal Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChanged(nameof(Count));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public decimal Total => Amount * Count;
        public string Notes { get; set; }
        public int TicketId { get; set; }

        // phương thức clone
        public Expense Clone()
        {
            return new Expense
            {
                Id = this.Id,
                Name = this.Name,
                Amount = this.Amount,
                Count = this.Count,
                // Total is a read-only property, do not assign
                Notes = this.Notes,
                TicketId = this.TicketId
            };
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
