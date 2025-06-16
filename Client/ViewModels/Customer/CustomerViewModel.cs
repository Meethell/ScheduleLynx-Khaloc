using Caliburn.Micro;
using Client.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Client.Entities.CustomerEntities;
using Client.Entities.ModelEntities;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class CustomerViewModel : Screen
    {
        private readonly MainViewModel _mainViewModel;
        private readonly CustomerMainViewModel _customerMainViewModel;
        // Constructor
        public CustomerViewModel(MainViewModel mainViewModel, CustomerMainViewModel customerMainViewModel)
        {
            _customerMainViewModel = customerMainViewModel;
            _mainViewModel = mainViewModel;
            LoadData();
            IsCustomerUserEditting = false;
            IsContactUserEditting = false;

            IsContactNewButtonEnable = false;
            IsContactEnable = true;
            IsCustomerEnable = true;
        }
        //-------------------------------------------------------------------------------------
        //Setting Properties
        #region Customer Setting Properties
        // Customer properties
        private List<Customer> _tempCustomer; // Dùng để lưu trữ dữ liệu khi người dùng editting

        private ObservableCollection<Customer> _Customers; // Dùng để hiển thị dữ liệu
        public ObservableCollection<Customer> Customers
        {
            get { return _Customers; }
            set
            {
                _Customers = value;
                NotifyOfPropertyChange(() => Customers);
                IsCustomerEditButtonEnable = Customers.Count > 0;
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
                if (SelectedCustomer != null)
                {
                    LoadContact();
                    Instruments = new ObservableCollection<Instrument>(CachesServices.Instance.Instruments.FindAll(x => x.CustomerId == SelectedCustomer.Id));
                    // Đưa dữ liệu về trạng thái ban đầu
                    IsContactUserEditting = false;
                    IsContactNewButtonEnable = true;
                }
                else
                {
                    IsContactNewButtonEnable = false;
                    Contacts = new ObservableCollection<Contact>();
                    Instruments = new ObservableCollection<Instrument>();
                }
                SetCustomerDeleteButtonEnable();

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
        #region Contact Setting Properties
        // Contact properties
        private ObservableCollection<Contact> _Contacts; // Dùng để hiển thị dữ liệu
        public ObservableCollection<Contact> Contacts
        {
            get { return _Contacts; }
            set
            {
                _Contacts = value;
                NotifyOfPropertyChange(() => Contacts);
            }
        }
        private List<Contact> _tempContact; // Dùng để lưu trữ dữ liệu khi người dùng editting
        
        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                NotifyOfPropertyChange(() => SelectedContact);
                SetContactDeleteButtonEnable();
                IsContactEditButtonEnable = SelectedContact != null;
            }
        }
        // IsContactUserEditting - KEY - Để xác định xem người dùng đang editting hay không
        // - Gía trị ngươc lại với IsContactReadOnly
        // - Cho phép nút Ok và Cancel hiện lên khi IsContactUserEditting = true
        private bool _isContactUserEditting;
        public bool IsContactUserEditting
        {
            get { return _isContactUserEditting; }
            set
            {
                _isContactUserEditting = value;
                NotifyOfPropertyChange(() => IsContactUserEditting);
                IsContactReadOnly = !value;
                IsContactNewButtonEnable = !value;
                SetContactDeleteButtonEnable();
            }
        }
        // IsContactReadOnly - Để xác định xem người dùng có thể chỉnh sửa ContactDataGrid hay không
        // - Cho phép nút Edit hiện lên khi IsContactReadOnly = true
        private bool _isContactReadOnly;
        public bool IsContactReadOnly
        {
            get { return _isContactReadOnly; }
            set
            {
                _isContactReadOnly = value;
                NotifyOfPropertyChange(() => IsContactReadOnly);
            }
        }
        // IsContactEnable - Để xác định xem người dùng có thể chỉnh sửa ContactDataGrid hay không
        private bool _isContactEnable;
        public bool IsContactEnable
        {
            get { return _isContactEnable; }
            set
            {
                _isContactEnable = value;
                NotifyOfPropertyChange(() => IsContactEnable);
            }
        }


        // Button Properties

        // IsContactEditButtonEnable - Để xác định xem nút Edit có được enable hay không
        // - Nếu ContactDataGrid không có dữ liệu thì nút Edit sẽ không được enable
        private bool _isContactEditButtonEnable;
        public bool IsContactEditButtonEnable
        {
            get { return _isContactEditButtonEnable; }
            set
            {
                _isContactEditButtonEnable = value;
                NotifyOfPropertyChange(() => IsContactEditButtonEnable);
            }
        }
        // IsContactDeleteButtonEnable - Để xác định xem nút Delete có được enable hay không
        // - Nếu SelectedContact = null thì nút Delete sẽ không được enable
        private bool _isContactDeleteButtonEnable;
        public bool IsContactDeleteButtonEnable
        {
            get { return _isContactDeleteButtonEnable; }
            set
            {
                _isContactDeleteButtonEnable = value;
                NotifyOfPropertyChange(() => IsContactDeleteButtonEnable);
            }
        }
        // IsContactNewButtonEnable - Để xác định xem nút New có được enable hay không
        // - Nếu IsContactUserEditting = true thì nút New sẽ không được enable
        // - Nếu Contacts = null thì nút New sẽ được enable
        private bool _isContactNewButtonEnable;
        public bool IsContactNewButtonEnable
        {
            get { return _isContactNewButtonEnable; }
            set
            {
                _isContactNewButtonEnable = value;
                NotifyOfPropertyChange(() => IsContactNewButtonEnable);
            }
        }
        // Lưu trạng thái của các nút
        private bool _tempIsContactNewButtonEnable;
        private bool _tempIsContactEditButtonEnable;
        private bool _tempIsContactDeleteButtonEnable;
        private bool _tempIsContactEnable;
        #endregion
        //-------------------------------------------------------------------------------------
        private ObservableCollection<Dealer> _dealers;
        public ObservableCollection<Dealer> Dealers
        {
            get { return _dealers; }
            set
            {
                _dealers = value;
                NotifyOfPropertyChange(() => Dealers);
            }
        }
        // Methods
        // Load Data
        public void LoadData()
        {
            Customers = new ObservableCollection<Customer>(CachesServices.Instance.Customers);
            Dealers = new ObservableCollection<Dealer>(CachesServices.Instance.Dealers);
        }

        // Load Contact theo Selected Customer
        public void LoadContact()
        {
            if (SelectedCustomer != null)
            {
                Contacts = new ObservableCollection<Contact>(CachesServices.Instance.Contacts.FindAll(x => x.CustomerId == SelectedCustomer.Id));
                //foreach (var contact in Contacts)
                //{
                //    // Gán contact cho Customer bằng CustomerId
                //    contact.Customer = Customers.FirstOrDefault(x => x.Id == contact.CustomerId);
                //}
            }
        }

        // Set delete button enable
        public void SetCustomerDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedCustomer != null và IsCustomerUserEditting = false
            IsCustomerDeleteButtonEnable = SelectedCustomer != null && !IsCustomerUserEditting;
        }
        public void SetContactDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedContact != null và IsContactUserEditting = false
            IsContactDeleteButtonEnable = SelectedContact != null && !IsContactUserEditting;
        }
        // Button Click
        //-------------------------------------------------------------------------------------
        #region Button Click Customer
        // Edit Button Click
        public void bnCustomerEdit()
        {
            // Lưu trữ dữ liệu ban đầu
            _tempCustomer = Customers.Select(c => c.Clone()).ToList();
            IsCustomerUserEditting = true;
            // Lưu trạng thái của các nút Contact
            _tempIsContactDeleteButtonEnable = IsContactDeleteButtonEnable;
            _tempIsContactNewButtonEnable = IsContactNewButtonEnable;
            _tempIsContactEditButtonEnable = IsContactEditButtonEnable;
            _tempIsContactEnable = IsContactEnable;
            // Khóa các nút Contact
            IsContactDeleteButtonEnable = false;
            IsContactNewButtonEnable = false;
            IsContactEditButtonEnable = false;
            IsContactEnable = false;
        }
        // Ok Button Click
        public async void bnCustomerSave()
        {
            // Check if a Dealer has been selected
            foreach (var item in Customers)
            {
                if (item.Dealer == null)
                {
                    var customMessageBoxViewModel = new CustomMessageBoxViewModel
                    {
                        Message = "Error",
                        TxtMessage = "Vui lòng chọn Nhà phân phối",
                        IsInformation = true
                    };
                    var windowManager = new WindowManager();
                    await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                    SelectedCustomer = item;
                    return;
                }
            }
            // Lưu dữ liệu hiển thị
            IsCustomerUserEditting = false;
            // Đưa Dealer về null trước khi lưu
            ObservableCollection<Customer> temp = new ObservableCollection<Customer>(Customers);
            foreach (var customer in temp)
            {
                customer.DealerId = customer.Dealer.Id;
                customer.Dealer = null;
            }
            // Lưu dữ liệu vào Caches
            await new SaveDataServices().SaveCustomerListAsync(temp.ToList());
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            LoadData();
            // Trả lại trạng thái của các nút Contact
            IsContactDeleteButtonEnable = _tempIsContactDeleteButtonEnable;
            IsContactNewButtonEnable = _tempIsContactNewButtonEnable;
            IsContactEditButtonEnable = _tempIsContactEditButtonEnable;
            IsContactEnable = _tempIsContactEnable;
            IsContactNewButtonEnable = false;
        }
        // Cancel Button Click
        public void bnCustomerCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu
            SelectedCustomer = null;
            Customers = new ObservableCollection<Customer>(_tempCustomer.Select(c => c.Clone()).ToList());
            IsCustomerUserEditting = false;
            // Trả lại trạng thái của các nút Contact
            IsContactDeleteButtonEnable = _tempIsContactDeleteButtonEnable;
            IsContactNewButtonEnable = _tempIsContactNewButtonEnable;
            IsContactEditButtonEnable = _tempIsContactEditButtonEnable;
            IsContactEnable = _tempIsContactEnable;
        }
        // New Button Click
        public void bnCustomerNew()
        {
            _tempCustomer = Customers.Select(c => c.Clone()).ToList(); ; // Lưu trữ dữ liệu ban đầu
            Customers.Add(new Customer { Name = "Khách hàng ...", Address = "Địa chỉ ...", Notes = "" });
            NotifyOfPropertyChange(() => Customers);
            SelectedCustomer = Customers[Customers.Count - 1];
            IsCustomerUserEditting = true;
            // Lưu trạng thái của các nút Contact
            _tempIsContactDeleteButtonEnable = IsContactDeleteButtonEnable;
            _tempIsContactNewButtonEnable = IsContactNewButtonEnable;
            _tempIsContactEditButtonEnable = IsContactEditButtonEnable;
            _tempIsContactEnable = IsContactEnable;
            // Khóa các nút Contact
            IsContactDeleteButtonEnable = false;
            IsContactNewButtonEnable = false;
            IsContactEditButtonEnable = false;
            IsContactEnable = false;
        }
        // Delete Button Click
        public async void bnCustomerDelete()
        {
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
                if (SelectedCustomer != null)
                {
                    await new DeleteDataServices().DeleteCustomerAsync(SelectedCustomer);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    LoadData();
                    SelectedCustomer = null;
                }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------
        #region Button Click Contact
        // Edit Button Click
        public void bnContactEdit()
        {
            // Lưu trữ dữ liệu ban đầu
            _tempContact = Contacts.Select(c => c.Clone()).ToList();
            IsContactUserEditting = true;
            // Lưu trạng thái của các nút Customer
            _tempIsCustomerDeleteButtonEnable = IsCustomerDeleteButtonEnable;
            _tempIsCustomerNewButtonEnable = IsCustomerNewButtonEnable;
            _tempIsCustomerEditButtonEnable = IsCustomerEditButtonEnable;
            _tempIsCustomerEnable = IsCustomerEnable;
            // Deactive các nút Customer
            IsCustomerDeleteButtonEnable = false;
            IsCustomerNewButtonEnable = false;
            IsCustomerEditButtonEnable = false;
            IsCustomerEnable = false;
        }
        // Ok Button Click
        public async void bnContactSave()
        {
            IsContactUserEditting = false;
            // Lưu dữ liêu hiển thị
            ObservableCollection<Contact> temp = new ObservableCollection<Contact>(Contacts);
            foreach (var contact in temp)
            {
                contact.Customer = null;
            }
            // Lưu dữ liệu vào database
            await new SaveDataServices().SaveContactListAsync(temp.ToList());
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Load lại dữ liệu
            LoadContact();
            // Trả lại trạng thái của các nút Customer
            IsCustomerDeleteButtonEnable = _tempIsCustomerDeleteButtonEnable;
            IsCustomerNewButtonEnable = _tempIsCustomerNewButtonEnable;
            IsCustomerEditButtonEnable = _tempIsCustomerEditButtonEnable;
            IsCustomerEnable = _tempIsCustomerEnable;
        }
        // Cancel Button Click
        public void bnContactCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu
            Contacts = new ObservableCollection<Contact>(_tempContact.Select(c => c.Clone()).ToList());
            IsContactUserEditting = false;
            // Trả lại trạng thái của các nút Customer
            IsCustomerDeleteButtonEnable = _tempIsCustomerDeleteButtonEnable;
            IsCustomerNewButtonEnable = _tempIsCustomerNewButtonEnable;
            IsCustomerEditButtonEnable = _tempIsCustomerEditButtonEnable;
            IsCustomerEnable = _tempIsCustomerEnable;
        }
        // New Button Click
        public void bnContactNew()
        {
            _tempContact = Contacts.Select(c => c.Clone()).ToList(); // Lưu trữ dữ liệu ban đầu
            Contacts.Add(new Contact { Name = "Liên hệ ...", CustomerId = SelectedCustomer.Id, Customer = SelectedCustomer , PhoneNumber = "(+84) ", Role = "", Notes = ""});
            NotifyOfPropertyChange(() => Contacts);
            SelectedContact = Contacts[Contacts.Count - 1];
            IsContactUserEditting = true;
            // Lưu trạng thái của các nút Customer
            _tempIsCustomerDeleteButtonEnable = IsCustomerDeleteButtonEnable;
            _tempIsCustomerNewButtonEnable = IsCustomerNewButtonEnable;
            _tempIsCustomerEditButtonEnable = IsCustomerEditButtonEnable;
            _tempIsCustomerEnable = IsCustomerEnable;
            // Deactive các nút Customer
            IsCustomerDeleteButtonEnable = false;
            IsCustomerNewButtonEnable = false;
            IsCustomerEditButtonEnable = false;
            IsCustomerEnable = false;
        }
        // Delete Button Click
        public async void bnContactDelete()
        {
            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "Xác nhận!!",
                TxtMessage = "Bạn có muốn xóa liên hệ này?",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                if (SelectedContact != null)
                {
                    await new DeleteDataServices().DeleteContactAsync(SelectedContact);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    LoadContact();
                    SelectedContact = null;
                }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------
        // Các phương pháp tìm kiếm
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
                    Customers = new ObservableCollection<Customer>(CachesServices.Instance.Customers);
                }
                else
                {
                    Customers = new ObservableCollection<Customer>(CachesServices.Instance.Customers.Where(c => c.Name.ToLower().Contains(SearchCustomer.ToLower())));
                }
            }
        }
        // Tìm kiếm Contact
        private string _searchContact;
        public string SearchContact
        {
            get { return _searchContact; }
            set
            {
                _searchContact = value;
                NotifyOfPropertyChange(() => SearchContact);
                if (string.IsNullOrEmpty(SearchContact))
                {
                    LoadContact();
                }
                else
                {
                    LoadContact();
                    _tempContact = new List<Contact>(Contacts);
                    Contacts = new ObservableCollection<Contact>(_tempContact.Where(c => c.Name.ToLower().Contains(SearchContact.ToLower())));
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
