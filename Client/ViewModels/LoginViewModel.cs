using Caliburn.Micro;
using Client.DTOs;
using Client.Services;
using System.Windows;

namespace Client.ViewModels
{
    public class LoginViewModel : Screen
    {
        private Login _login = new Login();
        public Login Login
        {
            get => _login;
            set
            {
                _login = value;
                NotifyOfPropertyChange(() => Login);
            }
        }
        private readonly MainViewModel _mainViewModel;
        public LoginViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public async void LoginCommand(object eventArgs = null)
        {
            if (eventArgs is System.Windows.Input.KeyEventArgs keyEvent)
            {
                if (keyEvent.Key != System.Windows.Input.Key.Enter)
                    return;
            }
            // Dành cho Debug
            //var result = await _mainViewModel.UserAccountService.SignInAsync(new Login { Name = "Admin", Password = "FujiAdmin" });

            // Kiểm tra thông tin đăng nhập
            if (string.IsNullOrWhiteSpace(Login.Name) || string.IsNullOrWhiteSpace(Login.Password))
            {
                _mainViewModel.AppNotificationService.ShowError("Tên đăng nhập hoặc mật khẩu không được để trống.");
                return;
            }

            var result = await _mainViewModel.UserAccountService.SignInAsync(Login);
            if (result.Flag)
            {
                // Lấy ID của User vừa đăng nhập để lưu vào Caches
                CachesServices.UserId = result.UserId;
                _mainViewModel.AppNotificationService.ShowSuccess(result.Message);
                _mainViewModel.LoadData();
                await this.TryCloseAsync();
            }
            else
            {
                _mainViewModel.AppNotificationService.ShowError(result.Message);
            }
        }
        public async void CloseApp()
        {
            await this.TryCloseAsync();
            Application.Current.Shutdown();
        }
    }
}
