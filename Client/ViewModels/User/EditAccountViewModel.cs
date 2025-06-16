using Caliburn.Micro;
using Client.Entities.UserEntity;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class EditAccountViewModel : Screen
    {
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                NotifyOfPropertyChange(() => User);
            }
        }
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
        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                NotifyOfPropertyChange(() => NewPassword);
            }
        }
        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                NotifyOfPropertyChange(() => ConfirmPassword);
            }
        }

        private readonly MainViewModel _mainViewModel;
        public EditAccountViewModel(MainViewModel mainViewModel, User user)
        {
            _mainViewModel = mainViewModel;
            User = user;
            User.Password = string.Empty; // Clear password to avoid showing it in the UI
        }

        public async Task UpdateCommand()
        {
            if (string.IsNullOrWhiteSpace(User.Name) || string.IsNullOrWhiteSpace(User.Name))
            {
                _mainViewModel.AppNotificationService.ShowError("Vui lòng điền đầy đủ thông tin");
                return;
            }

            // Check if new password is provided and not empty
            if (string.IsNullOrWhiteSpace(NewPassword) && string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                _mainViewModel.AppNotificationService.ShowError("Vui lòng nhập mật khẩu mới hoặc xác nhận mật khẩu");
                return;
            }

            // Check if new password is provided and matches confirm password
            if (!string.IsNullOrWhiteSpace(NewPassword) && NewPassword != ConfirmPassword)
            {
                _mainViewModel.AppNotificationService.ShowError("Mật khẩu mới và xác nhận mật khẩu không khớp");
                return;
            }

            var result = await _mainViewModel.UserAccountService.UpdateUserAccountAsync(User.Id, User.Name, NewPassword);
            if (!result.Flag)
            {
                Flag = false;
                _mainViewModel.AppNotificationService.ShowError(result.Message);
                return;
            }
            Flag = true;
            _mainViewModel.AppNotificationService.ShowSuccess("Cập nhật tài khoản thành công");

            await this.TryCloseAsync();
        }
        public async Task Cancel()
        {
            Flag = false;
            await this.TryCloseAsync();
        }
    }
}
