using Caliburn.Micro;
using Client.DTOs;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class RegisterViewModel : Screen
    {
        private bool _flag = false;
        public bool Flag
        {
            get => _flag;
            set
            {
                _flag = value;
                NotifyOfPropertyChange(() => Flag);
            }
        }

        private Register _register = new Register();
        public Register Register
        {
            get => _register;
            set
            {
                _register = value;
                NotifyOfPropertyChange(() => Register);
            }
        }

        private readonly MainViewModel _mainViewModel;
        // constructor
        public RegisterViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public async Task RegisterCommand()
        {

            if (!AreRequiredFieldsFilled())
            {
                _mainViewModel.AppNotificationService.ShowError("Vui lòng điền đầy đủ thông tin");
                return;
            }

            if (!CheckConfirmPassword())
            {
                Flag = false;
                return;
            }

            var result = await _mainViewModel.UserAccountService.CreateAsync(Register).ConfigureAwait(false);

            if (!result.Flag)
            {
                Flag = false;
                _mainViewModel.AppNotificationService.ShowError(result.Message);
                return;
            }

            _mainViewModel.AppNotificationService.ShowSuccess("Đăng ký thành công");
            Flag = true;
            await TryCloseAsync().ConfigureAwait(false);
        }

        private bool AreRequiredFieldsFilled()
        {
            return !string.IsNullOrEmpty(Register.Name) && !string.IsNullOrEmpty(Register.Password) && !string.IsNullOrEmpty(Register.ConfirmPassword);
        }

        private bool CheckConfirmPassword()
        {
            if (Register.Password != Register.ConfirmPassword)
            {
                _mainViewModel.AppNotificationService.ShowError("Mật khẩu không khớp");
                return false;
            }
            return true;
        }

        public void Cancel()
        {
            Flag = false;
            TryCloseAsync();
        }
    }
}
