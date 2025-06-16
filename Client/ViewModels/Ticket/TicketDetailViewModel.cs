using Caliburn.Micro;
using Client.AppEntities;
using Client.Entities.CustomerEntities;
using Client.Entities.ExpenseEntities;
using Client.Entities.ModelEntities;
using Client.Entities.TicketEntities;
using Client.Entities.UserEntity;
using Client.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels
{
    public class TicketDetailViewModel : Screen
    {
        // Button Settings
        private bool _isDeleteVisible = false;
        public bool IsDeleteVisible
        {
            get { return _isDeleteVisible; }
            set
            {
                _isDeleteVisible = value;
                NotifyOfPropertyChange(() => IsDeleteVisible);
            }
        }
        private bool _isStartProgressVisible;
        public bool IsStartProgressVisible
        {
            get { return _isStartProgressVisible; }
            set
            {
                _isStartProgressVisible = value;
                NotifyOfPropertyChange(() => IsStartProgressVisible);
            }
        }
        private bool _isResolvedVisible;
        public bool IsResolvedVisible
        {
            get { return _isResolvedVisible; }
            set
            {
                _isResolvedVisible = value;
                NotifyOfPropertyChange(() => IsResolvedVisible);
            }
        }
        private bool _isCancelledVisible;
        public bool IsCancelledVisible
        {
            get { return _isCancelledVisible; }
            set
            {
                _isCancelledVisible = value;
                NotifyOfPropertyChange(() => IsCancelledVisible);
            }
        }
        private bool _isReopenVisible;
        public bool IsReopenVisible
        {
            get { return _isReopenVisible; }
            set
            {
                _isReopenVisible = value;
                NotifyOfPropertyChange(() => IsReopenVisible);
            }
        }
        // Properties
        private ObservableCollection<TicketSpareParts> _ticketSpareParts;
        public ObservableCollection<TicketSpareParts> TicketSpareParts
        {
            get { return _ticketSpareParts; }
            set
            {
                _ticketSpareParts = value;
                NotifyOfPropertyChange(() => TicketSpareParts);
            }
        }
        private TicketSpareParts _selectedTicketSparePart;
        public TicketSpareParts SelectedTicketSparePart
        {
            get { return _selectedTicketSparePart; }
            set
            {
                _selectedTicketSparePart = value;
                NotifyOfPropertyChange(() => SelectedTicketSparePart);
            }
        }
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
        private bool _isResolveEnable = false;
        public bool IsResolveEnable
        {
            get { return _isResolveEnable; }
            set
            {
                _isResolveEnable = value;
                NotifyOfPropertyChange(() => IsResolveEnable);
            }
        }
        private ObservableCollection<TicketFile> _ticketFiles;
        public ObservableCollection<TicketFile> TicketFiles
        {
            get { return _ticketFiles; }
            set
            {
                _ticketFiles = value;
                NotifyOfPropertyChange(() => TicketFiles);
                if (_ticketFiles != null && _ticketFiles.Count > 0)
                {
                    IsResolveEnable = true; // Enable resolve button if there are files
                }
                else
                {
                    IsResolveEnable = false; // Disable resolve button if no files
                }
            }
        }
        private TicketFile _selectedTicketFile;
        public TicketFile SelectedTicketFile
        {
            get { return _selectedTicketFile; }
            set
            {
                _selectedTicketFile = value;
                NotifyOfPropertyChange(() => SelectedTicketFile);
            }
        }
        private Ticket _selectedRelatedTicket;
        public Ticket SelectedRelatedTicket
        {
            get { return _selectedRelatedTicket; }
            set
            {
                _selectedRelatedTicket = value;
                NotifyOfPropertyChange(() => SelectedRelatedTicket);
            }
        }
        private ObservableCollection<Contact> _contacts;
        public ObservableCollection<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                NotifyOfPropertyChange(() => Contacts);
            }
        }
        public ObservableCollection<Ticket> _relatedTicket;
        public ObservableCollection<Ticket> RelatedTickets
        {
            get { return _relatedTicket; }
            set
            {
                _relatedTicket = value;
                NotifyOfPropertyChange(() => RelatedTickets);
            }
        }
        private ObservableCollection<PickUser> _pickUsers;
        public ObservableCollection<PickUser> PickUsers
        {
            get { return _pickUsers; }
            set
            {
                _pickUsers = value;
                NotifyOfPropertyChange(() => PickUsers);
            }
        }
        private ObservableCollection<User> _assignUsers;
        public ObservableCollection<User> AssignUsers
        {
            get { return _assignUsers; }
            set
            {
                _assignUsers = value;
                NotifyOfPropertyChange(() => AssignUsers);
            }
        }
        private bool _isPickUsersVisible = false;
        public bool IsPickUsersVisible
        {
            get { return _isPickUsersVisible; }
            set
            {
                _isPickUsersVisible = value;
                NotifyOfPropertyChange(() => IsPickUsersVisible);
            }
        }
        // Constructor
        private readonly TicketViewModel _ticketViewModel;
        private readonly MainViewModel _mainViewModel;
        public TicketDetailViewModel(Ticket ticket, TicketViewModel ticketViewModel, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _ticketViewModel = ticketViewModel;
            Ticket = ticket;
            // Get the contacts for the ticket
            Contacts = new ObservableCollection<Contact>(CachesServices.Instance.Contacts.Where(x => x.CustomerId == Ticket.Instrument.CustomerId));
            // Get the related tickets for the ticket
            RelatedTickets = new ObservableCollection<Ticket>(CachesServices.Instance.Tickets.Where(x => x.InstrumentId == Ticket.InstrumentId));
            // Set Expense properties
            IsExpenseUserEditting = false;
            IsExpenseReadOnly = true;
            IsExpenseEnable = true;
            IsExpenseEditButtonEnable = false;
            IsExpenseDeleteButtonEnable = false;
            IsExpenseNewButtonEnable = true;
            IsExpenseVisible = true;

            if (CachesServices.SystemRole.Name == "Admin")
            {
                IsDeleteVisible = true; // Show delete button for Admin
            }
            else
            {
                IsDeleteVisible = false; // Hide delete button for other roles
            }

            // Set button visibility
            switch (Ticket.Status.Status)
            {
                case "Mới":
                    IsStartProgressVisible = true;
                    IsResolvedVisible = false;
                    IsCancelledVisible = true;
                    IsReopenVisible = false;
                    break;
                case "Đang xử lý":
                    IsResolvedVisible = true;
                    IsCancelledVisible = true;
                    IsReopenVisible = false;
                    IsStartProgressVisible = false;
                    break;
                case "Đã đóng":
                    if (CachesServices.SystemRole.Name == "Admin" || CachesServices.SystemRole
                        .Name == "Manager")
                    {
                        IsReopenVisible = true;
                        IsCancelledVisible = false;
                        IsResolvedVisible = false;
                        IsStartProgressVisible = false;
                    }
                    else
                    {
                        IsReopenVisible = false;
                        IsCancelledVisible = false;
                        IsResolvedVisible = false;
                        IsStartProgressVisible = false;
                    }
                    break;
                case "Đã hủy":
                    if (CachesServices.SystemRole.Name == "Admin" || CachesServices.SystemRole
                        .Name == "Manager")
                    {
                        IsReopenVisible = true;
                        IsCancelledVisible = false;
                        IsResolvedVisible = false;
                        IsStartProgressVisible = false;
                    }
                    else
                    {
                        IsReopenVisible = false;
                        IsCancelledVisible = false;
                        IsResolvedVisible = false;
                        IsStartProgressVisible = false;
                    }
                    break;
                default:
                    break;
            }

            // Set the visibility of the pick users
            if (CachesServices.SystemRole.Name == "Admin" || CachesServices.SystemRole.Name == "Manager")
            {
                IsPickUsersVisible = true;
            }
            else
            {
                IsPickUsersVisible = false;
            }
            // Get the ticket files
            GetTicketFiles();
            GetUserData();
            // Get the ticket spare parts
            GetTicketSpareParts();
            LoadExpense();

        }
        // Methods
        public async Task DeleteTicketCommand()
        {
            AppNotificationService notificationService = new AppNotificationService();

            // Show toast nofication
            notificationService.ShowWarning("THẬN TRỌNG!!!");

            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "XÁC NHẬN!!",
                TxtMessage = "Bạn có chắc chắn muốn xóa Thẻ này không?",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                // Xóa instrument đang chọn trong database
                await new DeleteDataServices().DeleteTicketAsync(Ticket);

                // Kiểm tra các spare parts có liên quan đến ticket này
                var ticketSpareParts = CachesServices.Instance.TicketSpareParts.Where(x => x.TicketId == Ticket.Id).ToList();
                // Xóa các spare parts liên quan đến ticket này
                foreach (var sparePart in ticketSpareParts)
                {
                    await new DeleteDataServices().DeleteTicketSparePartAsync(sparePart);
                    // Tăng số lượng spare part trong kho
                    await new SaveDataServices().IncreaseSparePartQuantity(sparePart.SparePartId, sparePart.Quantity);
                }
                // Xóa các file liên quan đến ticket này
                var ticketFiles = CachesServices.Instance.TicketFiles.Where(x => x.TicketId == Ticket.Id).ToList();
                foreach (var file in ticketFiles)
                {
                    await new DeleteDataServices().DeleteTicketFileAsync(file);
                    // Xóa file trên ổ đĩa
                    new FileServices().DeleteFile(file.FilePath);
                }
                // Xóa các tag liên quan đến ticket này
                var ticketTags = CachesServices.Instance.TicketTags.Where(x => x.TicketId == Ticket.Id).ToList();
                foreach (var tag in ticketTags)
                {
                    await new DeleteDataServices().DeleteTicketTagAsync(tag);
                }
                // Xóa các expense liên quan đến ticket này
                var ticketExpenses = CachesServices.Instance.Expenses.Where(x => x.TicketId == Ticket.Id).ToList();
                foreach (var expense in ticketExpenses)
                {
                    await new DeleteDataServices().DeleteExpenseAsync(expense);
                }


                // Show toast nofication
                notificationService.ShowSuccess("Đã xóa thành công");
                // Câp nhật cache
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                // Load lại danh sách instrument
                await CloseDetailView();
            }
        }
        public async Task CloseDetailView()
        {
            // Mở lại danh sách Ticket
            _ticketViewModel.IsTicketDetailChecked = false;
            await _ticketViewModel.TicketManagementView();
        }
        public async Task GetTicketSpareParts()
        {
            TicketSpareParts = new ObservableCollection<TicketSpareParts>(CachesServices.Instance.TicketSpareParts.Where(x => x.TicketId == Ticket.Id));
        }
        public async Task AddSparePart()
        {
            _mainViewModel.IsEnable = false;

            var pickSparePart = new PickSparePartViewModel(_mainViewModel);
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(pickSparePart);

            if (pickSparePart.SelectedSparePart != null && pickSparePart.Flag)
            {
                // Check if the spare part is already assigned to the ticket
                if (TicketSpareParts.Any(x => x.SparePartId == pickSparePart.SelectedSparePart.Id))
                {
                    // Show custom message box
                    var customMessageBoxViewModel = new CustomMessageBoxViewModel
                    {
                        Message = "Lỗi",
                        TxtMessage = "Phụ tùng đã được thêm vào Thẻ này",
                        IsInformation = true
                    };
                    await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                    _mainViewModel.IsEnable = true;
                    return;
                }
                // Add the spare part to the ticket
                var item = new TicketSpareParts
                {
                    SparePartId = pickSparePart.SelectedSparePart.Id,
                    SparePart = null,
                    SerialNumber = pickSparePart.SerialNumber,
                    TicketId = Ticket.Id,
                    Quantity = pickSparePart.QuantityInt
                };
                // Save the ticket spare part to the database
                await new SaveDataServices().SaveTicketSparePartsAsync(item);
                await new SaveDataServices().ReduceSparePartQuantity(pickSparePart.SelectedSparePart.Id, pickSparePart.QuantityInt);
                // Update Caches
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                // Update the view
                await GetTicketSpareParts();

                _mainViewModel.IsEnable = true;
            }

            _mainViewModel.IsEnable = true;
        }

        public async Task DeleteSparePartCommand(TicketSpareParts ticketSpareParts)
        {
            if (ticketSpareParts != null)
            {
                // Show confirmation dialog
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Xác nhận",
                    TxtMessage = "Bạn có chắc chắn muốn xóa phụ tùng này khỏi thẻ không?",
                    IsConfirmation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                if (customMessageBoxViewModel.DialogResult == System.Windows.MessageBoxResult.Yes)
                {
                    // Delete the spare part from the ticket
                    await new DeleteDataServices().DeleteTicketSparePartAsync(ticketSpareParts);
                    await new SaveDataServices().IncreaseSparePartQuantity(ticketSpareParts.SparePartId, ticketSpareParts.Quantity);
                    // Update Caches
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    // Update the view
                    await GetTicketSpareParts();
                }
            }
        }
        // Get User Data
        public async Task GetUserData()
        {
            //-----------------------------------------------------------------------------------
            // Get Assign users
            try { AssignUsers = new ObservableCollection<User>(await new SaveDataServices().GetAllUserByEntityId(Ticket.Id)); }
            catch (System.Exception ex)
            {
                // Show custom message box
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Lỗi",
                    TxtMessage = ex.Message,
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                _mainViewModel.IsEnable = true;
                _mainViewModel.CloseLoadingScreen();
                return;
            }

            // Get the pick users
            PickUsers = new ObservableCollection<PickUser>();
            foreach (var user in CachesServices.Instance.Users)
            {
                // Only add users who are activated
                if (user.IsActivated)
                {
                    PickUsers.Add(new PickUser
                    {
                        Id = user.Id,
                        Name = user.Name,
                        IsSelected = false
                    });
                    // Check if the user is selected
                    if (AssignUsers.Any(x => x.Id == user.Id))
                    {
                        PickUsers.Last().IsSelected = true;
                    }
                }
            }

            // Lắng nghe sự kiện thay đổi của User.IsSelected
            foreach (var user in PickUsers)
            {
                user.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "IsSelected")
                    {
                        var changedUser = sender as PickUser;
                        if (changedUser != null)
                        {
                            UpdateAssignedUsers(changedUser);
                        }
                    }
                };
            }
        }
        // Update Assigned Users
        private async Task UpdateAssignedUsers(PickUser changedUser)
        {
            _mainViewModel.IsEnable = false;
            await _mainViewModel.ShowLoadingScreen();

            var user = PickUsers.FirstOrDefault(u => u.Id == changedUser.Id);
            if (user != null)
            {
                if (user.IsSelected)
                {
                    // Add user to the assign users
                    AssignUsers.Add(new User
                    {
                        Id = user.Id,
                        Name = user.Name,
                    });
                    // Save the assign user to the database
                    var ticketTag = new TicketTag
                    {
                        TicketId = Ticket.Id,
                        UserId = user.Id
                    };
                    var result = await new SaveDataServices().AddTicketTag(ticketTag);

                    if (!result.Flag)
                    {
                        // Show custom message box
                        var customMessageBoxViewModel = new CustomMessageBoxViewModel
                        {
                            Message = "Lỗi",
                            TxtMessage = result.Message,
                            IsInformation = true
                        };
                        var windowManager = new WindowManager();
                        await windowManager.ShowDialogAsync(customMessageBoxViewModel);

                        _mainViewModel.IsEnable = true;
                        _mainViewModel.CloseLoadingScreen();
                        return;
                    }

                    _mainViewModel.AppNotificationService.ShowSuccess("Thêm người dùng thành công");
                }
                else
                {
                    // Remove user from the assign users
                    var userToRemove = AssignUsers.FirstOrDefault(x => x.Id == user.Id);
                    if (userToRemove != null)
                    {
                        AssignUsers.Remove(userToRemove);
                    }
                    // Remove the assign user from the database
                    var result = await new SaveDataServices().DeleteTicketTagById(user.Id, Ticket.Id);

                    if (!result.Flag)
                    {
                        // Show custom message box
                        var customMessageBoxViewModel = new CustomMessageBoxViewModel
                        {
                            Message = "Lỗi",
                            TxtMessage = result.Message,
                            IsInformation = true
                        };
                        var windowManager = new WindowManager();
                        await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                        _mainViewModel.IsEnable = true;
                        _mainViewModel.CloseLoadingScreen();
                        return;
                    }

                    _mainViewModel.AppNotificationService.ShowSuccess("Bỏ chọn người dùng thành công");
                }
            }
            // Update Caches
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Update the view
            _ticketViewModel.IsTicketDetailChecked = false;
            await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.Id == Ticket.Id));
            // Close loading screen
            _mainViewModel.IsEnable = true;
            _mainViewModel.CloseLoadingScreen();
        }
        // Get Ticket Files
        public void GetTicketFiles()
        {
            TicketFiles = new ObservableCollection<TicketFile>(CachesServices.Instance.TicketFiles.Where(x => x.TicketId == Ticket.Id));
        }

        // Edit Ticket
        public async Task EditTicket()
        {
            await _ticketViewModel.EditTicketView(CachesServices.Instance.Tickets.Find(x => x.Id == Ticket.Id));
        }
        // Start Progress
        public async Task StartProgress()
        {
            Ticket.Status = CachesServices.Instance.TicketStatuses[1];
            Ticket.TicketStatusId = Ticket.Status.Id;
            Ticket.DateUpdated = System.DateTime.Now;
            Ticket.Status = null;
            Ticket.Instrument = null;
            Ticket.Issue = null;
            Ticket.Priority = null;
            Ticket.Type = null;
            // Save the ticket to the database
            await new SaveDataServices().SaveTicketAsync(Ticket);
            // Update Caches
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Update the view
            _ticketViewModel.IsTicketDetailChecked = false;
            await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.Id == Ticket.Id));
        }
        // Resolve Ticket
        public async Task ResolveTicket()
        {
            Ticket.Status = CachesServices.Instance.TicketStatuses[2];
            Ticket.TicketStatusId = Ticket.Status.Id;
            Ticket.DateUpdated = System.DateTime.Now;
            Ticket.Status = null;
            Ticket.Instrument = null;
            Ticket.Issue = null;
            Ticket.Priority = null;
            Ticket.Type = null;
            // Save the ticket to the database
            await new SaveDataServices().SaveTicketAsync(Ticket);
            // Update Caches
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Update the view
            _ticketViewModel.IsTicketDetailChecked = false;
            await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.Id == Ticket.Id));
        }
        // Reopen Ticket
        public async Task ReopenTicket()
        {
            Ticket.Status = CachesServices.Instance.TicketStatuses[0];
            Ticket.TicketStatusId = Ticket.Status.Id;
            Ticket.DateUpdated = System.DateTime.Now;
            Ticket.Status = null;
            Ticket.Instrument = null;
            Ticket.Issue = null;
            Ticket.Priority = null;
            Ticket.Type = null;
            // Save the ticket to the database
            await new SaveDataServices().SaveTicketAsync(Ticket);
            // Update Caches
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Update the view
            _ticketViewModel.IsTicketDetailChecked = false;
            await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.Id == Ticket.Id));
        }
        // Cancel Ticket
        public async Task CancelTicket()
        {
            Ticket.Status = CachesServices.Instance.TicketStatuses[3];
            Ticket.TicketStatusId = Ticket.Status.Id;
            Ticket.DateUpdated = System.DateTime.Now;
            Ticket.Status = null;
            Ticket.Instrument = null;
            Ticket.Issue = null;
            Ticket.Priority = null;
            Ticket.Type = null;
            // Save the ticket to the database
            await new SaveDataServices().SaveTicketAsync(Ticket);
            // Update Caches
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Update the view
            _ticketViewModel.IsTicketDetailChecked = false;
            await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.Id == Ticket.Id));
        }
        // Open Related Ticket
        public async Task OpenTicketDetail()
        {
            await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.Id == SelectedRelatedTicket.Id));
        }
        // UploadFile
        public async Task UploadFile()
        {
            TicketFile ticketFile = new FileServices().OpenTicketFile();
            if (ticketFile != null)
            {
                // Kiểm tra file đã tồn tại chưa
                if (CachesServices.Instance.TicketFiles.Any(x => x.SafeFileName == ticketFile.SafeFileName))
                {
                    // Show custom message box
                    // Thông báo lỗi
                    var customMessageBoxViewModel = new CustomMessageBoxViewModel
                    {
                        Message = "Lỗi",
                        TxtMessage = "File đã tồn tại",
                        IsInformation = true
                    };
                    var windowManager = new WindowManager();
                    await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                    return;
                }
                // Save Ticket File
                ticketFile.TicketId = Ticket.Id;
                ticketFile.Ticket = null;
                var result = await Task.Run(() => new FileServices().SaveTicketFile(ticketFile));

                if (result == false)
                {
                    // Show custom message box
                    var customMessageBoxViewModel = new CustomMessageBoxViewModel
                    {
                        Message = "Lỗi",
                        TxtMessage = "Không thể tải lên file",
                        IsInformation = true
                    };
                    var windowManager = new WindowManager();
                    await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                    return;
                }
                // Toast notification
                new AppNotificationService().ShowSuccess("Tải lên file thành công");

                // Update Caches
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                // Update the view
                await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.Id == Ticket.Id));
            }
        }
        // OpenFile
        public void OpenFile()
        {
            new FileServices().OpenFile(SelectedTicketFile.FilePath);
        }
        public async Task DeleteFile()
        {
            await new DeleteDataServices().DeleteTicketFileAsync(SelectedTicketFile);
            // Update Caches
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Update the view
            GetTicketFiles();
        }

        // OpenInstrumentDetail
        public async Task OpenInstrumentDetail()
        {
            await _mainViewModel.OpenInstrumentDetailView(Ticket.Instrument);
        }

        // Open Issue Detail
        public async Task OpenIssueDetail()
        {
            await _mainViewModel.OpenIssueDetailView(Ticket.Issue);
        }

        //------------------------------------------------------------------------------------------------------------
        //Setting Properties
        #region Expense Setting Properties
        // Expense properties
        private List<Expense> _tempExpense; // Dùng để lưu trữ dữ liệu khi người dùng editting

        private ObservableCollection<Expense> _Expenses; // Dùng để hiển thị dữ liệu
        public ObservableCollection<Expense> Expenses
        {
            get { return _Expenses; }
            set
            {
                _Expenses = value;
                NotifyOfPropertyChange(() => Expenses);
                IsExpenseEditButtonEnable = Expenses.Count > 0;
            }
        }

        private Expense _selectedExpense;
        public Expense SelectedExpense
        {
            get { return _selectedExpense; }
            set
            {
                _selectedExpense = value;
                NotifyOfPropertyChange(() => SelectedExpense);

                SetExpenseDeleteButtonEnable();

            }
        }
        // IsExpenseUserEditting - KEY - Để xác định xem người dùng đang editting hay không
        // - Gía trị ngươc lại với IsExpenseReadOnly
        // - Cho phép nút Ok và Cancel hiện lên khi IsExpenseUserEditting = true
        private bool _isExpenseUserEditting;
        public bool IsExpenseUserEditting
        {
            get { return _isExpenseUserEditting; }
            set
            {
                _isExpenseUserEditting = value;
                NotifyOfPropertyChange(() => IsExpenseUserEditting);
                IsExpenseReadOnly = !value;
                IsExpenseNewButtonEnable = !value;
                SetExpenseDeleteButtonEnable();
            }
        }
        // IsExpenseReadOnly - Để xác định xem người dùng có thể chỉnh sửa ExpenseDataGrid hay không
        // - Cho phép nút Edit hiện lên khi IsExpenseReadOnly = true
        private bool _isExpenseReadOnly;
        public bool IsExpenseReadOnly
        {
            get { return _isExpenseReadOnly; }
            set
            {
                _isExpenseReadOnly = value;
                NotifyOfPropertyChange(() => IsExpenseReadOnly);

            }
        }
        // IsExpenseEnable - Để xác định xem người dùng có thể chỉnh sửa ExpenseDataGrid hay không
        private bool _isExpenseEnable;
        public bool IsExpenseEnable
        {
            get { return _isExpenseEnable; }
            set
            {
                _isExpenseEnable = value;
                NotifyOfPropertyChange(() => IsExpenseEnable);
            }
        }

        // Button Properties

        // IsExpenseEditButtonEnable - Để xác định xem nút Edit có được enable hay không
        // - Nếu ExpenseDataGrid không có dữ liệu thì nút Edit sẽ không được enable
        private bool _isExpenseEditButtonEnable;
        public bool IsExpenseEditButtonEnable
        {
            get { return _isExpenseEditButtonEnable; }
            set
            {
                _isExpenseEditButtonEnable = value;
                NotifyOfPropertyChange(() => IsExpenseEditButtonEnable);
            }
        }
        // IsExpenseDeleteButtonEnable - Để xác định xem nút Delete có được enable hay không
        // - Nếu SelectedExpense = null thì nút Delete sẽ không được enable
        private bool _isExpenseDeleteButtonEnable;
        public bool IsExpenseDeleteButtonEnable
        {
            get { return _isExpenseDeleteButtonEnable; }
            set
            {
                _isExpenseDeleteButtonEnable = value;
                NotifyOfPropertyChange(() => IsExpenseDeleteButtonEnable);
            }
        }
        // IsExpenseNewButtonEnable - Để xác định xem nút New có được enable hay không
        // - Nếu IsExpenseUserEditting = true thì nút New sẽ không được enable
        private bool _isExpenseNewButtonEnable;
        public bool IsExpenseNewButtonEnable
        {
            get { return _isExpenseNewButtonEnable; }
            set
            {
                _isExpenseNewButtonEnable = value;
                NotifyOfPropertyChange(() => IsExpenseNewButtonEnable);
            }
        }

        private bool _isExpenseVisible;
        public bool IsExpenseVisible
        {
            get { return _isExpenseVisible; }
            set
            {
                _isExpenseVisible = value;
                NotifyOfPropertyChange(() => IsExpenseVisible);
            }
        }


        #endregion

        public void SetExpenseDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedExpense != null và IsExpenseUserEditting = false
            IsExpenseDeleteButtonEnable = SelectedExpense != null && !IsExpenseUserEditting;
        }

        // Button Click
        //-------------------------------------------------------------------------------------
        #region Button Click Expense
        // Edit Button Click
        public void bnExpenseEdit()
        {
            // Lưu trữ dữ liệu ban đầu
            _tempExpense = Expenses.Select(c => c.Clone()).ToList();
            IsExpenseUserEditting = true;
        }
        // Ok Button Click
        public async void bnExpenseSave()
        {
            // Kiểm tra dữ liệu trước khi lưu
            foreach (var expense in Expenses)
            {
                // Kiểm tra Amount và Total là số thực, Count là số nguyên
                if (expense.Amount < 0 || expense.Count < 0)
                {
                    // Hiển thị thông báo lỗi
                    var customMessageBoxViewModel = new CustomMessageBoxViewModel
                    {
                        Message = "Lỗi dữ liệu",
                        TxtMessage = "Chi phí và số lượng phải là số dương.",
                        IsInformation = true
                    };
                    var windowManager = new WindowManager();
                    await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                    return;
                }
                // Có thể kiểm tra thêm nếu cần
            }

            // Tiếp tục lưu như cũ
            IsExpenseUserEditting = false;
            ObservableCollection<Expense> temp = new ObservableCollection<Expense>(Expenses);
            foreach (var Expense in temp)
            {
                Expense.TicketId = Ticket.Id;
            }
            await new SaveDataServices().SaveExpenseListAsync(temp.ToList());
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            LoadExpense();
        }
        // Cancel Button Click
        public void bnExpenseCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu
            SelectedExpense = null;
            Expenses = new ObservableCollection<Expense>(_tempExpense.Select(c => c.Clone()).ToList());
            IsExpenseUserEditting = false;

        }
        // New Button Click
        public void bnExpenseNew()
        {
            _tempExpense = Expenses.Select(c => c.Clone()).ToList(); ; // Lưu trữ dữ liệu ban đầu
            Expenses.Add(new Expense { Name = "Chi phí ...", Amount = 0, Count = 0, Notes = "" });
            NotifyOfPropertyChange(() => Expenses);
            SelectedExpense = Expenses[Expenses.Count - 1];
            IsExpenseUserEditting = true;
        }
        // Delete Button Click
        public async void bnExpenseDelete()
        {
            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "XÁC NHẬN!!",
                TxtMessage = "Bạn có muốn xóa chi phí này không?",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                if (SelectedExpense != null)
                {
                    await new DeleteDataServices().DeleteExpenseAsync(SelectedExpense);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    LoadExpense();
                    SelectedExpense = null;
                }
            }
        }
        #endregion

        public void LoadExpense()
        {
            // Lấy Expense từ Caches
            Expenses = new ObservableCollection<Expense>(CachesServices.Instance.Expenses.Where(x => x.TicketId == Ticket.Id));
        }
    }
}
