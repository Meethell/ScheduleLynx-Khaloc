using Caliburn.Micro;
using System.Threading.Tasks;
using System;
using Client.Entities.TicketEntities;

namespace Client.ViewModels
{
    public class IssueViewModel : Conductor<IScreen>
    {
        // Setting View properties        

        // Settings Button properties
        private bool _isIssueManagementViewChecked;
        public bool IsIssueManagementViewChecked
        {
            get => _isIssueManagementViewChecked;
            set
            {
                _isIssueManagementViewChecked = value;
                NotifyOfPropertyChange(() => IsIssueManagementViewChecked);
            }
        }
        private bool _isIssueDetailViewChecked = false;
        public bool IsIssueDetailViewChecked
        {
            get => _isIssueDetailViewChecked;
            set
            {
                _isIssueDetailViewChecked = value;
                NotifyOfPropertyChange(() => IsIssueDetailViewChecked);
            }
        }
        private bool _isIssueNewEditViewChecked = false;
        public bool IsIssueNewEditViewChecked
        {
            get => _isIssueNewEditViewChecked;
            set
            {
                _isIssueNewEditViewChecked = value;
                NotifyOfPropertyChange(() => IsIssueNewEditViewChecked);
            }
        }
        private bool _isIssueStatisticsViewViewChecked;
        public bool IsIssueStatisticsViewChecked
        {
            get => _isIssueStatisticsViewViewChecked;
            set
            {
                _isIssueStatisticsViewViewChecked = value;
                NotifyOfPropertyChange(() => IsIssueStatisticsViewChecked);
            }
        }

        // Constructor
        private readonly MainViewModel _mainViewModel;
        public IssueViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public async Task IssueManagementView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsIssueManagementViewChecked = true;
            await ActivateItemAsync(new IssueManagementViewModel(this));
        }
        public async Task IssueDetailView(Issue issue)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsIssueDetailViewChecked = true;
            await ActivateItemAsync(new IssueDetailViewModel(_mainViewModel, this, issue));
        }
        public async Task IssueStatisticsView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsIssueStatisticsViewChecked = true;
            //await ActivateItemAsync(new IssueStatisticsViewModel(this));
        }
    }
}
