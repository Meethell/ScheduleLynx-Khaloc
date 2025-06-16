using System.ComponentModel;

namespace Client.Entities.ModelEntities
{
    public class Model : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string Descriptions { get; set; }
        private ModelType _modelType;
        public ModelType ModelType
        {
            get => _modelType;
            set
            {
                _modelType = value;
                OnPropertyChanged(nameof(ModelType));
            }
        }
        public int ModelTypeId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int ManufacturerId { get; set; }
        // Gắn file ảnh vào Model
        public string ImagePath { get; set; }
        public byte[] Image { get; set; }
        // Có là VTTH không
        public bool IsConsumable { get; set; }
        public string Notes { get; set; }

        // Phương thức Clone
        public Model Clone()
        {
            return new Model
            {
                Id = this.Id,
                Name = this.Name,
                TradeName = this.TradeName,
                Descriptions = this.Descriptions,
                ModelType = this.ModelType,
                ModelTypeId = this.ModelTypeId,
                Manufacturer = this.Manufacturer,
                ManufacturerId = this.ManufacturerId,
                IsConsumable = this.IsConsumable,
                ImagePath = this.ImagePath,
                Image = this.Image,
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
