using Caliburn.Micro;
using Client.Entities.TicketEntities;
using System.Collections.ObjectModel;
using Client.Services;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Client.Entities.ModelEntities;

namespace Client.ViewModels
{
    public class IssueManagementViewModel : Screen
    {
        //-----------------------------------------------------------------------------------------
        // Phần Model Selector
        // View properties
        private bool _isIssueNewAble = false;
        public bool IsIssueNewAble
        {
            get => _isIssueNewAble;
            set
            {
                _isIssueNewAble = value;
                NotifyOfPropertyChange(() => IsIssueNewAble);
            }
        }
        private bool _isModelSelectorVisible = true;
        public bool IsModelSelectorVisible
        {
            get => _isModelSelectorVisible;
            set
            {
                _isModelSelectorVisible = value;
                NotifyOfPropertyChange(() => IsModelSelectorVisible);
            }
        }
        private bool _isModelListVisible = false;
        public bool IsModelListVisible
        {
            get => _isModelListVisible;
            set
            {
                _isModelListVisible = value;
                NotifyOfPropertyChange(() => IsModelListVisible);
            }
        }
        // Properties
        private List<Model> _tempModels;
        private ObservableCollection<Model> _models;
        public ObservableCollection<Model> Models
        {
            get => _models;
            set
            {
                _models = value;
                NotifyOfPropertyChange(() => Models);
            }
        }
        private Model _selectedModel;
        public Model SelectedModel
        {
            get => _selectedModel;
            set
            {
                _selectedModel = value;
                NotifyOfPropertyChange(() => SelectedModel);

                if (value != null)
                {
                    IsIssueNewAble = true;
                    LoadIssues();
                }
                else
                {
                    IsIssueNewAble = false;
                }
            }
        }
        private ObservableCollection<Manufacturer> _manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers.OrderBy(x => x.Name));
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get => _manufacturers;
            set
            {
                _manufacturers = value;
                NotifyOfPropertyChange(() => Manufacturers);
            }
        }
        private Manufacturer _selectedManufacturer;
        public Manufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                _selectedManufacturer = value;
                NotifyOfPropertyChange(() => SelectedManufacturer);
                SelectedModel = null;
                if (value != null)
                {
                    LoadModel();
                    IsModelListVisible = true;

                }
                else
                {
                    IsModelListVisible = false;
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        // Constructor
        private readonly IssueViewModel _issueViewModel;
        public IssueManagementViewModel(IssueViewModel issueViewModel)
        {
            _issueViewModel = issueViewModel;
        }

        // Methods
        public void LoadModel()
        {
            if (SelectedManufacturer != null)
            {
                Models = new ObservableCollection<Model>(CachesServices.Instance.Models.FindAll(x => x.ManufacturerId == SelectedManufacturer.Id && x.IsConsumable == false).OrderBy(x => x.Name));
            }
        }
        public void ToggleModelSelector()
        {
            IsModelSelectorVisible = !IsModelSelectorVisible;
        }
        //-----------------------------------------------------------------------------------------
        // Tìm kiếm Manufacturer
        private string _searchManufacturer;
        public string SearchManufacturer
        {
            get { return _searchManufacturer; }
            set
            {
                _searchManufacturer = value;
                NotifyOfPropertyChange(() => SearchManufacturer);
                if (string.IsNullOrEmpty(SearchManufacturer))
                {
                    Manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers);
                }
                else
                {
                    Manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers.Where(c => c.Name.ToLower().Contains(SearchManufacturer.ToLower())).OrderBy(x => x.Name));
                }
            }
        }
        // Tìm kiếm Model
        private string _searchModel;
        public string SearchModel
        {
            get { return _searchModel; }
            set
            {
                _searchModel = value;
                NotifyOfPropertyChange(() => SearchModel);
                if (string.IsNullOrEmpty(SearchModel))
                {
                    LoadModel();
                }
                else
                {
                    LoadModel();
                    _tempModels = new List<Model>(Models); // Lưu lại các Model đã được load khi chọn manufacturer
                    Models = new ObservableCollection<Model>(_tempModels.Where(c => c.Name.ToLower().Contains(SearchModel.ToLower())).OrderBy(x => x.Name));
                }
            }
        }


        //-----------------------------------------------------------------------------------------
        // Phần Issue Management
        private ObservableCollection<Issue> _issues;
        public ObservableCollection<Issue> Issues
        {
            get => _issues;
            set
            {
                _issues = value;
                NotifyOfPropertyChange(() => Issues);
            }
        }

        public void LoadIssues()
        {
            if (SelectedModel != null)
            {
                Issues = new ObservableCollection<Issue>(CachesServices.Instance.Issues.FindAll(x => x.ModelId == SelectedModel.Id).OrderBy(x => x.Title));
            }
        }
        public async Task OpenIssueDetailView(RoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement fe)
            {
                if (fe.DataContext is Issue issue)
                {
                    await _issueViewModel.IssueDetailView(issue);
                }
            }
        }

        public async Task NewIssue()
        {
            // Mở NewIssueWindow
            var newIssueWindowViewModel = new NewIssueWindowViewModel(SelectedModel);
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(newIssueWindowViewModel);
            // Kiểm tra kết quả trả về
            if (newIssueWindowViewModel.Result)
            {
                // Cập nhật lại Issue
                LoadIssues();
            }
        }
    }
}
