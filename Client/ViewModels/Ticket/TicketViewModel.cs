using Caliburn.Micro;
using Client.Entities.TicketEntities;
using System;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class TicketViewModel : Conductor<IScreen>
    {
        // Full properties

        // Settings
        private string _newEditTabTitle;
        public string NewEditTabTitle
        {
            get { return _newEditTabTitle; }
            set
            {
                _newEditTabTitle = value;
                NotifyOfPropertyChange(() => NewEditTabTitle);
            }
        }
        private bool _isInstrumnetDetailVisible = false;
        public bool IsInstrumnetDetailVisible
        {
            get { return _isInstrumnetDetailVisible; }
            set
            {
                _isInstrumnetDetailVisible = value;
                NotifyOfPropertyChange(() => IsInstrumnetDetailVisible);
            }
        }
        private bool _isNewEditTicketVisible = false;
        public bool IsNewEditTicketVisible
        {
            get { return _isNewEditTicketVisible; }
            set
            {
                _isNewEditTicketVisible = value;
                NotifyOfPropertyChange(() => IsNewEditTicketVisible);
            }
        }


        // Button Check Properties
        private bool _isTicketManagementViewChecked = true;
        public bool IsTicketManagementViewChecked
        {
            get { return _isTicketManagementViewChecked; }
            set
            {
                _isTicketManagementViewChecked = value;
                NotifyOfPropertyChange(() => IsTicketManagementViewChecked);
            }
        }
        private bool _isTicketPendingViewChecked = false;
        public bool IsTicketPendingViewChecked
        {
            get { return _isTicketPendingViewChecked; }
            set
            {
                _isTicketPendingViewChecked = value;
                NotifyOfPropertyChange(() => IsTicketPendingViewChecked);
            }
        }
        private bool _isTicketReportViewChecked = false;
        public bool IsTicketReportViewChecked
        {
            get { return _isTicketReportViewChecked; }
            set
            {
                _isTicketReportViewChecked = value;
                NotifyOfPropertyChange(() => IsTicketReportViewChecked);
            }
        }
        private bool _isNewEditTicketViewChecked = false;
        public bool IsNewEditTicketViewChecked
        {
            get { return _isNewEditTicketViewChecked; }
            set
            {
                _isNewEditTicketViewChecked = value;
                NotifyOfPropertyChange(() => IsNewEditTicketViewChecked);
                IsNewEditTicketVisible = value;
            }
        }
        private bool _isTicketDetailChecked = false;
        private readonly MainViewModel _mainViewModel;
        public bool IsTicketDetailChecked
        {
            get { return _isTicketDetailChecked; }
            set
            {
                _isTicketDetailChecked = value;
                NotifyOfPropertyChange(() => IsTicketDetailChecked);
                IsInstrumnetDetailVisible = value;
            }
        }


        // Constructor
        public TicketViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        // Command to change tab
        public async Task TicketManagementView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            await ActivateItemAsync(new TicketManagementViewModel(this));
        }
        public async Task TicketPendingView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsTicketPendingViewChecked = true;
            await ActivateItemAsync(new TicketPendingViewModel(this));
        }
        public async Task TicketReportView()
        {
            IsTicketReportViewChecked = true;
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
        }
        public async Task TicketDetail(Ticket Ticket)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsTicketDetailChecked = true;
            await ActivateItemAsync(new TicketDetailViewModel(Ticket, this, _mainViewModel));
        }
        public async Task NewTicketView(Ticket Ticket, bool isNew)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            NewEditTabTitle = "New Ticket";
            IsNewEditTicketViewChecked = true;
            await ActivateItemAsync(new NewEditTicketViewModel(Ticket, true, this, _mainViewModel));
        }
        public async Task EditTicketView(Ticket Ticket)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            NewEditTabTitle = "Edit Ticket";
            IsNewEditTicketViewChecked = true;
            await ActivateItemAsync(new NewEditTicketViewModel(Ticket, false, this, _mainViewModel));
        }

    }
}
