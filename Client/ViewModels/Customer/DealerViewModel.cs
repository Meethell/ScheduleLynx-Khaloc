using Caliburn.Micro;
using Client.Services;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System;
using Client.Entities.CustomerEntities;
using Client.Entities.ModelEntities;
using System.Linq;

namespace Client.ViewModels
{
    public class DealerViewModel : Screen
    {
        private readonly MainViewModel _mainViewModel;
        private readonly CustomerMainViewModel _customerMainViewModel1;
        // Constructor
        public DealerViewModel(MainViewModel mainViewModel, CustomerMainViewModel customerMainViewModel)
        {
            _mainViewModel = mainViewModel;
            _customerMainViewModel1 = customerMainViewModel;
            
            LoadData();
            IsDealerUserEditting = false;
            IsCustomerUserEditting = false;

            IsCustomerNewButtonEnable = false;
            IsCustomerEnable = true;
            IsDealerEnable = true;
        }
        //-------------------------------------------------------------------------------------
        //Setting Properties
        #region Dealer Setting Properties
        // Dealer properties
        private List<Dealer> _tempDealer; // Dùng để lưu trữ dữ liệu khi người dùng editting

        private ObservableCollection<Dealer> _Dealers; // Dùng để hiển thị dữ liệu
        public ObservableCollection<Dealer> Dealers
        {
            get { return _Dealers; }
            set
            {
                _Dealers = value;
                NotifyOfPropertyChange(() => Dealers);
                IsDealerEditButtonEnable = Dealers.Count > 0;
            }
        }

        private Dealer _selectedDealer;
        public Dealer SelectedDealer
        {
            get { return _selectedDealer; }
            set
            {
                _selectedDealer = value;
                NotifyOfPropertyChange(() => SelectedDealer);
                if (SelectedDealer != null && SelectedDealer.Id != 0)
                {
                    LoadCustomer();
                    // Đưa danh sách các Instrument có liên quan đến dealer
                    Instruments = new ObservableCollection<Instrument>(CachesServices.Instance.Instruments.FindAll(x => x.Customer.DealerId == SelectedDealer.Id));
                    // Đưa dữ liệu về trạng thái ban đầu
                    IsCustomerUserEditting = false;
                    IsCustomerNewButtonEnable = true;
                }
                else
                {
                    IsCustomerNewButtonEnable = false;
                    Customers = new ObservableCollection<Customer>();
                    Instruments = new ObservableCollection<Instrument>();
                }
                SetDealerDeleteButtonEnable();

            }
        }
        // IsDealerUserEditting - KEY - Để xác định xem người dùng đang editting hay không
        // - Gía trị ngươc lại với IsDealerReadOnly
        // - Cho phép nút Ok và Cancel hiện lên khi IsDealerUserEditting = true
        private bool _isDealerUserEditting;
        public bool IsDealerUserEditting
        {
            get { return _isDealerUserEditting; }
            set
            {
                _isDealerUserEditting = value;
                NotifyOfPropertyChange(() => IsDealerUserEditting);
                IsDealerReadOnly = !value;
                IsDealerNewButtonEnable = !value;
                SetDealerDeleteButtonEnable();
            }
        }
        // IsDealerReadOnly - Để xác định xem người dùng có thể chỉnh sửa DealerDataGrid hay không
        // - Cho phép nút Edit hiện lên khi IsDealerReadOnly = true
        private bool _isDealerReadOnly;
        public bool IsDealerReadOnly
        {
            get { return _isDealerReadOnly; }
            set
            {
                _isDealerReadOnly = value;
                NotifyOfPropertyChange(() => IsDealerReadOnly);

            }
        }
        // IsDealerEnable - Để xác định xem người dùng có thể chỉnh sửa DealerDataGrid hay không
        private bool _isDealerEnable;
        public bool IsDealerEnable
        {
            get { return _isDealerEnable; }
            set
            {
                _isDealerEnable = value;
                NotifyOfPropertyChange(() => IsDealerEnable);
            }
        }

        // Button Properties

        // IsDealerEditButtonEnable - Để xác định xem nút Edit có được enable hay không
        // - Nếu DealerDataGrid không có dữ liệu thì nút Edit sẽ không được enable
        private bool _isDealerEditButtonEnable;
        public bool IsDealerEditButtonEnable
        {
            get { return _isDealerEditButtonEnable; }
            set
            {
                _isDealerEditButtonEnable = value;
                NotifyOfPropertyChange(() => IsDealerEditButtonEnable);
            }
        }
        // IsDealerDeleteButtonEnable - Để xác định xem nút Delete có được enable hay không
        // - Nếu SelectedDealer = null thì nút Delete sẽ không được enable
        private bool _isDealerDeleteButtonEnable;
        public bool IsDealerDeleteButtonEnable
        {
            get { return _isDealerDeleteButtonEnable; }
            set
            {
                _isDealerDeleteButtonEnable = value;
                NotifyOfPropertyChange(() => IsDealerDeleteButtonEnable);
            }
        }
        // IsDealerNewButtonEnable - Để xác định xem nút New có được enable hay không
        // - Nếu IsDealerUserEditting = true thì nút New sẽ không được enable
        private bool _isDealerNewButtonEnable;
        public bool IsDealerNewButtonEnable
        {
            get { return _isDealerNewButtonEnable; }
            set
            {
                _isDealerNewButtonEnable = value;
                NotifyOfPropertyChange(() => IsDealerNewButtonEnable);
            }
        }
        // Lưu trạng thái của các nút
        private bool _tempIsDealerNewButtonEnable;
        private bool _tempIsDealerEditButtonEnable;
        private bool _tempIsDealerDeleteButtonEnable;
        private bool _tempIsDealerEnable;
        #endregion
        //-------------------------------------------------------------------------------------
        #region Customer Setting Properties
        // Customer properties
        private ObservableCollection<Customer> _Customers; // Dùng để hiển thị dữ liệu
        public ObservableCollection<Customer> Customers
        {
            get { return _Customers; }
            set
            {
                _Customers = value;
                NotifyOfPropertyChange(() => Customers);
            }
        }
        private List<Customer> _tempCustomer; // Dùng để lưu trữ dữ liệu khi người dùng editting

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                NotifyOfPropertyChange(() => SelectedCustomer);
                SetCustomerDeleteButtonEnable();
                IsCustomerEditButtonEnable = SelectedCustomer != null;
            }
        }
        // IsCustomerUserEditting - KEY - Để xác định xem người dùng đang editting hay không
        // - Gía trị ngươc lại với IsCustomerReadOnly
        // - Cho phép nút Ok và Cancel hiện lên khi IsCustomerUserEditting = true
        private bool _isCustomerUserEditting;
        public bool IsCustomerUserEditting
        {
            get { return _isCustomerUserEditting; }
            set
            {
                _isCustomerUserEditting = value;
                NotifyOfPropertyChange(() => IsCustomerUserEditting);
                IsCustomerReadOnly = !value;
                IsCustomerNewButtonEnable = !value;
                SetCustomerDeleteButtonEnable();
            }
        }
        // IsCustomerReadOnly - Để xác định xem người dùng có thể chỉnh sửa CustomerDataGrid hay không
        // - Cho phép nút Edit hiện lên khi IsCustomerReadOnly = true
        private bool _isCustomerReadOnly;
        public bool IsCustomerReadOnly
        {
            get { return _isCustomerReadOnly; }
            set
            {
                _isCustomerReadOnly = value;
                NotifyOfPropertyChange(() => IsCustomerReadOnly);
            }
        }
        // IsCustomerEnable - Để xác định xem người dùng có thể chỉnh sửa CustomerDataGrid hay không
        private bool _isCustomerEnable;
        public bool IsCustomerEnable
        {
            get { return _isCustomerEnable; }
            set
            {
                _isCustomerEnable = value;
                NotifyOfPropertyChange(() => IsCustomerEnable);
            }
        }


        // Button Properties

        // IsCustomerEditButtonEnable - Để xác định xem nút Edit có được enable hay không
        // - Nếu CustomerDataGrid không có dữ liệu thì nút Edit sẽ không được enable
        private bool _isCustomerEditButtonEnable;
        public bool IsCustomerEditButtonEnable
        {
            get { return _isCustomerEditButtonEnable; }
            set
            {
                _isCustomerEditButtonEnable = value;
                NotifyOfPropertyChange(() => IsCustomerEditButtonEnable);
            }
        }
        // IsCustomerDeleteButtonEnable - Để xác định xem nút Delete có được enable hay không
        // - Nếu SelectedCustomer = null thì nút Delete sẽ không được enable
        private bool _isCustomerDeleteButtonEnable;
        public bool IsCustomerDeleteButtonEnable
        {
            get { return _isCustomerDeleteButtonEnable; }
            set
            {
                _isCustomerDeleteButtonEnable = value;
                NotifyOfPropertyChange(() => IsCustomerDeleteButtonEnable);
            }
        }
        // IsCustomerNewButtonEnable - Để xác định xem nút New có được enable hay không
        // - Nếu IsCustomerUserEditting = true thì nút New sẽ không được enable
        // - Nếu Customers = null thì nút New sẽ được enable
        private bool _isCustomerNewButtonEnable;
        public bool IsCustomerNewButtonEnable
        {
            get { return _isCustomerNewButtonEnable; }
            set
            {
                _isCustomerNewButtonEnable = value;
                NotifyOfPropertyChange(() => IsCustomerNewButtonEnable);
            }
        }
        // Lưu trạng thái của các nút
        private bool _tempIsCustomerNewButtonEnable;
        private bool _tempIsCustomerEditButtonEnable;
        private bool _tempIsCustomerDeleteButtonEnable;
        private bool _tempIsCustomerEnable;
        #endregion
        //-------------------------------------------------------------------------------------

        // Methods
        // Load Data
        public void LoadData()
        {
            Dealers = new ObservableCollection<Dealer>(CachesServices.Instance.Dealers.OrderBy(x => x.Name));
        }

        // Load Customer theo Selected Dealer
        public void LoadCustomer()
        {
            if (SelectedDealer != null)
            {
                Customers = new ObservableCollection<Customer>(CachesServices.Instance.Customers.FindAll(x => x.DealerId == SelectedDealer.Id).OrderBy(x => x.Name));
            }
        }

        // Set delete button enable
        public void SetDealerDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedDealer != null và IsDealerUserEditting = false
            IsDealerDeleteButtonEnable = SelectedDealer != null && !IsDealerUserEditting;
        }
        public void SetCustomerDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedCustomer != null và IsCustomerUserEditting = false
            IsCustomerDeleteButtonEnable = SelectedCustomer != null && !IsCustomerUserEditting;
        }
        // Button Click
        //-------------------------------------------------------------------------------------
        #region Button Click Dealer
        // Edit Button Click
        public void bnDealerEdit()
        {
            // Lưu trữ dữ liệu ban đầu
            // create a true clone of the Dealers collection and ensure that the initial data is not modified
            _tempDealer = Dealers.Select(c => c.Clone()).ToList();

            IsDealerUserEditting = true;
            // Lưu trạng thái của các nút Customer
            _tempIsCustomerDeleteButtonEnable = IsCustomerDeleteButtonEnable;
            _tempIsCustomerNewButtonEnable = IsCustomerNewButtonEnable;
            _tempIsCustomerEditButtonEnable = IsCustomerEditButtonEnable;
            _tempIsCustomerEnable = IsCustomerEnable;
            // Khóa các nút Customer
            IsCustomerDeleteButtonEnable = false;
            IsCustomerNewButtonEnable = false;
            IsCustomerEditButtonEnable = false;
            IsCustomerEnable = false;
        }
        // Ok Button Click
        public async void bnDealerSave()
        {
            // Lưu dữ liệu hiển thị
            IsDealerUserEditting = false;
            // Lưu dữ liệu vào Caches
            await new SaveDataServices().SaveDealerListAsync(Dealers.ToList());
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            LoadData();
            // Trả lại trạng thái của các nút Customer
            IsCustomerDeleteButtonEnable = _tempIsCustomerDeleteButtonEnable;
            IsCustomerNewButtonEnable = _tempIsCustomerNewButtonEnable;
            IsCustomerEditButtonEnable = _tempIsCustomerEditButtonEnable;
            IsCustomerEnable = _tempIsCustomerEnable;
            IsCustomerNewButtonEnable = false;
        }

        // Cancel Button Click
        public void bnDealerCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu
            SelectedDealer = null;
            Dealers = new ObservableCollection<Dealer>(_tempDealer.ToList());
            IsDealerUserEditting = false;
            // Trả lại trạng thái của các nút Customer
            IsCustomerDeleteButtonEnable = _tempIsCustomerDeleteButtonEnable;
            IsCustomerNewButtonEnable = _tempIsCustomerNewButtonEnable;
            IsCustomerEditButtonEnable = _tempIsCustomerEditButtonEnable;
            IsCustomerEnable = _tempIsCustomerEnable;
        }
        // New Button Click
        public void bnDealerNew()
        {
            _tempDealer = Dealers.Select(c => c.Clone()).ToList(); // Lưu trữ dữ liệu ban đầu
            Dealers.Add(new Dealer { Name = "Tên Dealer ...", Address = "Địa chỉ ...", Notes = "" });
            NotifyOfPropertyChange(() => Dealers);
            SelectedDealer = Dealers[Dealers.Count - 1];
            IsDealerUserEditting = true;
            // Lưu trạng thái của các nút Customer
            _tempIsCustomerDeleteButtonEnable = IsCustomerDeleteButtonEnable;
            _tempIsCustomerNewButtonEnable = IsCustomerNewButtonEnable;
            _tempIsCustomerEditButtonEnable = IsCustomerEditButtonEnable;
            _tempIsCustomerEnable = IsCustomerEnable;
            // Khóa các nút Customer
            IsCustomerDeleteButtonEnable = false;
            IsCustomerNewButtonEnable = false;
            IsCustomerEditButtonEnable = false;
            IsCustomerEnable = false;
        }
        // Delete Button Click
        public async void bnDealerDelete()
        {
            // Kiểm tra Dealer có mới không
            if (SelectedDealer.Id == 0)
            {
                Dealers.Remove(SelectedDealer);
                return;
            }

            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "XÁC NHẬN!!",
                TxtMessage = "Bạn có chắc chắn xóa khách hàng này?\nViệc xóa khách hàng sẽ xóa hết tất cả thông tin liên quan đến khách hàng, bao gồm THIẾT BỊ và TICKET liên quan",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                if (SelectedDealer != null)
                {
                    await new DeleteDataServices().DeleteDealerAsync(SelectedDealer);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    LoadData();
                    SelectedDealer = null;
                }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------
        #region Button Click Customer
        // Edit Button Click
        public void bnCustomerEdit()
        {
            // Lưu trữ dữ liệu ban đầu
            _tempCustomer = Customers.Select(c => c.Clone()).ToList();
            IsCustomerUserEditting = true;
            // Lưu trạng thái của các nút Dealer
            _tempIsDealerDeleteButtonEnable = IsDealerDeleteButtonEnable;
            _tempIsDealerNewButtonEnable = IsDealerNewButtonEnable;
            _tempIsDealerEditButtonEnable = IsDealerEditButtonEnable;
            _tempIsDealerEnable = IsDealerEnable;
            // Deactive các nút Dealer
            IsDealerDeleteButtonEnable = false;
            IsDealerNewButtonEnable = false;
            IsDealerEditButtonEnable = false;
            IsDealerEnable = false;
        }
        // Ok Button Click
        public async void bnCustomerSave()
        {
            IsCustomerUserEditting = false;
            // Lưu dữ liêu hiển thị
            ObservableCollection<Customer> temp = new ObservableCollection<Customer>(Customers);
            foreach (var Customer in temp)
            {
                Customer.Dealer = null;
            }
            // Lưu dữ liệu vào database
            await new SaveDataServices().SaveCustomerListAsync(temp.ToList());
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Load lại dữ liệu
            LoadCustomer();
            // Trả lại trạng thái của các nút Dealer
            IsDealerDeleteButtonEnable = _tempIsDealerDeleteButtonEnable;
            IsDealerNewButtonEnable = _tempIsDealerNewButtonEnable;
            IsDealerEditButtonEnable = _tempIsDealerEditButtonEnable;
            IsDealerEnable = _tempIsDealerEnable;
        }
        // Cancel Button Click
        public void bnCustomerCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu
            Customers = new ObservableCollection<Customer>(_tempCustomer.ToList());
            IsCustomerUserEditting = false;
            // Trả lại trạng thái của các nút Dealer
            IsDealerDeleteButtonEnable = _tempIsDealerDeleteButtonEnable;
            IsDealerNewButtonEnable = _tempIsDealerNewButtonEnable;
            IsDealerEditButtonEnable = _tempIsDealerEditButtonEnable;
            IsDealerEnable = _tempIsDealerEnable;
        }
        // New Button Click
        public void bnCustomerNew()
        {
            _tempCustomer = Customers.Select(c => c.Clone()).ToList(); // Lưu trữ dữ liệu ban đầu
            Customers.Add(new Customer { Name = "Khách hàng ...", DealerId = SelectedDealer.Id, Dealer = SelectedDealer, Address = "Địa chỉ ...", Notes = "" });
            NotifyOfPropertyChange(() => Customers);
            SelectedCustomer = Customers[Customers.Count - 1];
            IsCustomerUserEditting = true;
            // Lưu trạng thái của các nút Dealer
            _tempIsDealerDeleteButtonEnable = IsDealerDeleteButtonEnable;
            _tempIsDealerNewButtonEnable = IsDealerNewButtonEnable;
            _tempIsDealerEditButtonEnable = IsDealerEditButtonEnable;
            _tempIsDealerEnable = IsDealerEnable;
            // Deactive các nút Dealer
            IsDealerDeleteButtonEnable = false;
            IsDealerNewButtonEnable = false;
            IsDealerEditButtonEnable = false;
            IsDealerEnable = false;
        }
        // Delete Button Click
        public async void bnCustomerDelete()
        {
            // Kiểm tra Customer mới được thêm sẽ không có Id, nếu đúng thì xóa trực tiếp trong Customers
            if (SelectedCustomer.Id == 0)
            {
                Customers.Remove(SelectedCustomer);
                return;
            }

            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "Xác nhận!!",
                TxtMessage = "Bạn có muốn xóa khách hàng này?",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                if (SelectedCustomer != null)
                {
                    await new DeleteDataServices().DeleteCustomerAsync(SelectedCustomer);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    LoadCustomer();
                    SelectedCustomer = null;
                }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------
        // Các phương pháp tìm kiếm
        // Tìm kiếm Dealer
        private string _searchDealer;
        public string SearchDealer
        {
            get { return _searchDealer; }
            set
            {
                _searchDealer = value;
                NotifyOfPropertyChange(() => SearchDealer);
                if (string.IsNullOrEmpty(SearchDealer))
                {
                    Dealers = new ObservableCollection<Dealer>(CachesServices.Instance.Dealers);
                }
                else
                {
                    Dealers = new ObservableCollection<Dealer>(CachesServices.Instance.Dealers.Where(c => c.Name.ToLower().Contains(SearchDealer.ToLower())));
                }
            }
        }
        // Tìm kiếm Customer
        private string _searchCustomer;
        public string SearchCustomer
        {
            get { return _searchCustomer; }
            set
            {
                _searchCustomer = value;
                NotifyOfPropertyChange(() => SearchCustomer);
                if (string.IsNullOrEmpty(SearchCustomer))
                {
                    LoadCustomer();
                }
                else
                {
                    LoadCustomer();
                    _tempCustomer = new List<Customer>(Customers);
                    Customers = new ObservableCollection<Customer>(_tempCustomer.Where(c => c.Name.ToLower().Contains(SearchCustomer.ToLower())));
                }
            }
        }
        //-------------------------------------------------------------------------------------
        // Instruments
        private ObservableCollection<Instrument> _Instruments;
        public ObservableCollection<Instrument> Instruments
        {
            get { return _Instruments; }
            set
            {
                _Instruments = value;
                NotifyOfPropertyChange(() => Instruments);
            }
        }

        public async Task ViewInstrument(RoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement fe)
            {
                if (fe.DataContext is Instrument instrument)
                {
                    await _mainViewModel.OpenInstrumentDetailView(CachesServices.Instance.Instruments.Find(x => x.Id == instrument.Id));
                }
            }
        }
    }
}
