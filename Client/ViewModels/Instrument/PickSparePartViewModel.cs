using Caliburn.Micro;
using Client.Entities.ModelEntities;
using Client.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class PickSparePartViewModel : Screen
    {
        // Setting properties for binding
        private bool _flag;
        public bool Flag
        {
            get => _flag;
            set
            {
                _flag = value;
                NotifyOfPropertyChange(() => Flag);
            }
        }
        private List<string> _filters = new List<string> { "Tên phụ tùng", "Mã phụ tùng" };
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
                LoadSpareParts();
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                LoadSpareParts();
            }
        }
        // Full property names for binding
        private SpareParts _selectedSparePart;
        public SpareParts SelectedSparePart
        {
            get { return _selectedSparePart; }
            set
            {
                _selectedSparePart = value;
                NotifyOfPropertyChange(() => SelectedSparePart);
            }
        }
        private BindableCollection<SpareParts> _spareParts;
        public BindableCollection<SpareParts> SpareParts
        {
            get { return _spareParts; }
            set
            {
                _spareParts = value;
                NotifyOfPropertyChange(() => SpareParts);
            }
        }
        private ObservableCollection<Manufacturer> _manufacturers;
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return _manufacturers; }
            set
            {
                _manufacturers = value;
                NotifyOfPropertyChange(() => Manufacturers);
            }
        }
        private Manufacturer _selectedManufacturer;
        public Manufacturer SelectedManufacturer
        {
            get { return _selectedManufacturer; }
            set
            {
                _selectedManufacturer = value;
                NotifyOfPropertyChange(() => SelectedManufacturer);
                LoadSpareParts();
            }
        }
        private string _quantity;
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyOfPropertyChange(() => Quantity);
            }
        }
        private int _quantityInt;
        public int QuantityInt
        {
            get { return _quantityInt; }
            set
            {
                _quantityInt = value;
                NotifyOfPropertyChange(() => QuantityInt);
            }
        }
        private string _serialNumber;
        public string SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                _serialNumber = value;
                NotifyOfPropertyChange(() => SerialNumber);
            }
        }
        // Constructor
        private readonly MainViewModel _mainViewModel;
        public PickSparePartViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Manufacturers = new ObservableCollection<Manufacturer>(
                CachesServices.Instance.Manufacturers.OrderBy(x => x.Name)
            );
            LoadSpareParts();
        }

        public void LoadSpareParts()
        {
            // Load spare parts from database
            var sparePartsQuery = CachesServices.Instance.SpareParts.AsQueryable();

            if (SelectedManufacturer != null)
            {
                sparePartsQuery = sparePartsQuery.Where(x => x.ManufacturerId == SelectedManufacturer.Id);
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                switch (SelectedFilter)
                {
                    case "Tên phụ tùng":
                        sparePartsQuery = sparePartsQuery
                            .Where(x => x.Name != null && x.Name.ToLower().Contains(SearchText.ToLower()));
                        break;
                    case "Mã phụ tùng":
                        sparePartsQuery = sparePartsQuery
                            .Where(x => x.PartNumber != null && x.PartNumber.ToLower().Contains(SearchText.ToLower()));
                        break;
                    default:
                        sparePartsQuery = sparePartsQuery
                            .Where(x => x.Name != null && x.Name.ToLower().Contains(SearchText.ToLower()));
                        break;
                }
            }

            SpareParts = new BindableCollection<SpareParts>(sparePartsQuery.OrderBy(x => x.Name));
        }

        // Button
        public async Task Ok()
        {
            // thử sparse Quantity
            if (!int.TryParse(Quantity, out int quantity) || quantity <= 0)
            {
                _mainViewModel.AppNotificationService.ShowError("Vui lòng nhập Số lượng hợp lệ!");
                return;
            }

            if (SelectedSparePart != null && quantity > 0)
            {
                QuantityInt = quantity;
                Flag = true;
                await TryCloseAsync().ConfigureAwait(false);
            }
            else
            {
                Flag = false;
                if (SelectedSparePart == null && quantity <= 0)
                {
                    _mainViewModel.AppNotificationService.ShowError("Vui lòng chọn Phụ tùng và nhập Số lượng hợp lệ!");
                }
                else if (SelectedSparePart == null)
                {
                    _mainViewModel.AppNotificationService.ShowError("Chưa chọn Phụ tùng!");
                }
                else // Quantity <= 0
                {
                    _mainViewModel.AppNotificationService.ShowError("Vui lòng nhập Số lượng lớn hơn 0!");
                }
            }
        }
        public async Task Cancel()
        {
            Flag = false;
            await TryCloseAsync().ConfigureAwait(false);
        }
    }
}
