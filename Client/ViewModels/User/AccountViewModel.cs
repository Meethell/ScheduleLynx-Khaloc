using Caliburn.Micro;
using Client.Services;
using System;
using System.Threading.Tasks;


namespace Client.ViewModels
{
    public class AccountViewModel : Conductor<IScreen>
    {
        // Full properties
        private bool _isUserManagementViewChecked = false;
        public bool IsUserManagementViewChecked
        {
            get { return _isUserManagementViewChecked; }
            set
            {
                _isUserManagementViewChecked = value;
                NotifyOfPropertyChange(() => IsUserManagementViewChecked);
            }
        }
        private bool _isUserDetailViewChecked = false;
        public bool IsUserDetailViewChecked
        {
            get { return _isUserDetailViewChecked; }
            set
            {
                _isUserDetailViewChecked = value;
                NotifyOfPropertyChange(() => IsUserDetailViewChecked);
            }
        }
        private bool _isUserEditViewChecked = false;
        public bool IsUserEditViewChecked
        {
            get { return _isUserEditViewChecked; }
            set
            {
                _isUserEditViewChecked = value;
                NotifyOfPropertyChange(() => IsUserEditViewChecked);
            }
        }
        private bool _isUserManagementViewVisible = false;
        public bool IsUserManagementViewVisible
        {
            get { return _isUserManagementViewVisible; }
            set
            {
                _isUserManagementViewVisible = value;
                NotifyOfPropertyChange(() => IsUserManagementViewVisible);
            }
        }
        private readonly MainViewModel _mainViewModel;
        // Constructor
        public AccountViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        // Navigation
        private async Task DeactivateAndDisposeActiveItem()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
        }
        //public async Task UserDetailView(int userId)
        //{
        //    if (userId == 0)
        //    {
        //        userId = CachesServices.Instance.UserId;
        //    }
        //    await DeactivateAndDisposeActiveItem();
        //    var userDetailViewModel = new UserDetailViewModel(_mainViewModel, userId);
        //    IsUserDetailViewChecked = true;
        //    await ActivateItemAsync(userDetailViewModel);
        //}
        public async Task UserManagementView()
        {
            await DeactivateAndDisposeActiveItem();
            var userManagementViewModel = new UserManagementViewModel(_mainViewModel, this);
            IsUserManagementViewChecked = true;
            await ActivateItemAsync(userManagementViewModel);
        }


    }
}
