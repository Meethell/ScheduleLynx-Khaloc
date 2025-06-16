using Caliburn.Micro;
using Client.DTOs;
using Client.Entities.ModelEntities;
using Client.Entities.TicketEntities;
using Client.Helpers;
using Client.Services;
using Client.Views;
using ClientLibrary.Services.Constracts;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;


namespace Client.ViewModels
{
    public class MainViewModel : Conductor<IScreen>
    {
        // Full properties
        private Register _register;
        public Register Register
        {
            get { return _register; }
            set
            {
                _register = value;
                NotifyOfPropertyChange(() => Register);
            }
        }

        private bool _isEnable;
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                _isEnable = value;
                NotifyOfPropertyChange(() => IsEnable);
            }
        }
        // Services
        private readonly AppNotificationService _appNotificationService;
        public AppNotificationService AppNotificationService => _appNotificationService;
        private readonly IWindowManager _windowManager;
        private readonly IUserAccountInterface _userAccountService;
        public IUserAccountInterface UserAccountService => _userAccountService;

        public MainViewModel(IWindowManager windowManager)
        {


            _windowManager = windowManager;
            _userAccountService = App.ServiceProvider.GetService(typeof(IUserAccountInterface)) as IUserAccountInterface;
            _appNotificationService = new AppNotificationService();
            // Set MainView to MainWindow
            var window = Application.Current.Windows.OfType<MainView>().FirstOrDefault();
            Application.Current.MainWindow = window;
            SetFullScreen();


        }
        //-----------------------------------------------------------------------------------------
        // Setting property
        private bool _isWindowFullScreen = Properties.Settings.Default.stWindowFullScreen; //Kiem tra Setting co dang o trang thai Full Screen hay khong
        public bool IsWindowFullScreen
        {
            get { return _isWindowFullScreen; }
            set
            {
                _isWindowFullScreen = value;
                NotifyOfPropertyChange(() => IsWindowFullScreen);
                Properties.Settings.Default.stWindowFullScreen = value;
                Properties.Settings.Default.Save(); // Add this line
                SetFullScreen();
            }
        }
        private WindowState _windowState;
        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value;
                NotifyOfPropertyChange(() => WindowState);
            }
        }
        private WindowStyle _windowStyle;
        public WindowStyle WindowStyle
        {
            get { return _windowStyle; }
            set
            {
                _windowStyle = value;
                NotifyOfPropertyChange(() => WindowStyle);
            }
        }
        private bool _isAccountButtonVisible = false;
        public bool IsAccountButtonVisible
        {
            get { return _isAccountButtonVisible; }
            set
            {
                _isAccountButtonVisible = value;
                NotifyOfPropertyChange(() => IsAccountButtonVisible);
            }
        }
        //-----------------------------------------------------------------------------------------
        public async void OnStartup()
        {
            IsEnable = false;
            await HomeViewAsync();

            ////For Debugging purpose, you can set the Register object directly
            //Properties.Settings.Default.FistStartup = true; // Đánh dấu đã khởi tạo
            //Properties.Settings.Default.Save(); // Lưu thay đổi vào Settings

            // Đọc thông tin đăng ký từ Settings
            if (Properties.Settings.Default.FistStartup)
            {
                Register = new Register
                {
                    Name = "Admin",
                    Password = "FujiAdmin",
                    ConfirmPassword = "FujiAdmin"
                };

                // Tạo user Admin cho lần đầu tiên
                var result = await _userAccountService.CreateAsync(Register);
                if (result.Flag)
                {
                    AppNotificationService.ShowSuccess(result.Message);
                    Properties.Settings.Default.FistStartup = false; // Đánh dấu đã khởi tạo
                    Properties.Settings.Default.Save(); // Lưu thay đổi vào Settings
                }
                else
                {
                    Properties.Settings.Default.FistStartup = false; // Đánh dấu đã khởi tạo
                    Properties.Settings.Default.Save(); // Lưu thay đổi vào Settings
                }
            }

            // Cửa sổ đăng nhập
            var loginViewModel = new LoginViewModel(this);
            await _windowManager.ShowWindowAsync(loginViewModel);

        }
        //Set FullScreen
        public void SetFullScreen()
        {
            if (_isWindowFullScreen)
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }
            else
            {
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }
        public async Task LoadData()
        {
            IsEnable = false;
            await ShowLoadingScreen();

            // Đăng ký sự kiện khi đăng nhập thành công
            try
            {
                var backupService = new Client.Services.DatabaseBackupService();
                backupService.BackupIfNeeded();
                
                await CachesServices.Instance.GetAllData();

                // Check usser role
                if(CachesServices.SystemRole.Name == Constants.Admin)
                {
                    IsAccountButtonVisible = true;
                }
                else
                {
                    IsAccountButtonVisible = false;
                }
            }
            catch (Exception ex)
            {
                //Custom Messagebox!
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = ex.Message,
                    IsConfirmation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            }

            IsEnable = true;
            CloseLoadingScreen();
        }
        //Show LoadingScreen
        public async Task ShowLoadingScreen()
        {
            var loadingScreenViewModel = new LoadingScreenViewModel();
            await _windowManager.ShowWindowAsync(loadingScreenViewModel);
        }
        //Close LoadingScreen
        public void CloseLoadingScreen()
        {
            var window = Application.Current.Windows.OfType<LoadingScreenView>().FirstOrDefault();
            if (window != null)
            {
                window.Close();
                (window.DataContext as IDisposable)?.Dispose();
            }
        }

        //------------------------------------------------------------------------------------------
        //Button event
        public void bnCloseMenu()
        {
            // Get a reference to the MainView window

            var window = Application.Current.Windows.OfType<MainView>().FirstOrDefault();
            if (window != null)
            {
                // Find the HideMenuText Storyboard                
                var storyboard = window.FindResource("MenuClose") as Storyboard;

                if (storyboard != null)
                {
                    window.bnCloseMenu.Visibility = Visibility.Hidden;
                    // Begin the storyboard
                    storyboard.Begin();
                    window.bnOpenMenu.Visibility = Visibility.Visible;
                }
            }
        }
        public void bnOpenMenu()
        {
            var window = Application.Current.Windows.OfType<MainView>().FirstOrDefault();
            if (window != null)
            {
                window.bnCloseMenu.Visibility = Visibility.Visible;
                window.bnOpenMenu.Visibility = Visibility.Hidden;
            }
        }
        public void bnExit()
        {
            //Custom Messagebox!
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "Thoát FUJIMED Quản Lý",
                TxtMessage = "Bạn có muốn thoát phần mềm không?",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        // Logout
        public async Task Logout()
        {
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "Đăng xuất",
                TxtMessage = "Bạn có muốn đăng xuất không?",
                IsConfirmation = true
            };

            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);

            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {

                try
                {

                    // Clear cache
                    CachesServices.ResetInstance();
                }
                catch (Exception ex)
                {
                    customMessageBoxViewModel = new CustomMessageBoxViewModel
                    {
                        Message = "Error",
                        TxtMessage = ex.Message,
                        IsConfirmation = false
                    };
                    await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                    return;
                }
                // Show notification

                AppNotificationService.ShowInformation("Đăng xuất thành công");

                OnStartup();

            }
        }


        //Navigation
        public async Task bnDataViewer()
        {
            //CachesServices.Instance.ResetCaches(); //Xóa bộ nhớ đệm
            //await CachesServices.Instance.GetAllData(); //Cập nhât bộ nhớ đệm
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            await ActivateItemAsync(new DataViewerViewModel());
        }
        public async Task TicketView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            TicketViewModel ticketViewModel = new TicketViewModel(this);
            await ActivateItemAsync(ticketViewModel);
            await ticketViewModel.TicketManagementView();
        }
        public async Task InstrumentView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            InstrumentViewModel instrumentViewModel = new InstrumentViewModel(this);
            await ActivateItemAsync(instrumentViewModel);
            await instrumentViewModel.InstrumentManagementView();
        }
        public async Task CustomerView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            //await ActivateItemAsync(new CustomerViewModel(this));
            CustomerMainViewModel customerMainViewModel = new CustomerMainViewModel(this);
            await ActivateItemAsync(customerMainViewModel);
            await customerMainViewModel.CustomerView();
        }
        public async Task SpecificationView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            SpecificationViewModel specificationViewModel = new SpecificationViewModel();
            await ActivateItemAsync(specificationViewModel);
            await specificationViewModel.ModelView();
        }
        public async Task IssueView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IssueViewModel issueViewModel = new IssueViewModel(this);
            await ActivateItemAsync(issueViewModel);
            await issueViewModel.IssueManagementView();
        }

        public async Task AccountView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            AccountViewModel accountViewModel = new AccountViewModel(this);
            await ActivateItemAsync(accountViewModel);
            await accountViewModel.UserManagementView();
        }

        //-----------------------------------------------------------------------------------------
        //Mở New Ticket từ các View khác
        public async Task OpenNewEditTicketView(Ticket ticket, bool isNew)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            var ticketViewModel = new TicketViewModel(this);
            await ActivateItemAsync(ticketViewModel);
            await ticketViewModel.NewTicketView(ticket, isNew);
        }
        //Mở Ticket Detail từ các View khác
        public async Task OpenTicketDetailView(Ticket ticket)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            var ticketViewModel = new TicketViewModel(this);
            await ActivateItemAsync(ticketViewModel);
            await ticketViewModel.TicketDetail(ticket);
        }

        //Mở InstrumentDetail từ các View khác
        public async Task OpenInstrumentDetailView(Instrument instrument)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            var instrumentViewModel = new InstrumentViewModel(this);
            await ActivateItemAsync(instrumentViewModel);
            await instrumentViewModel.InstrumentDetail(instrument);
        }

        // Mở IssueDetail từ các View khác
        public async Task OpenIssueDetailView(Issue issue)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            var issueViewModel = new IssueViewModel(this);
            await ActivateItemAsync(issueViewModel);
            await issueViewModel.IssueDetailView(issue);
        }

        public async Task HomeViewAsync()
        {
            await DeactivateAndDisposeActiveItem();
            var homeViewModel = new HomeViewModel();
            await ActivateItemAsync(homeViewModel);
        }
        private async Task DeactivateAndDisposeActiveItem()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
        }
    }
}
