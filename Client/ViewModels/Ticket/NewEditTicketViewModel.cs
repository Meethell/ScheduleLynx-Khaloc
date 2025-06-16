using Caliburn.Micro;
using Client.Entities.TicketEntities;
using Client.Entities.CustomerEntities;
using Client.Entities.ModelEntities;
using Client.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Entities.UserEntity;

namespace Client.ViewModels
{
    public class NewEditTicketViewModel : Screen
    {
        //Full Properties
        // Configure
        private bool _isDateDueEnable;
        public bool IsDateDueEnable
        {
            get { return _isDateDueEnable; }
            set
            {
                _isDateDueEnable = value;
                NotifyOfPropertyChange(() => IsDateDueEnable);
            }
        }
        private bool _isNew;
        public bool IsNew
        {
            get { return _isNew; }
            set
            {
                _isNew = value;
                NotifyOfPropertyChange(() => IsNew);
            }
        }

        private bool _isNewIssueEnabled;
        public bool IsNewIssueEnabled
        {
            get { return _isNewIssueEnabled; }
            set
            {
                _isNewIssueEnabled = value;
                NotifyOfPropertyChange(() => IsNewIssueEnabled);
            }
        }

        // Header
        private string _header;
        public string tbHeader
        {
            get { return _header; }
            set
            {
                _header = value;
                NotifyOfPropertyChange(() => tbHeader);
            }
        }

        //Ticket
        private Ticket _ticket;
        public Ticket Ticket
        {
            get { return _ticket; }
            set
            {
                _ticket = value;
                NotifyOfPropertyChange(() => Ticket);
            }
        }

        //Truy vấn dữ liệu gián tiếp
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
                // Truy vấn dữ liệu cho Instrument
                if (SelectedCustomer != null)
                {
                    Instruments = new ObservableCollection<Instrument>(CachesServices.Instance.Instruments.Where(x => x.CustomerId == SelectedCustomer.Id));
                }
            }
        }
        private string _customerSearchText;
        public string CustomerSearchText
        {
            get { return _customerSearchText; }
            set
            {
                _customerSearchText = value;
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
                Ticket.Instrument = value;
                // Truy vấn dữ liệu cho Issue
                if (SelectedInstrument != null)
                {
                    IsNewIssueEnabled = true;
                    Issues = new ObservableCollection<Issue>(CachesServices.Instance.Issues.Where(x => x.ModelId == SelectedInstrument.ModelId));
                }
                else
                {
                    IsNewIssueEnabled = false;
                }
            }
        }

        private ObservableCollection<Issue> _issues;
        public ObservableCollection<Issue> Issues
        {
            get { return _issues; }
            set
            {
                _issues = value;
                NotifyOfPropertyChange(() => Issues);
            }
        }

        private string _issueSearchText;
        public string IssueSearchText
        {
            get { return _issueSearchText; }
            set
            {
                _issueSearchText = value;
                NotifyOfPropertyChange(() => IssueSearchText);
                // Truy vấn dữ liệu
                if (!string.IsNullOrEmpty(IssueSearchText))
                {
                    // Truy vấn theo SelectedInstrument.ModelId
                    Issues = new ObservableCollection<Issue>(CachesServices.Instance.Issues.Where(x => x.ModelId == Ticket.Instrument.ModelId && x.Title.ToLower().Contains(IssueSearchText.ToLower())));
                }
                else
                {
                    Issues = new ObservableCollection<Issue>(CachesServices.Instance.Issues.Where(x => x.ModelId == Ticket.Instrument.ModelId));
                }
            }
        }

        // ComboBox
        private ObservableCollection<TicketType> _ticketTypes = new ObservableCollection<TicketType>(CachesServices.Instance.TicketTypes);
        public ObservableCollection<TicketType> TicketTypes
        {
            get { return _ticketTypes; }
            set
            {
                _ticketTypes = value;
                NotifyOfPropertyChange(() => TicketTypes);
            }
        }

        private ObservableCollection<TicketServiceType> ticketServiceTypes = new ObservableCollection<TicketServiceType>(CachesServices.Instance.TicketServiceTypes);
        public ObservableCollection<TicketServiceType> TicketServiceTypes
        {
            get { return ticketServiceTypes; }
            set
            {
                ticketServiceTypes = value;
                NotifyOfPropertyChange(() => TicketServiceTypes);
            }
        }

        private ObservableCollection<TicketPriority> _priorities = new ObservableCollection<TicketPriority>(CachesServices.Instance.TicketPriorities);
        public ObservableCollection<TicketPriority> TicketPriorities
        {
            get { return _priorities; }
            set
            {
                _priorities = value;
                NotifyOfPropertyChange(() => TicketPriorities);
            }
        }

        private ObservableCollection<TicketStatus> _ticketStatuses = new ObservableCollection<TicketStatus>(CachesServices.Instance.TicketStatuses);
        public ObservableCollection<TicketStatus> TicketStatuses
        {
            get { return _ticketStatuses; }
            set
            {
                _ticketStatuses = value;
                NotifyOfPropertyChange(() => TicketStatuses);
            }
        }




        //Constructor
        private readonly TicketViewModel _ticketViewModel;
        private readonly MainViewModel _mainViewModel;
        public NewEditTicketViewModel(Ticket ticket, bool isNew, TicketViewModel ticketViewModel, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _ticketViewModel = ticketViewModel;
            Ticket = ticket;
            IsNew = isNew;

            if (isNew)
            {
                Ticket = new Ticket();
                tbHeader = "Tạo Ticket mới - ID: ";
                Ticket.DateCreated = DateTime.Now;
                Ticket.DateDue = DateTime.Now;
                Ticket.DateUpdated = DateTime.Now;
                Ticket.TicketNumber = CachesServices.Instance.NextTicketNumber;
                Ticket.Status = CachesServices.Instance.TicketStatuses[0];

                if(CachesServices.SystemRole.Name == "Admin")
                {
                    IsDateDueEnable = true;
                }
                else
                {
                    IsDateDueEnable = false;
                }

            }
            else
            {
                Ticket = ticket;
                tbHeader = "Chỉnh sửa Ticket - ID: ";
                SelectedInstrument = Ticket.Instrument;
                GetCustomerAndIssueByInstrument();
                if (CachesServices.SystemRole.Name == "Admin")
                {
                    IsDateDueEnable = true;
                }
                else
                {
                    IsDateDueEnable = false;
                }

            }
        }

        // Get customer and Issue by Instrument
        public void GetCustomerAndIssueByInstrument()
        {
            if (Ticket.Instrument != null)
            {
                SelectedCustomer = Customers.FirstOrDefault(x => x.Id == Ticket.Instrument.CustomerId);
                CustomerSearchText = SelectedCustomer.Name;
                IssueSearchText = Ticket.Issue.Title;
            }
        }
        // Save instrument
        public async Task btSave()
        {
            // Kiểm tra Các miền dữ liệu
            if (string.IsNullOrEmpty(Ticket.Title) || SelectedCustomer == null || Ticket.Instrument == null || Ticket.Issue == null || Ticket.Type == null || Ticket.Priority == null)
            {
                // Thông báo lỗi
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Lỗi",
                    TxtMessage = "Các trường chưa được điền đầy đủ!",
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return;
            }
            else
            {
                Ticket.InstrumentId = Ticket.Instrument.Id;
                Ticket.Instrument = null;
                Ticket.TicketTypeId = Ticket.Type.Id;
                Ticket.Type = null;
                Ticket.PriorityId = Ticket.Priority.Id;
                Ticket.Priority = null;
                Ticket.TicketStatusId = Ticket.Status.Id;
                Ticket.Status = null;
                Ticket.IssueId = Ticket.Issue.Id;
                Ticket.Issue = null;
                Ticket.TicketServiceTypeId = Ticket.ServiceType.Id;
                Ticket.ServiceType = null;
                Ticket.DateUpdated = DateTime.Now;

                // So sánh OverdueDate với ngày hiện tại
                if (Ticket.DateDue < DateTime.Now)
                {
                    Ticket.Overdue = true;
                }
                else
                {
                    Ticket.Overdue = false;
                }
                // Lưu vào database
                await new SaveDataServices().SaveTicketAsync(Ticket);
                // Cập nhật lại Cache
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                // Mở lại TicketManagementView
                _ticketViewModel.IsTicketManagementViewChecked = true;
                await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.TicketNumber == Ticket.TicketNumber));
            }
        }
        public async Task btCancel()
        {
            _ticketViewModel.IsTicketManagementViewChecked = true;
            await _ticketViewModel.TicketManagementView();
        }
        public async Task btNewIssue()
        {
            // Mở NewIssueWindow
            var newIssueWindowViewModel = new NewIssueWindowViewModel(Ticket.Instrument.Model);
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(newIssueWindowViewModel);
            // Kiểm tra kết quả trả về
            if (newIssueWindowViewModel.Result)
            {
                // Cập nhật lại Issue
                Issues = new ObservableCollection<Issue>(CachesServices.Instance.Issues.Where(x => x.ModelId == Ticket.Instrument.ModelId));
            }

        }
    }
}
