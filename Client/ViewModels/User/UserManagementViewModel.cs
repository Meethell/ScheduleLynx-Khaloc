using Caliburn.Micro;
using Client.Entities.TicketEntities;
using Client.Entities.UserEntity;
using Client.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static Client.ViewModels.UserManagementViewModel;

namespace Client.ViewModels
{
    public class UserManagementViewModel : Screen
    {
        // Settings

        private MainViewModel _mainViewModel;
        public class AccountUser
        {
            public int Id { get; set; }                // From User
            public string Name { get; set; }           // From User
            public bool IsActivated { get; set; }      // From User

            public int UserRoleId { get; set; }        // From UserRole
            public int RoleId { get; set; }            // From UserRole

            public string Role { get; set; }           // From SystemRole.Name

            // Optionally, add more properties as needed
        }
        private ObservableCollection<AccountUser> _accountUsers;
        public ObservableCollection<AccountUser> AccountUsers
        {
            get => _accountUsers;
            set
            {
                _accountUsers = value;
                NotifyOfPropertyChange(() => AccountUsers);
            }
        }
        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }
        private AccountUser _selectedUser;
        public AccountUser SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                NotifyOfPropertyChange(() => SelectedUser);
                LoadTicket(DateFrom, DateTo);
            }
        }

        // Constructor
        private readonly AccountViewModel _userViewModel;
        public UserManagementViewModel(MainViewModel mainViewModel, AccountViewModel userViewModel)
        {
            // Lấy ngày bắt đầu và kết thúc của tháng hiện tại
            DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = DateFrom.AddMonths(1).AddDays(-1);

            Users = new ObservableCollection<User>();
            _mainViewModel = mainViewModel;
            _userViewModel = userViewModel;
            LoadUsers();
        }

        public async Task LoadUsers()
        {
            try
            {
                var users = CachesServices.Instance.Users;
                var userRoles = CachesServices.Instance.UserRoles;
                var systemRoles = CachesServices.Instance.SystemRoles;

                var accountUsers = from user in users
                                   join userRole in userRoles on user.Id equals userRole.UserId into ur
                                   from userRole in ur.DefaultIfEmpty()
                                   join systemRole in systemRoles on userRole?.RoleId ?? 0 equals systemRole.Id into sr
                                   from systemRole in sr.DefaultIfEmpty()
                                   select new AccountUser
                                   {
                                       Id = user.Id,
                                       Name = user.Name,
                                       IsActivated = user.IsActivated,
                                       UserRoleId = userRole?.Id ?? 0,
                                       RoleId = userRole?.RoleId ?? 0,
                                       Role = systemRole?.Name ?? string.Empty
                                   };

                AccountUsers = new ObservableCollection<AccountUser>(accountUsers);
            }
            catch (Exception ex)
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Lỗi",
                    TxtMessage = ex.Message,
                    IsInformation = true
                };

                _mainViewModel.AppNotificationService.ShowError(ex.Message);

                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);

                _mainViewModel.IsEnable = true;
                _mainViewModel.CloseLoadingScreen();
                return;
            }
        }


        // Buttons

        public async Task New()
        {
            _mainViewModel.IsEnable = false;

            var registerViewModel = new RegisterViewModel(_mainViewModel);
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(registerViewModel);

            if (!registerViewModel.Flag)
            {
                _mainViewModel.IsEnable = true;
                return;
            }

            _mainViewModel.IsEnable = true;

            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();

            Users.Clear();
            await LoadUsers();
        }


        public async Task OnChecked()
        {


            var userDetail = SelectedUser;

            var result = await _mainViewModel.UserAccountService.ChangeActivateAccountStatus(userDetail.Id, true);

            if (!result.Flag)
            {
                _mainViewModel.AppNotificationService.ShowError(result.Message);
                _mainViewModel.IsEnable = true;

                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();

                Users.Clear();
                await LoadUsers();
                return;
            }

            SelectedUser.IsActivated = true;
            _mainViewModel.AppNotificationService.ShowSuccess(result.Message);



            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
        }

        public async Task OnUnChecked()
        {

            var userDetail = SelectedUser;
            var result = await _mainViewModel.UserAccountService.ChangeActivateAccountStatus(userDetail.Id, false);
            if (!result.Flag)
            {
                _mainViewModel.AppNotificationService.ShowError(result.Message);

                Users.Clear();
                await LoadUsers();
                return;
            }
            SelectedUser.IsActivated = false;
            _mainViewModel.AppNotificationService.ShowSuccess(result.Message);

            _mainViewModel.IsEnable = true;

            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();

        }

        public async Task ViewUser()
        {
            _mainViewModel.IsEnable = false;
            if (SelectedUser == null)
            {
                _mainViewModel.AppNotificationService.ShowError("Vui lòng chọn người dùng để chỉnh sửa");
                return;
            }
            // Lấy User của SelectedUser
            var user = CachesServices.Instance.Users.FirstOrDefault(u => u.Id == SelectedUser.Id);

            var editAccountViewModel = new EditAccountViewModel(_mainViewModel, user);
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(editAccountViewModel);

            if (!editAccountViewModel.Flag)
            {
                _mainViewModel.IsEnable = true;
                return;
            }
            _mainViewModel.IsEnable = true;

            // Reset CachesServices and reload data
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            await LoadUsers();
        }



        //-------------------------------------------------------------------------------------
        // Ticket
        private DateTime dateFrom;
        public DateTime DateFrom
        {
            get => dateFrom;
            set
            {
                dateFrom = value;
                NotifyOfPropertyChange(() => DateFrom);
            }
        }
        private DateTime dateTo;
        public DateTime DateTo
        {
            get => dateTo;
            set
            {
                dateTo = value;
                NotifyOfPropertyChange(() => DateTo);
            }
        }


        private ObservableCollection<Ticket> tickets;
        public ObservableCollection<Ticket> Tickets
        {
            get => tickets;
            set
            {
                tickets = value;
                NotifyOfPropertyChange(() => Tickets);
            }
        }
        private int ticketCount;
        public int TicketCount
        {
            get => ticketCount;
            set
            {
                ticketCount = value;
                NotifyOfPropertyChange(() => TicketCount);
            }
        }
        private ObservableCollection<Ticket> ticketsOverdue;
        public ObservableCollection<Ticket> TicketsOverdue
        {
            get => ticketsOverdue;
            set
            {
                ticketsOverdue = value;
                NotifyOfPropertyChange(() => TicketsOverdue);
            }
        }
        private int ticketOverdueCount;
        public int TicketOverdueCount
        {
            get => ticketOverdueCount;
            set
            {
                ticketOverdueCount = value;
                NotifyOfPropertyChange(() => TicketOverdueCount);
            }
        }
        private ObservableCollection<Ticket> ticketsPending;
        public ObservableCollection<Ticket> TicketsPending
        {
            get => ticketsPending;
            set
            {
                ticketsPending = value;
                NotifyOfPropertyChange(() => TicketsPending);
            }
        }
        private int ticketPendingCount;
        public int TicketPendingCount
        {
            get => ticketPendingCount;
            set
            {
                ticketPendingCount = value;
                NotifyOfPropertyChange(() => TicketPendingCount);
            }
        }
        private ObservableCollection<Ticket> ticketsMaintenance;
        public ObservableCollection<Ticket> TicketsMaintenance
        {
            get => ticketsMaintenance;
            set
            {
                ticketsMaintenance = value;
                NotifyOfPropertyChange(() => TicketsMaintenance);
            }
        }
        private int ticketMaintenanceCount;
        public int TicketMaintenanceCount
        {
            get => ticketMaintenanceCount;
            set
            {
                ticketMaintenanceCount = value;
                NotifyOfPropertyChange(() => TicketMaintenanceCount);
            }
        }
        private ObservableCollection<Ticket> ticketsRepair;
        public ObservableCollection<Ticket> TicketsRepair
        {
            get => ticketsRepair;
            set
            {
                ticketsRepair = value;
                NotifyOfPropertyChange(() => TicketsRepair);
            }
        }
        private int ticketRepairCount;
        public int TicketRepairCount
        {
            get => ticketRepairCount;
            set
            {
                ticketRepairCount = value;
                NotifyOfPropertyChange(() => TicketRepairCount);
            }
        }
        private ObservableCollection<Ticket> ticketsInstall;
        public ObservableCollection<Ticket> TicketsInstall
        {
            get => ticketsInstall;
            set
            {
                ticketsInstall = value;
                NotifyOfPropertyChange(() => TicketsInstall);
            }
        }
        private int ticketInstallCount;
        public int TicketInstallCount
        {
            get => ticketInstallCount;
            set
            {
                ticketInstallCount = value;
                NotifyOfPropertyChange(() => TicketInstallCount);
            }
        }
        public async Task LoadTicket(DateTime dateFrom, DateTime dateTo)
        {
            if (SelectedUser == null)
            {
                Tickets = new ObservableCollection<Ticket>();
                return;
            }
            try
            {
                
                var userId = SelectedUser.Id;
                var tickets = await new SaveDataServices().GetByUserIdFromTo(userId, dateFrom, dateTo);
                if (tickets == null || !tickets.Any())
                {
                    _mainViewModel.AppNotificationService.ShowInformation("Không có dữ liệu");
                    Tickets = new ObservableCollection<Ticket>();
                    return;
                }
                Tickets = new ObservableCollection<Ticket>(tickets);
                TicketCount = Tickets.Count;
                TicketsOverdue = new ObservableCollection<Ticket>(Tickets.Where(t => t.Overdue));
                TicketOverdueCount = TicketsOverdue.Count;
                TicketsPending = new ObservableCollection<Ticket>(Tickets.Where(t => t.Status.Status != "Đã đóng" && t.Status.Status != "Đã hủy"));
                TicketPendingCount = TicketsPending.Count;
                TicketsMaintenance = new ObservableCollection<Ticket>(Tickets.Where(t => t.Type.Type == "Bảo trì định kỳ"));
                TicketMaintenanceCount = TicketsMaintenance.Count;
                TicketsRepair = new ObservableCollection<Ticket>(Tickets.Where(t => t.Type.Type != "Bảo trì định kỳ" && t.Type.Type != "Lắp đặt mới"));
                TicketRepairCount = TicketsRepair.Count;
                TicketsInstall = new ObservableCollection<Ticket>(Tickets.Where(t => t.Type.Type == "Lắp đặt mới"));
                TicketInstallCount = TicketsInstall.Count;
                

            }
            catch (Exception ex)
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Lỗi",
                    TxtMessage = ex.Message,
                    IsInformation = true
                };
                _mainViewModel.AppNotificationService.ShowError(ex.Message);
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            }
        }
        // Xem chi tiết ticket
        public async void OpenTicketDetail(RoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement fe)
            {
                if (fe.DataContext is Ticket ticket)
                {
                    await _mainViewModel.OpenTicketDetailView(CachesServices.Instance.Tickets.Find(x => x.Id == ticket.Id));
                }
            }
        }

        public async Task QuerySparePartStatistics()
        {
            if (SelectedUser == null)
            {
                _mainViewModel.AppNotificationService.ShowError("Vui lòng chọn người dùng để xem thống kê");
                return;
            }
            await LoadTicket(DateFrom, DateTo);
        }
    }
}
