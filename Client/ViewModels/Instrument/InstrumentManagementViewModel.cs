using Caliburn.Micro;
using Client.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Client.Entities.ModelEntities;

namespace Client.ViewModels
{
    public class InstrumentManagementViewModel : Screen
    {
        // Full properties
        private bool _isNewButtonVisible = false;
        public bool IsNewButtonVisible
        {
            get { return _isNewButtonVisible; }
            set
            {
                _isNewButtonVisible = value;
                NotifyOfPropertyChange(() => IsNewButtonVisible);
            }
        }
        private ObservableCollection<Instrument> _instruments;
        public ObservableCollection<Instrument> Instruments
        {
            get { return _instruments; }
            set
            {
                _instruments = value;
                NotifyOfPropertyChange(() => Instruments);
            }
        }
        private Instrument _selectedInstrument;
        public Instrument SelectedInstrument
        {
            get { return _selectedInstrument; }
            set
            {
                _selectedInstrument = value;
                NotifyOfPropertyChange(() => SelectedInstrument);
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                LoadInstruments();
            }
        }
        private List<string> _filters = new List<string> { "Tên thiết bị", "Số Seri", "Khách hàng" };
        public List<string> Filters
        {
            get { return _filters; }
            set
            {
                _filters = value;
                NotifyOfPropertyChange(() => Filters);
            }
        }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                NotifyOfPropertyChange(() => SelectedFilter);
            }
        }
        // Constructor
        private readonly InstrumentViewModel _instrumentViewModel;
        public InstrumentManagementViewModel(InstrumentViewModel instrumentViewModel)
        {
            // Hiển thị nút New nếu là Admin
            IsNewButtonVisible = CachesServices.SystemRole?.Name == "Admin";

            _instrumentViewModel = instrumentViewModel;
            LoadInstruments();

        }
        // Method
        public void LoadInstruments()
        {
            //Load instruments from database
            if (SearchText != null)
            {
                switch (SelectedFilter)
                {
                    case "Tên thiết bị":
                        Instruments = new ObservableCollection<Instrument>(CachesServices.Instance.Instruments.Where(x => x.Name.ToLower().Contains(SearchText.ToLower())).OrderBy(x => x.Name));
                        break;
                    case "Số Seri":
                        Instruments = new ObservableCollection<Instrument>(CachesServices.Instance.Instruments.Where(x => x.SerialNumber.ToLower().Contains(SearchText.ToLower())).OrderBy(x => x.Name));
                        break;
                    case "Khách hàng":
                        Instruments = new ObservableCollection<Instrument>(CachesServices.Instance.Instruments.Where(x => x.Customer.Name.ToLower().Contains(SearchText.ToLower())).OrderBy(x => x.Name));
                        break;
                    default:
                        Instruments = new ObservableCollection<Instrument>(CachesServices.Instance.Instruments.Where(x => x.Name.ToLower().Contains(SearchText.ToLower())).OrderBy(x => x.Name));
                        break;
                }
            }
            else
            {
                Instruments = new ObservableCollection<Instrument>(CachesServices.Instance.Instruments.OrderBy(x => x.Name));
            }
        }

        // Method
        public async Task btNew()
        {
            await _instrumentViewModel.NewInstrumentView(new Instrument(), true);
        }
        public async Task OpenInstrumentDetailViewCommand()
        {
            await _instrumentViewModel.InstrumentDetail(CachesServices.Instance.Instruments.Find(x => x.Id == SelectedInstrument.Id));
        }

    }
}
