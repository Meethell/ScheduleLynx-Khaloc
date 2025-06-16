using System.ComponentModel;

namespace Client.Entities.ModelEntities
{
    public class Manufacturer : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private Country _country;
        public Country Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged(nameof(Country));
            }
        }
        public int CountryId { get; set; }
        public string Descriptions { get; set; }

        // Phương thức Clone
        public Manufacturer Clone()
        {
            return new Manufacturer
            {
                Id = this.Id,
                Name = this.Name,
                Country = this.Country,
                CountryId = this.CountryId,
                Descriptions = this.Descriptions
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
