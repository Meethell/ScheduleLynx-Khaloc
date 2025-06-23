using Caliburn.Micro;
using Client.Entities.TicketEntities;
using Client.Services;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels
{
    public class IssueDetailViewModel : Screen
    {
        // Setting Properties
        private bool _isUserEditting = false;
        public bool IsUserEditting
        {
            get => _isUserEditting;
            set
            {
                _isUserEditting = value;
                NotifyOfPropertyChange(() => IsUserEditting);
                IsReadOnly = !value;
            }
        }
        private bool _isReadOnly = true;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                NotifyOfPropertyChange(() => IsReadOnly);
            }
        }
        // Test properties
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
        private Issue _issue;
        public Issue Issue
        {
            get => _issue;
            set
            {
                _issue = value;
                NotifyOfPropertyChange(() => Issue);
            }
        }
        private bool _isDeleteVisible = false;
        public bool IsDeleteVisible
        {
            get => _isDeleteVisible;
            set
            {
                _isDeleteVisible = value;
                NotifyOfPropertyChange(() => IsDeleteVisible);
            }
        }
        // Constructor
        private readonly MainViewModel _mainViewModel;
        private readonly IssueViewModel _issueViewModel;
        public IssueDetailViewModel(MainViewModel mainViewModel, IssueViewModel issueViewModel, Issue issue)
        {
            // Kiểm tra User Role trong CacheServices để gán visible cho nút delete
            IsDeleteVisible = CachesServices.SystemRole?.Name == "Admin";

            _issueViewModel = issueViewModel;
            _mainViewModel = mainViewModel;
            Issue = issue;
            Tickets = new ObservableCollection<Ticket>(CachesServices.Instance.Tickets.Where(x => x.IssueId == Issue.Id).OrderByDescending(x => x.DateCreated));
        }

        private Issue TempIssue { get; set; }
        // Button functions
        public void EditIssue()
        {
            // Tạo bản sao cho Issue
            TempIssue = new Issue
            {
                Id = Issue.Id,
                Title = Issue.Title,
                ModelId = Issue.ModelId,
                Model = Issue.Model,
                IssueDescription = Issue.IssueDescription,
                IssueSolution = Issue.IssueSolution,
                IssueCause = Issue.IssueCause
            };
            IsUserEditting = true;
        }
        public async Task bnSave()
        {
            // Kiểm tra Title
            if (string.IsNullOrWhiteSpace(Issue.Title))
            {
                // Hiển thị thông báo lỗi
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Lỗi",
                    TxtMessage = "Chưa điền tiêu đề cho Issue",
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return;
            }
            else
            {
                // Lưu Issue vào database
                Issue.Model = null;
                // Lưu vào database
                await new SaveDataServices().SaveIssueAsync(Issue);
                // Cập nhật lại Cache
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                IsUserEditting = false;
            }
        }

        public void bnCancel()
        {
            Issue = TempIssue;
            IsUserEditting = false;
        }
        // Open Ticket
        public async Task OpenTicket(RoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement fe)
            {
                if (fe.DataContext is Ticket ticket)
                {
                    await _mainViewModel.OpenTicketDetailView(ticket);
                }
            }
        }

        // Close Issue View
        public async Task CloseIssue()
        {
            await _issueViewModel.IssueManagementView();
        }

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

        public async Task DeleteIssueCommand()
        {
            // Kiểm tra Issue có Ticket không
            if (Tickets.Count > 0)
            {
                // Hiển thị thông báo lỗi
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Lỗi",
                    TxtMessage = "Không thể xóa Issue này vì nó có Ticket liên quan",
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return;
            }
            else
            {
                AppNotificationService notificationService = new AppNotificationService();
                // Show toast nofication
                notificationService.ShowWarning("THẬN TRỌNG!!!");
                // Bật hội thoại xác nhận
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "XÁC NHẬN!!",
                    TxtMessage = "Bạn có chắc chắn muốn xóa Issue này không?",
                    IsConfirmation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
                {
                    // Xóa Issue khỏi database
                    await new DeleteDataServices().DeleteIssueAsync(Issue);
                    // Cập nhật lại Cache
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    // Quay về trang quản lý Issue
                    await _issueViewModel.IssueManagementView();
                }
            }
        }
    }
}
