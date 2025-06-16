using System.ComponentModel;

namespace Client.Entities.ModelEntities
{
    public class ModelSpecification : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Specification { get; set; }
        public string QuoteLine { get; set; }
        public string ReferenceLine { get; set; }
        private ModelFile _modelFile;
        public ModelFile ModelFile
        {
            get => _modelFile;
            set
            {
                _modelFile = value;
                OnPropertyChanged(nameof(ModelFile));
            }
        }
        public int ModelFileId { get; set; }
        private ModelSpecificationGroup _modelSpecificationGroup;
        public ModelSpecificationGroup ModelSpecificationGroup
        {
            get => _modelSpecificationGroup;
            set
            {
                _modelSpecificationGroup = value;
                OnPropertyChanged(nameof(ModelSpecificationGroup));
            }
        }
        public int ModelSpecificationGroupId { get; set; }
        public bool IsKeySpecification { get; set; }
        public Model Model { get; set; }
        public int ModelId { get; set; }
        public string Notes { get; set; }

        public ModelSpecification Clone()
        {
            return new ModelSpecification
            {
                Id = this.Id,
                Specification = this.Specification,
                QuoteLine = this.QuoteLine,
                ModelFile = this.ModelFile,
                ModelFileId = this.ModelFileId,
                IsKeySpecification = this.IsKeySpecification,
                Model = this.Model,
                ModelId = this.ModelId,
                Notes = this.Notes,
                ModelSpecificationGroup = this.ModelSpecificationGroup,
                ModelSpecificationGroupId = this.ModelSpecificationGroupId,
                ReferenceLine = this.ReferenceLine
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
