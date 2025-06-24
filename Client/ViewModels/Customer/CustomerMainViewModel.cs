using Caliburn.Micro;
using System;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class CustomerMainViewModel : Conductor<IScreen>
    {
        private bool _isCustomerViewChecked = false;
        public bool IsCustomerViewChecked
        {
            get { return _isCustomerViewChecked; }
            set
            {
                _isCustomerViewChecked = value;
                NotifyOfPropertyChange(() => IsCustomerViewChecked);
            }
        }
        private bool _isDealerViewChecked = false;
        public bool IsDealerViewChecked
        {
            get { return _isDealerViewChecked; }
            set
            {
                _isDealerViewChecked = value;
                NotifyOfPropertyChange(() => IsDealerViewChecked);
            }
        }

        private bool _isDealerDetailChecked = false;
        public bool IsDealerDetailChecked
        {
            get { return _isDealerDetailChecked; }
            set
            {
                _isDealerDetailChecked = value;
                NotifyOfPropertyChange(() => IsDealerDetailChecked);
            }
        }
        private bool _isCustomerDetailChecked = false;
        public bool IsCustomerDetailChecked
        {
            get { return _isCustomerDetailChecked; }
            set
            {
                _isCustomerDetailChecked = value;
                NotifyOfPropertyChange(() => IsCustomerDetailChecked);
            }
        }
        private bool _isModelCustomerViewChecked = false;
        public bool IsModelCustomerViewChecked
        {
            get { return _isModelCustomerViewChecked; }
            set
            {
                _isModelCustomerViewChecked = value;
                NotifyOfPropertyChange(() => IsModelCustomerViewChecked);
            }
        }

        // Constructor
        private readonly MainViewModel _mainViewModel;
        public CustomerMainViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        // Bật các trang
        public async Task CustomerView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsCustomerViewChecked = true;
            await ActivateItemAsync(new CustomerViewModel(_mainViewModel,this));
        }
        public async Task DealerView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsDealerViewChecked = true;
            await ActivateItemAsync(new DealerViewModel(_mainViewModel, this));
        }
        public async Task ModelCustomerView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsModelCustomerViewChecked = true;
            await ActivateItemAsync(new ModelCustomerViewModel(_mainViewModel, this));
        }
    }
}
