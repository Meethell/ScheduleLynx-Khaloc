using Caliburn.Micro;
using Client.Entities.ModelEntities;
using Client.Entities.TicketEntities;
using Client.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels
{
    public class ModelCustomerViewModel : Screen
    {
        public class ModelCount
        {
            public Model Model { get; set; }
            public int Count { get; set; }
        }

        // View properties
        private bool _isModelSelectorVisible = true;
        public bool IsModelSelectorVisible
        {
            get => _isModelSelectorVisible;
            set
            {
                _isModelSelectorVisible = value;
                NotifyOfPropertyChange(() => IsModelSelectorVisible);
            }
        }
        private bool _isModelListVisible = false;
        public bool IsModelListVisible
        {
            get => _isModelListVisible;
            set
            {
                _isModelListVisible = value;
                NotifyOfPropertyChange(() => IsModelListVisible);
            }
        }

        // Properties
        private List<ModelCount> _tempModels;
        private ObservableCollection<ModelCount> _models;
        public ObservableCollection<ModelCount> Models
        {
            get => _models;
            set
            {
                _models = value;
                NotifyOfPropertyChange(() => Models);
            }
        }
        private ModelCount _selectedModel;
        public ModelCount SelectedModel
        {
            get => _selectedModel;
            set
            {
                _selectedModel = value;
                NotifyOfPropertyChange(() => SelectedModel);
                LoadInstrument();
            }
        }
        private ObservableCollection<Manufacturer> _manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers.OrderBy(x => x.Name));
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get => _manufacturers;
            set
            {
                _manufacturers = value;
                NotifyOfPropertyChange(() => Manufacturers);
            }
        }
        private Manufacturer _selectedManufacturer;
        public Manufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                _selectedManufacturer = value;
                NotifyOfPropertyChange(() => SelectedManufacturer);
                SelectedModel = null;
                if (value != null)
                {
                    LoadModel();
                    IsModelListVisible = true;

                }
                else
                {
                    IsModelListVisible = false;
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        // Tìm kiếm Manufacturer
        private string _searchManufacturer;
        public string SearchManufacturer
        {
            get { return _searchManufacturer; }
            set
            {
                _searchManufacturer = value;
                NotifyOfPropertyChange(() => SearchManufacturer);
                if (string.IsNullOrEmpty(SearchManufacturer))
                {
                    Manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers);
                }
                else
                {
                    Manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers.Where(c => c.Name.ToLower().Contains(SearchManufacturer.ToLower())).OrderBy(x => x.Name));
                }
            }
        }
        // Tìm kiếm Model
        private string _searchModel;
        public string SearchModel
        {
            get { return _searchModel; }
            set
            {
                _searchModel = value;
                NotifyOfPropertyChange(() => SearchModel);
                if (string.IsNullOrEmpty(SearchModel))
                {
                    LoadModel();
                }
                else
                {
                    LoadModel();
                    _tempModels = new List<ModelCount>(Models); // Lưu lại các Model đã được load khi chọn manufacturer
                    Models = new ObservableCollection<ModelCount>(
                        _tempModels
                            .Where(c => c.Model != null && c.Model.Name != null && c.Model.Name.ToLower().Contains(SearchModel.ToLower()))
                            .OrderBy(x => x.Model.Name)
                    );
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        // Constructor
        private readonly CustomerMainViewModel _customerMainViewModel;
        private readonly MainViewModel _mainViewModel;
        public ModelCustomerViewModel(MainViewModel mainViewModel, CustomerMainViewModel customerMainViewModel)
        {
            _customerMainViewModel = customerMainViewModel;
            _mainViewModel = mainViewModel;
        }

        // Methods
        public void LoadModel()
        {
            if (SelectedManufacturer != null)
            {
                Models = new ObservableCollection<ModelCount>();
                List<Model> temp = CachesServices.Instance.Models.FindAll(x => x.ManufacturerId == SelectedManufacturer.Id && x.IsConsumable == false).OrderBy(x => x.Name).ToList();
                foreach (var model in temp)
                {
                    var modelCount = new ModelCount
                    {
                        Model = model,
                        Count = CachesServices.Instance.Instruments.Count(x => x.ModelId == model.Id)
                    };
                    Models.Add(modelCount);
                }
            }
        }
        public void ToggleModelSelector()
        {
            IsModelSelectorVisible = !IsModelSelectorVisible;
        }

        //------------------------------------------------------------------------------------------
        // Phần Instrument
        private ObservableCollection<Instrument> _instruments;
        public ObservableCollection<Instrument> Instruments
        {
            get => _instruments;
            set
            {
                _instruments = value;
                NotifyOfPropertyChange(() => Instruments);
            }
        }

        public void LoadInstrument()
        {
            if (SelectedModel != null)
            {
                Instruments = new ObservableCollection<Instrument>(CachesServices.Instance.Instruments.Where(x => x.ModelId == SelectedModel.Model.Id).OrderBy(x => x.Customer.Dealer.Name));
            }
            else
            {
                Instruments = new ObservableCollection<Instrument>();
            }
        }

        public async Task ViewInstrument(RoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement fe)
            {
                if (fe.DataContext is Instrument ins)
                {
                    await _mainViewModel.OpenInstrumentDetailView(ins);
                }
            }
        }
    }
}
