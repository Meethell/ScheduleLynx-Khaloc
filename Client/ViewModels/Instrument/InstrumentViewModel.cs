using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using Client.Entities.ModelEntities;
using System.Runtime.InteropServices;

namespace Client.ViewModels
{
    public class InstrumentViewModel : Conductor<IScreen>
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
        private bool _isNewEditInstrumentVisible = false;
        public bool IsNewEditInstrumentVisible
        {
            get { return _isNewEditInstrumentVisible; }
            set
            {
                _isNewEditInstrumentVisible = value;
                NotifyOfPropertyChange(() => IsNewEditInstrumentVisible);
            }
        }
        private bool _isInstrumentModelDetailVisible = false;
        public bool IsInstrumentModelDetailVisible
        {
            get { return _isInstrumentModelDetailVisible; }
            set
            {
                _isInstrumentModelDetailVisible = value;
                NotifyOfPropertyChange(() => IsInstrumentModelDetailVisible);
            }
        }

        // Button Check Properties
        private bool _isInstrumentManagementViewChecked = true;
        public bool IsInstrumentManagementViewChecked
        {
            get { return _isInstrumentManagementViewChecked; }
            set
            {
                _isInstrumentManagementViewChecked = value;
                NotifyOfPropertyChange(() => IsInstrumentManagementViewChecked);
            }
        }
        private bool _isModelViewChecked = false;
        public bool IsModelViewChecked
        {
            get { return _isModelViewChecked; }
            set
            {
                _isModelViewChecked = value;
                NotifyOfPropertyChange(() => IsModelViewChecked);
            }
        }
        private bool _isNewEditInstrumentViewChecked = false;
        public bool IsNewEditInstrumentViewChecked
        {
            get { return _isNewEditInstrumentViewChecked; }
            set
            {
                _isNewEditInstrumentViewChecked = value;
                NotifyOfPropertyChange(() => IsNewEditInstrumentViewChecked);
                IsNewEditInstrumentVisible = value;
            }
        }
        private bool _isInstrumentDetailChecked = false;
        public bool IsInstrumentDetailChecked
        {
            get { return _isInstrumentDetailChecked; }
            set
            {
                _isInstrumentDetailChecked = value;
                NotifyOfPropertyChange(() => IsInstrumentDetailChecked);
                IsInstrumnetDetailVisible = value;
            }
        }
        private bool _isInstrumentModelDetailViewChecked = false;
        public bool IsInstrumentModelDetailViewChecked
        {
            get { return _isInstrumentModelDetailViewChecked; }
            set
            {
                _isInstrumentModelDetailViewChecked = value;
                NotifyOfPropertyChange(() => IsInstrumentModelDetailViewChecked);
                IsInstrumentModelDetailVisible = value;
            }
        }
        private bool _isSparePartManagementChecked;
        public bool IsSparePartManagementChecked
        {
            get { return _isSparePartManagementChecked; }
            set
            {
                _isSparePartManagementChecked = value;
                NotifyOfPropertyChange(() => IsSparePartManagementChecked);
            }
        }
        // Constructor
        private MainViewModel _mainViewModel;
        public InstrumentViewModel(MainViewModel mainViewModel)
        {
            // Initialize with default view
            _mainViewModel = mainViewModel;
        }

        // Command to change tab
        public async Task InstrumentManagementView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            await ActivateItemAsync(new InstrumentManagementViewModel(this));
        }
        public async Task InstrumentModelView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            await ActivateItemAsync(new InstrumentModelViewModel(this));
        }
        public async Task InstrumentModelDetailView(Model model)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsInstrumentModelDetailViewChecked = true;

        }
        public async Task NewInstrumentView(Instrument instrument, bool isNew)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            NewEditTabTitle = "Tạo mới Thiết bị";
            IsNewEditInstrumentViewChecked = true;
            await ActivateItemAsync(new NewEditInstrumentViewModel(new Instrument(), true, this));
        }
        public async Task EditInstrumentView(Instrument instrument)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            NewEditTabTitle = "Chỉnh sửa Thiết bị";
            IsNewEditInstrumentViewChecked = true;
            await ActivateItemAsync(new NewEditInstrumentViewModel(instrument, false, this));
        }
        public async Task InstrumentDetail(Instrument instrument)
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsInstrumentDetailChecked = true;
            await ActivateItemAsync(new InstrumentDetailViewModel(instrument, this, _mainViewModel));
        }
        public async Task SparePartManagementView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsSparePartManagementChecked = true;
            await ActivateItemAsync(new SparePartManagementViewModel(this, _mainViewModel));

        }
    }
}
