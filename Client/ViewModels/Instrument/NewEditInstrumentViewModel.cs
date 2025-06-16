using Caliburn.Micro;
using Client.Entities.CustomerEntities;
using Client.Entities.ModelEntities;
using Client.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class NewEditInstrumentViewModel : Screen
    {
        // Full properties
        private string _newEditTitle;
        public string NewEditTitle
        {
            get { return _newEditTitle; }
            set
            {
                _newEditTitle = value;
                NotifyOfPropertyChange(() => NewEditTitle);
            }
        }
        private string _newEditInstrumentTitle;
        public string NewEditInstrumentTitle
        {
            get { return _newEditInstrumentTitle; }
            set
            {
                _newEditInstrumentTitle = value;
                NotifyOfPropertyChange(() => NewEditInstrumentTitle);
            }
        }
        private bool _isInstrumnetDetailVisible = false;
        public bool IsInstrumnetDetailVisible
        {
            get { return _isInstrumnetDetailVisible; }
            set
            {
                _isInstrumnetDetailVisible = value;
                NotifyOfPropertyChange(() => IsInstrumnetDetailVisible);
            }
        }
        private bool _isNewEditInstrumentVisible = false;
        public bool IsNewEditInstrumentVisible
        {
            get { return _isNewEditInstrumentVisible; }
            set
            {
                _isNewEditInstrumentVisible = value;
                NotifyOfPropertyChange(() => IsNewEditInstrumentVisible);
            }
        }

        // Insrtument properties
        private Instrument _instrument;
        public Instrument Instrument
        {
            get { return _instrument; }
            set
            {
                _instrument = value;
                NotifyOfPropertyChange(() => Instrument);
            }
        }
        // Lấy các ModelType không là Consumable
        private ObservableCollection<ModelType> _modelTypes = new ObservableCollection<ModelType>(CachesServices.Instance.ModelTypes.Where(x => x.IsConsumable == false));
        public ObservableCollection<ModelType> ModelTypes
        {
            get { return _modelTypes; }
            set
            {
                _modelTypes = value;
                NotifyOfPropertyChange(() => ModelTypes);
            }
        }
        private ObservableCollection<InstrumentStatus> _instrumentStatuses = new ObservableCollection<InstrumentStatus>(CachesServices.Instance.InstrumentStatuses);
        public ObservableCollection<InstrumentStatus> InstrumentStatuses
        {
            get { return _instrumentStatuses; }
            set
            {
                _instrumentStatuses = value;
                NotifyOfPropertyChange(() => InstrumentStatuses);
            }
        }
        private ObservableCollection<Model> _models;
        public ObservableCollection<Model> Models
        {
            get { return _models; }
            set
            {
                _models = value;
                NotifyOfPropertyChange(() => Models);
            }
        }

        private ObservableCollection<Manufacturer> _manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers);
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
                // Get models by manufacturer
                GetModelsByManufacturerAndInstrumentType();
            }
        }
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>(CachesServices.Instance.Customers);
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                NotifyOfPropertyChange(() => Customers);
            }
        }
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                NotifyOfPropertyChange(() => SelectedCustomer);
                Instrument.Customer = SelectedCustomer;
            }
        }
        private ModelType _selectedInstrumentType;
        public ModelType SelectedInstrumentType
        {
            get { return _selectedInstrumentType; }
            set
            {
                _selectedInstrumentType = value;
                NotifyOfPropertyChange(() => SelectedInstrumentType);
                // Get models by instrumentType
                GetModelsByManufacturerAndInstrumentType();
            }
        }

        private bool IsNew { get; set; }
        // Constructor
        private readonly InstrumentViewModel _instrumentViewModel;
        public NewEditInstrumentViewModel(Instrument instrument, bool isNew, InstrumentViewModel instrumentViewModel)
        {
            // Initialize with default view
            Instrument = instrument;
            IsNew = isNew;
            _instrumentViewModel = instrumentViewModel;
            if (IsNew)
            {
                NewEditTitle = "Tạo mới Thiết bị";
                Instrument.InstallDate = DateTime.Now;
                Instrument.LastPmDate = DateTime.Now;
                Instrument.NextPmDate = DateTime.Now;
            }
            else
            {
                NewEditTitle = "Chỉnh sửa Thiết bị";
                CustomerSearchText = Instrument.Customer.Name;
                SelectedCustomer = Instrument.Customer;
                SelectedInstrumentType = GetInstrumentTypeByModel();
                SelectedManufacturer = GetManufacturerByModel();
            }
        }

        // Get models by manufacturer and InstrumentType
        public void GetModelsByManufacturerAndInstrumentType()
        {
            if (SelectedManufacturer != null && SelectedInstrumentType != null)
            {
                Models = new ObservableCollection<Model>(CachesServices.Instance.Models.FindAll(x => x.ManufacturerId == SelectedManufacturer.Id && x.ModelTypeId == SelectedInstrumentType.Id));
            }
        }
        // Get manufacturer by model
        public Manufacturer GetManufacturerByModel()
        {
            if (Instrument.Model != null)
            {
                return CachesServices.Instance.Manufacturers.FirstOrDefault(x => x.Id == Instrument.Model.ManufacturerId);
            }
            return null;
        }
        // Get InstrumentType by model
        public ModelType GetInstrumentTypeByModel()
        {
            if (Instrument.Model != null)
            {
                return CachesServices.Instance.ModelTypes.FirstOrDefault(x => x.Id == Instrument.Model.ModelTypeId);
            }
            return null;
        }
        // Save instrument
        public async Task btSave()
        {
            // Kiểm tra Các miền dữ liệu
            if (string.IsNullOrEmpty(Instrument.Name) || string.IsNullOrEmpty(Instrument.SerialNumber) || Instrument.Model == null || Instrument.Customer == null || Instrument.Status == null)
            {
                // Thông báo lỗi
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Lỗi",
                    TxtMessage = "Các trường chưa được điền đầy đủ",
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return;
            }
            else
            {
                Instrument.CustomerId = Instrument.Customer.Id;
                Instrument.Customer = null;
                Instrument.ModelId = Instrument.Model.Id;
                Instrument.Model = null;
                Instrument.StatusId = Instrument.Status.Id;
                Instrument.Status = null;
                // Lưu vào database
                await new SaveDataServices().SaveInstrumentAsync(Instrument);
                // Cập nhật lại Cache
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                // Mở lại InstrumentManagementView
                _instrumentViewModel.IsInstrumentManagementViewChecked = true;
                await _instrumentViewModel.InstrumentDetail(CachesServices.Instance.Instruments.Find(x => x.Id == Instrument.Id));
            }
        }
        public async Task btCancel()
        {
            _instrumentViewModel.IsInstrumentManagementViewChecked = true;
            await _instrumentViewModel.InstrumentManagementView();
        }

        private string _CustomerSearchText;
        public string CustomerSearchText
        {
            get { return _CustomerSearchText; }
            set
            {
                _CustomerSearchText = value;
                NotifyOfPropertyChange(() => CustomerSearchText);
                // Truy vấn dữ liệu
                if (!string.IsNullOrEmpty(CustomerSearchText))
                {
                    Customers = new ObservableCollection<Customer>(CachesServices.Instance.Customers.Where(x => x.Name.ToLower().Contains(CustomerSearchText.ToLower())));
                }
                else
                {
                    Customers = new ObservableCollection<Customer>(CachesServices.Instance.Customers);
                }
            }
        }
    }
}
