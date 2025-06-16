using Caliburn.Micro;
using Client.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Client.Entities.ModelEntities;


namespace Client.ViewModels
{
    public class ModelViewModel : Screen
    {
        private readonly SpecificationViewModel _productCatalogViewModel;
        private bool _isManufacturerColumnVisible;
        public bool IsManufacturerColumnVisible
        {
            get { return _isManufacturerColumnVisible; }
            set
            {
                _isManufacturerColumnVisible = value;
                NotifyOfPropertyChange(() => IsManufacturerColumnVisible);
            }
        }
        private bool IsConsumable { get; set; }
        // Constructor
        private AppNotificationService NotificationService = new AppNotificationService();
        public ModelViewModel(SpecificationViewModel productCatalogViewModel, bool isConsumable)
        {
            IsConsumable = isConsumable;
            IsManufacturerColumnVisible = true;
            _productCatalogViewModel = productCatalogViewModel;
            LoadData();
            IsManufacturerUserEditting = false;
            IsModelUserEditting = false;

            IsModelNewButtonEnable = false;
            IsModelEnable = true;
            IsManufacturerEnable = true;
        }

        public void SetManufacturerColumnVisible()
        {
            IsManufacturerColumnVisible = !IsManufacturerColumnVisible;
        }

        //-------------------------------------------------------------------------------------
        //Setting Properties
        private ObservableCollection<ModelType> _modelTypes;
        public ObservableCollection<ModelType> ModelTypes
        {
            get { return _modelTypes; }
            set
            {
                _modelTypes = value;
                NotifyOfPropertyChange(() => ModelTypes);
            }
        }

        private ObservableCollection<Country> _countries;
        public ObservableCollection<Country> Countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
                NotifyOfPropertyChange(() => Countries);
            }
        }

        #region Manufacturer Setting Properties
        // Manufacturer properties
        private List<Manufacturer> _tempManufacturer; // Dùng để lưu trữ dữ liệu khi người dùng editting

        private ObservableCollection<Manufacturer> _Manufacturers; // Dùng để hiển thị dữ liệu
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return _Manufacturers; }
            set
            {
                _Manufacturers = value;
                NotifyOfPropertyChange(() => Manufacturers);
                IsManufacturerEditButtonEnable = Manufacturers.Count > 0;
            }
        }

        private Manufacturer _selectedManufacturer;
        public Manufacturer SelectedManufacturer
        {
            get { return _selectedManufacturer; }
            set
            {
                _selectedManufacturer = value;
                NotifyOfPropertyChange(() => SelectedManufacturer);
                if (SelectedManufacturer != null)
                {
                    LoadModel();
                    // Đưa dữ liệu về trạng thái ban đầu
                    IsModelUserEditting = false;
                    IsModelNewButtonEnable = true;
                }
                else
                {
                    IsModelNewButtonEnable = false;
                    Models = new ObservableCollection<Model>();
                }
                SetManufacturerDeleteButtonEnable();

            }
        }

        // IsManufacturerUserEditting - KEY - Để xác định xem người dùng đang editting hay không
        // - Gía trị ngươc lại với IsManufacturerReadOnly
        // - Cho phép nút Ok và Cancel hiện lên khi IsManufacturerUserEditting = true
        private bool _isManufacturerUserEditting;
        public bool IsManufacturerUserEditting
        {
            get { return _isManufacturerUserEditting; }
            set
            {
                _isManufacturerUserEditting = value;
                NotifyOfPropertyChange(() => IsManufacturerUserEditting);
                IsManufacturerReadOnly = !value;
                IsManufacturerNewButtonEnable = !value;
                IsModelNewButtonEnable = !value;
                SetManufacturerDeleteButtonEnable();
            }
        }
        // IsManufacturerReadOnly - Để xác định xem người dùng có thể chỉnh sửa ManufacturerDataGrid hay không
        // - Cho phép nút Edit hiện lên khi IsManufacturerReadOnly = true
        private bool _isManufacturerReadOnly;
        public bool IsManufacturerReadOnly
        {
            get { return _isManufacturerReadOnly; }
            set
            {
                _isManufacturerReadOnly = value;
                NotifyOfPropertyChange(() => IsManufacturerReadOnly);

            }
        }
        // IsManufacturerEnable - Để xác định xem người dùng có thể chỉnh sửa ManufacturerDataGrid hay không
        private bool _isManufacturerEnable;
        public bool IsManufacturerEnable
        {
            get { return _isManufacturerEnable; }
            set
            {
                _isManufacturerEnable = value;
                NotifyOfPropertyChange(() => IsManufacturerEnable);
            }
        }

        // Button Properties

        // IsManufacturerEditButtonEnable - Để xác định xem nút Edit có được enable hay không
        // - Nếu ManufacturerDataGrid không có dữ liệu thì nút Edit sẽ không được enable
        private bool _isManufacturerEditButtonEnable;
        public bool IsManufacturerEditButtonEnable
        {
            get { return _isManufacturerEditButtonEnable; }
            set
            {
                _isManufacturerEditButtonEnable = value;
                NotifyOfPropertyChange(() => IsManufacturerEditButtonEnable);
            }
        }
        // IsManufacturerDeleteButtonEnable - Để xác định xem nút Delete có được enable hay không
        // - Nếu SelectedManufacturer = null thì nút Delete sẽ không được enable
        private bool _isManufacturerDeleteButtonEnable;
        public bool IsManufacturerDeleteButtonEnable
        {
            get { return _isManufacturerDeleteButtonEnable; }
            set
            {
                _isManufacturerDeleteButtonEnable = value;
                NotifyOfPropertyChange(() => IsManufacturerDeleteButtonEnable);
            }
        }
        // IsManufacturerNewButtonEnable - Để xác định xem nút New có được enable hay không
        // - Nếu IsManufacturerUserEditting = true thì nút New sẽ không được enable
        // - Nếu IsManufacturerUserEditting = true thì nút New của Model sẽ không được enable
        private bool _isManufacturerNewButtonEnable;
        public bool IsManufacturerNewButtonEnable
        {
            get { return _isManufacturerNewButtonEnable; }
            set
            {
                _isManufacturerNewButtonEnable = value;
                NotifyOfPropertyChange(() => IsManufacturerNewButtonEnable);
            }
        }
        // Lưu trạng thái của các nút
        private bool _tempIsManufacturerNewButtonEnable;
        private bool _tempIsManufacturerEditButtonEnable;
        private bool _tempIsManufacturerDeleteButtonEnable;
        private bool _tempIsManufacturerEnable;
        #endregion
        //-------------------------------------------------------------------------------------
        #region Model Setting Properties
        // Model properties
        private ObservableCollection<Model> _Models; // Dùng để hiển thị dữ liệu
        public ObservableCollection<Model> Models
        {
            get { return _Models; }
            set
            {
                _Models = value;
                NotifyOfPropertyChange(() => Models);
            }
        }
        private List<Model> _tempModel; // Dùng để lưu trữ dữ liệu khi người dùng editting

        private Model _selectedModel;
        public Model SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                NotifyOfPropertyChange(() => SelectedModel);
                SetModelDeleteButtonEnable();
                IsModelEditButtonEnable = SelectedModel != null;
            }
        }
        // IsModelUserEditting - KEY - Để xác định xem người dùng đang editting hay không
        // - Gía trị ngươc lại với IsModelReadOnly
        // - Cho phép nút Ok và Cancel hiện lên khi IsModelUserEditting = true
        private bool _isModelUserEditting;
        public bool IsModelUserEditting
        {
            get { return _isModelUserEditting; }
            set
            {
                _isModelUserEditting = value;
                NotifyOfPropertyChange(() => IsModelUserEditting);
                IsModelReadOnly = !value;
                IsModelNewButtonEnable = !value;
                SetModelDeleteButtonEnable();
            }
        }
        // IsModelReadOnly - Để xác định xem người dùng có thể chỉnh sửa ModelDataGrid hay không
        // - Cho phép nút Edit hiện lên khi IsModelReadOnly = true
        private bool _isModelReadOnly;
        public bool IsModelReadOnly
        {
            get { return _isModelReadOnly; }
            set
            {
                _isModelReadOnly = value;
                NotifyOfPropertyChange(() => IsModelReadOnly);
            }
        }
        // IsModelEnable - Để xác định xem người dùng có thể chỉnh sửa ModelDataGrid hay không
        private bool _isModelEnable;
        public bool IsModelEnable
        {
            get { return _isModelEnable; }
            set
            {
                _isModelEnable = value;
                NotifyOfPropertyChange(() => IsModelEnable);
            }
        }


        // Button Properties

        // IsModelEditButtonEnable - Để xác định xem nút Edit có được enable hay không
        // - Nếu ModelDataGrid không có dữ liệu thì nút Edit sẽ không được enable
        private bool _isModelEditButtonEnable;
        public bool IsModelEditButtonEnable
        {
            get { return _isModelEditButtonEnable; }
            set
            {
                _isModelEditButtonEnable = value;
                NotifyOfPropertyChange(() => IsModelEditButtonEnable);
            }
        }
        // IsModelDeleteButtonEnable - Để xác định xem nút Delete có được enable hay không
        // - Nếu SelectedModel = null thì nút Delete sẽ không được enable
        private bool _isModelDeleteButtonEnable;
        public bool IsModelDeleteButtonEnable
        {
            get { return _isModelDeleteButtonEnable; }
            set
            {
                _isModelDeleteButtonEnable = value;
                NotifyOfPropertyChange(() => IsModelDeleteButtonEnable);
            }
        }
        // IsModelNewButtonEnable - Để xác định xem nút New có được enable hay không
        // - Nếu IsModelUserEditting = true thì nút New sẽ không được enable
        // - Nếu Models = null thì nút New sẽ được enable
        private bool _isModelNewButtonEnable;
        public bool IsModelNewButtonEnable
        {
            get { return _isModelNewButtonEnable; }
            set
            {
                _isModelNewButtonEnable = value;
                NotifyOfPropertyChange(() => IsModelNewButtonEnable);
            }
        }
        // Lưu trạng thái của các nút
        private bool _tempIsModelNewButtonEnable;
        private bool _tempIsModelEditButtonEnable;
        private bool _tempIsModelDeleteButtonEnable;
        private bool _tempIsModelEnable;
        #endregion
        // Methods
        // Load Data
        public void LoadData()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers.OrderBy(x => x.Name));
            Countries = new ObservableCollection<Country>(CachesServices.Instance.Countries.OrderBy(x => x.Name));
            ModelTypes = new ObservableCollection<ModelType>(CachesServices.Instance.ModelTypes.FindAll(x => x.IsConsumable == false).OrderBy(x => x.Type));
        }

        // Load Model theo Selected Manufacturer
        public void LoadModel()
        {
            if (SelectedManufacturer != null)
            {
                ModelTypes = new ObservableCollection<ModelType>(CachesServices.Instance.ModelTypes.FindAll(x => x.IsConsumable == false).OrderBy(x => x.Type));
                if (IsConsumable)
                {
                    Models = new ObservableCollection<Model>(CachesServices.Instance.Models.FindAll(x => x.ManufacturerId == SelectedManufacturer.Id && x.IsConsumable == true).OrderBy(x => x.Name));

                }
                else
                {
                    Models = new ObservableCollection<Model>(CachesServices.Instance.Models.FindAll(x => x.ManufacturerId == SelectedManufacturer.Id && x.IsConsumable == false).OrderBy(x => x.Name));
                }
            }
        }

        // Set delete button enable
        public void SetManufacturerDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedManufacturer != null và IsManufacturerUserEditting = false
            IsManufacturerDeleteButtonEnable = SelectedManufacturer != null && !IsManufacturerUserEditting;
        }
        public void SetModelDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedModel != null và IsModelUserEditting = false
            IsModelDeleteButtonEnable = SelectedModel != null && !IsModelUserEditting;
        }
        // Button Click
        //-------------------------------------------------------------------------------------
        #region Button Click Manufacturer
        // Edit Button Click
        public void bnManufacturerEdit()
        {
            // Lưu trữ dữ liệu ban đầu
            _tempManufacturer = Manufacturers.Select(c => c.Clone()).ToList();
            IsManufacturerUserEditting = true;
            // Lưu trạng thái của các nút Model
            _tempIsModelDeleteButtonEnable = IsModelDeleteButtonEnable;
            _tempIsModelNewButtonEnable = IsModelNewButtonEnable;
            _tempIsModelEditButtonEnable = IsModelEditButtonEnable;
            _tempIsModelEnable = IsModelEnable;
            // Khóa các nút Model
            IsModelDeleteButtonEnable = false;
            IsModelNewButtonEnable = false;
            IsModelEditButtonEnable = false;
            IsModelEnable = false;
        }
        // Save Button Click
        public async void bnManufacturerSave()
        {
            // Lưu dữ liệu hiển thị
            IsManufacturerUserEditting = false;

            ObservableCollection<Manufacturer> temp = new ObservableCollection<Manufacturer>(Manufacturers);
            foreach (var item in temp)
            {
                item.CountryId = item.Country.Id;
                item.Country = null;
            }
            // Lưu dữ liệu vào Database
            await new SaveDataServices().SaveManufacturerListAsync(temp.ToList());
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            LoadData();
            // Trả lại trạng thái của các nút Model
            IsModelDeleteButtonEnable = _tempIsModelDeleteButtonEnable;
            IsModelNewButtonEnable = _tempIsModelNewButtonEnable;
            IsModelEditButtonEnable = _tempIsModelEditButtonEnable;
            IsModelEnable = _tempIsModelEnable;
        }
        // Cancel Button Click
        public void bnManufacturerCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu
            SelectedManufacturer = null;
            Manufacturers = new ObservableCollection<Manufacturer>(_tempManufacturer.Select(c => c.Clone()).ToList());
            IsManufacturerUserEditting = false;
            // Trả lại trạng thái của các nút Model
            IsModelDeleteButtonEnable = _tempIsModelDeleteButtonEnable;
            IsModelNewButtonEnable = _tempIsModelNewButtonEnable;
            IsModelEditButtonEnable = _tempIsModelEditButtonEnable;
            IsModelEnable = _tempIsModelEnable;
        }
        // New Button Click
        public void bnManufacturerNew()
        {
            _tempManufacturer = Manufacturers.Select(c => c.Clone()).ToList(); // Lưu trữ dữ liệu ban đầu
            Manufacturers.Add(new Manufacturer
            {
                Name = "New Manufacturer",
                Descriptions = "",
                CountryId = 1,
                Country = Countries.FirstOrDefault(c => c.Id == 1)
            });
            NotifyOfPropertyChange(() => Manufacturers);
            SelectedManufacturer = Manufacturers[Manufacturers.Count - 1];
            NotifyOfPropertyChange(() => SelectedManufacturer);
            IsManufacturerUserEditting = true;
            // Lưu trạng thái của các nút Model
            _tempIsModelDeleteButtonEnable = IsModelDeleteButtonEnable;
            _tempIsModelNewButtonEnable = IsModelNewButtonEnable;
            _tempIsModelEditButtonEnable = IsModelEditButtonEnable;
            _tempIsModelEnable = IsModelEnable;
            // Khóa các nút Model
            IsModelDeleteButtonEnable = false;
            IsModelNewButtonEnable = false;
            IsModelEditButtonEnable = false;
            IsModelEnable = false;
        }
        // Delete Button Click
        public async void bnManufacturerDelete()
        {
            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "Are you sure!!",
                TxtMessage = "Do you want to Permanent Delete this Manufacturer?",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                if (SelectedManufacturer != null)
                {
                    await new DeleteDataServices().DeleteManufacturerAsync(SelectedManufacturer);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    LoadData();
                    Manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers);
                    SelectedManufacturer = null;
                }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------
        #region Button Click Model
        // Edit Button Click
        public void bnModelEdit()
        {
            // Lưu trữ dữ liệu ban đầu
            _tempModel = Models.Select(c => c.Clone()).ToList();
            IsModelUserEditting = true;
            // Lưu trạng thái của các nút Manufacturer
            _tempIsManufacturerDeleteButtonEnable = IsManufacturerDeleteButtonEnable;
            _tempIsManufacturerNewButtonEnable = IsManufacturerNewButtonEnable;
            _tempIsManufacturerEditButtonEnable = IsManufacturerEditButtonEnable;
            _tempIsManufacturerEnable = IsManufacturerEnable;
            // Deactive các nút Manufacturer
            IsManufacturerDeleteButtonEnable = false;
            IsManufacturerNewButtonEnable = false;
            IsManufacturerEditButtonEnable = false;
            IsManufacturerEnable = false;
        }
        // Save Button Click
        public async void bnModelSave()
        {
            // Kiểm tra dữ liệu trước khi lưu
            foreach (var item in Models)
            {
                if (item.ModelType == null)
                {
                    var customMessageBoxViewSpecification = new CustomMessageBoxViewModel
                    {
                        Message = "Error",
                        TxtMessage = "Chưa chọn loại thiết bị",
                        IsInformation = true
                    };
                    var windowManager = new WindowManager();
                    await windowManager.ShowDialogAsync(customMessageBoxViewSpecification);
                    return;
                }
            }
            // Lưu dữ liệu hiển thị
            IsModelUserEditting = false;
            Manufacturer tempSelectedManufaturer = SelectedManufacturer;
            // Lưu dữ liệu hiển thị
            ObservableCollection<Model> temp = new ObservableCollection<Model>(Models);
            foreach (var item in temp)
            {
                item.ManufacturerId = SelectedManufacturer.Id;
                item.Manufacturer = null;
                item.ModelTypeId = item.ModelType.Id;
                item.ModelType = null;
                item.IsConsumable = IsConsumable;
            }
            // Lưu dữ liệu vào database
            await new SaveDataServices().SaveModelListAsync(temp.ToList());
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Load lại dữ liệu
            LoadModel();
            // Cập nhật lại dữ liệu trong Model
            NotifyOfPropertyChange(() => SearchManufacturer);
            SelectedManufacturer = tempSelectedManufaturer;
            // Trả lại trạng thái của các nút Manufacturer
            IsManufacturerDeleteButtonEnable = _tempIsManufacturerDeleteButtonEnable;
            IsManufacturerNewButtonEnable = _tempIsManufacturerNewButtonEnable;
            IsManufacturerEditButtonEnable = _tempIsManufacturerEditButtonEnable;
            IsManufacturerEnable = _tempIsManufacturerEnable;
        }
        // Cancel Button Click
        public void bnModelCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu
            Models = new ObservableCollection<Model>(_tempModel.Select(c => c.Clone()).ToList());
            IsModelUserEditting = false;
            // Trả lại trạng thái của các nút Manufacturer
            IsManufacturerDeleteButtonEnable = _tempIsManufacturerDeleteButtonEnable;
            IsManufacturerNewButtonEnable = _tempIsManufacturerNewButtonEnable;
            IsManufacturerEditButtonEnable = _tempIsManufacturerEditButtonEnable;
            IsManufacturerEnable = _tempIsManufacturerEnable;
        }
        // New Button Click
        public void bnModelNew()
        {
            _tempModel = Models.Select(c => c.Clone()).ToList(); // Lưu trữ dữ liệu ban đầu
            Models.Add(new Model
            {
                Name = "New Model",
                Descriptions = "",
                ManufacturerId = SelectedManufacturer.Id,
                Manufacturer = SelectedManufacturer,

            });
            NotifyOfPropertyChange(() => Models);
            SelectedModel = Models[Models.Count - 1];
            IsModelUserEditting = true;
            // Lưu trạng thái của các nút Manufacturer
            _tempIsManufacturerDeleteButtonEnable = IsManufacturerDeleteButtonEnable;
            _tempIsManufacturerNewButtonEnable = IsManufacturerNewButtonEnable;
            _tempIsManufacturerEditButtonEnable = IsManufacturerEditButtonEnable;
            _tempIsManufacturerEnable = IsManufacturerEnable;
            // Deactive các nút Manufacturer
            IsManufacturerDeleteButtonEnable = false;
            IsManufacturerNewButtonEnable = false;
            IsManufacturerEditButtonEnable = false;
            IsManufacturerEnable = false;
        }
        // Delete Button Click
        public async void bnModelDelete()
        {
            Manufacturer tempSelectedManufaturer = SelectedManufacturer;
            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "Are you sure!!",
                TxtMessage = "Do you want to Permanent Delete this Model?",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                if (SelectedModel != null)
                {
                    await new DeleteDataServices().DeleteModelAsync(SelectedModel);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    // Cập nhật lại dữ liệu trong Model
                    NotifyOfPropertyChange(() => SearchManufacturer);
                    SelectedManufacturer = tempSelectedManufaturer;
                    SelectedModel = null;
                }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------
        // Các phương pháp tìm kiếm
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
                    _tempModel = new List<Model>(Models); // Lưu lại các Model đã được load khi chọn manufacturer
                    Models = new ObservableCollection<Model>(_tempModel.Where(c => c.Name.ToLower().Contains(SearchModel.ToLower())).OrderBy(x => x.Name));
                }
            }
        }
        //-------------------------------------------------------------------------------------
        // Xem chi tiết InstrumentModel
        public async Task OpenModelspecificationsViewCommand()
        {
            if (CachesServices.Instance.Models.Find(x => x.Id == SelectedModel.Id) == null)
            {
                NotificationService.ShowError("Chưa hoàn tất lưu thiết bị mới");
                return;
            }
            await _productCatalogViewModel.ModelSpecificationView(CachesServices.Instance.Models.Find(x => x.Id == SelectedModel.Id));
        }
    }
}
