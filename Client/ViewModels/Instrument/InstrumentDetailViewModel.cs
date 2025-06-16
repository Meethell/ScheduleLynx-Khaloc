using Caliburn.Micro;
using Client.Entities.TicketEntities;
using Client.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Client.Entities.ModelEntities;

namespace Client.ViewModels
{
    public class InstrumentDetailViewModel : Screen
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

        private bool _isIntrumentDetialVisible;
        public bool IsIntrumentDetialVisible
        {
            get { return _isIntrumentDetialVisible; }
            set
            {
                _isIntrumentDetialVisible = value;
                NotifyOfPropertyChange(() => IsIntrumentDetialVisible);
            }
        }
        public void ToggleInstrumentDetail()
        {
            IsIntrumentDetialVisible = !IsIntrumentDetialVisible;
        }
        //--------------------------------------------------------------
        // Constructor
        private readonly InstrumentViewModel _instrumentViewModel;
        private readonly MainViewModel _mainViewModel;
        public InstrumentDetailViewModel(Instrument instrument, InstrumentViewModel instrumentViewModel, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            IsIntrumentDetialVisible = true;
            _instrumentViewModel = instrumentViewModel;
            Instrument = instrument;
            // Hiển thị nút New nếu là Admin
            IsNewButtonVisible = CachesServices.SystemRole?.Name == "Admin";
            LoadTickets();
        }

        // Get tickets by instrument
        public void LoadTickets()
        {
            Tickets = new ObservableCollection<Ticket>(CachesServices.Instance.Tickets.FindAll(x => x.InstrumentId == Instrument.Id));
        }

        // Open the edit tab
        public async Task EditInstrument()
        {
            await _instrumentViewModel.EditInstrumentView(Instrument);
        }
        public async Task DeleteInstrumentCommand()
        {
            AppNotificationService notificationService = new AppNotificationService();

            // Show toast nofication
            notificationService.ShowWarning("THẬN TRỌNG!!!");

            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "XÁC NHẬN!!",
                TxtMessage = "Bạn có chắc chắn muốn xóa Thiết bị này không?\nViệc xóa thiết bị này sẽ xóa hết tất cả thông tin liên quan đến thiết bị, Bao gồm các TICKET liên quan",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                // Xóa instrument đang chọn trong database
                await new DeleteDataServices().DeleteInstrumentAsync(Instrument);

                // Show toast nofication
                notificationService.ShowSuccess("Đã xóa thành công");
                // Câp nhật cache
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                // Load lại danh sách instrument
                await CloseDetailView();
            }
        }
        // Close the tab
        public async Task CloseDetailView()
        {
            _instrumentViewModel.IsInstrumentManagementViewChecked = true;
            await _instrumentViewModel.InstrumentManagementView();
        }

        //--------------------------------- Ticket ---------------------------------
        private ObservableCollection<Ticket> _tickets;
        public ObservableCollection<Ticket> Tickets
        {
            get { return _tickets; }
            set
            {
                _tickets = value;
                NotifyOfPropertyChange(() => Tickets);
            }
        }
        // Thêm mới ticket
        public async void AddTicket()
        {
            var newTicket = new Ticket
            {
                Instrument = Instrument,
                InstrumentId = Instrument.Id
            };
            await _mainViewModel.OpenNewEditTicketView(newTicket, true);
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
    }
}
