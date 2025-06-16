using Caliburn.Micro;
using Client.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Client.Entities.ModelEntities;

namespace Client.ViewModels
{
    public class ModelSpecificationViewModel : Screen
    {
        // Properties
        private Model _model;
        public Model Model
        {
            get { return _model; }
            set
            {
                _model = value;
                NotifyOfPropertyChange(() => Model);
            }
        }
        private ObservableCollection<ModelSpecification> _modelSpecifications;
        public ObservableCollection<ModelSpecification> ModelSpecifications
        {
            get { return _modelSpecifications; }
            set
            {
                _modelSpecifications = value;
                NotifyOfPropertyChange(() => ModelSpecifications);
            }
        }

        private ObservableCollection<ModelFile> _modelFiles;
        public ObservableCollection<ModelFile> ModelFiles
        {
            get { return _modelFiles; }
            set
            {
                _modelFiles = value;
                NotifyOfPropertyChange(() => ModelFiles);
                if (ModelFiles.Count == 0)
                {
                    IsSpecificationNewButtonEnable = false;
                }
                else
                {
                    IsSpecificationNewButtonEnable = true;
                }
            }
        }
        private ModelFile _selectedModelFile;
        public ModelFile SelectedModelFile
        {
            get { return _selectedModelFile; }
            set
            {
                _selectedModelFile = value;
                NotifyOfPropertyChange(() => SelectedModelFile);
            }
        }
        // Gán giá trị ban đầu cho combobox SpecificationFilter
        private List<string> _specificationFilter = new List<string> { "Tính năng kỹ thuật", "Trích dẫn", "Trang tham khảo" };
        public List<string> SpecificationFilter
        {
            get { return _specificationFilter; }
            set
            {
                _specificationFilter = value;
                NotifyOfPropertyChange(() => SpecificationFilter);
            }
        }
        private string _selectedSpecificationFilter;
        public string SelectedSpecificationFilter
        {
            get { return _selectedSpecificationFilter; }
            set
            {
                _selectedSpecificationFilter = value;
                NotifyOfPropertyChange(() => SelectedSpecificationFilter);
            }
        }
        private string _specificationSearchText;
        public string SpecificationSearchText
        {
            get { return _specificationSearchText; }
            set
            {
                _specificationSearchText = value;
                NotifyOfPropertyChange(() => SpecificationSearchText);
                LoadSpecification();
            }
        }
        //Làm cho cột File Hiển thị lên màn hình
        private bool _isSecondColumnVisible = true;
        public bool IsSecondColumnVisible
        {
            get { return _isSecondColumnVisible; }
            set
            {
                _isSecondColumnVisible = value;
                NotifyOfPropertyChange(() => IsSecondColumnVisible);
            }
        }
        public void ToggleUploadFile()
        {
            IsSecondColumnVisible = !IsSecondColumnVisible;
        }
        // Làm cho cột Specification Hiển thị lên màn hình
        private bool _isSpecificationGroupColumnVisible = true;
        public bool IsSpecificationGroupColumnVisible
        {
            get { return _isSpecificationGroupColumnVisible; }
            set
            {
                _isSpecificationGroupColumnVisible = value;
                NotifyOfPropertyChange(() => IsSpecificationGroupColumnVisible);
            }
        }
        public void ToggleSpecificationGroup()
        {
            IsSpecificationGroupColumnVisible = !IsSpecificationGroupColumnVisible;
        }
        // Làm cho header hiển thị
        private bool _isHeaderRowVisible;
        public bool IsHeaderRowVisible
        {
            get { return _isHeaderRowVisible; }
            set
            {
                _isHeaderRowVisible = value;
                NotifyOfPropertyChange(() => IsHeaderRowVisible);
                IsHeaderRowNotVisible = !IsHeaderRowVisible;
            }
        }
        private bool _isHeaderRowNotVisible;
        public bool IsHeaderRowNotVisible
        {
            get { return _isHeaderRowNotVisible; }
            set
            {
                _isHeaderRowNotVisible = value;
                NotifyOfPropertyChange(() => IsHeaderRowNotVisible);
            }
        }
        public void GridExpand()
        {
            IsHeaderRowVisible = false;
            IsSecondColumnVisible = false;
            IsSpecificationGroupColumnVisible = false;
        }
        public void GridCollapse()
        {
            IsHeaderRowVisible = true;
        }


        //-------------------------------------------------------------------------------------
        #region ModelSpecificationGroup Setting Properties
        // ModelSpecificationGroup properties
        private List<ModelSpecificationGroup> _tempModelSpecificationGroup; // Dùng để lưu trữ dữ liệu khi người dùng editting

        private ObservableCollection<ModelSpecificationGroup> _ModelSpecificationGroups; // Dùng để hiển thị dữ liệu
        public ObservableCollection<ModelSpecificationGroup> ModelSpecificationGroups
        {
            get { return _ModelSpecificationGroups; }
            set
            {
                _ModelSpecificationGroups = value;
                NotifyOfPropertyChange(() => ModelSpecificationGroups);
                IsModelSpecificationGroupEditButtonEnable = ModelSpecificationGroups.Count > 0;
            }
        }

        private ModelSpecificationGroup _selectedModelSpecificationGroup;
        public ModelSpecificationGroup SelectedModelSpecificationGroup
        {
            get { return _selectedModelSpecificationGroup; }
            set
            {
                _selectedModelSpecificationGroup = value;
                NotifyOfPropertyChange(() => SelectedModelSpecificationGroup);
                if (SelectedModelSpecificationGroup != null)
                {
                    // Đưa dữ liệu về trạng thái ban đầu
                    IsSpecificationUserEditting = false;
                    IsSpecificationNewButtonEnable = true;
                }
                else
                {
                    IsSpecificationNewButtonEnable = false;
                }
                LoadSpecification();
                SetModelSpecificationGroupDeleteButtonEnable();
            }
        }

        // IsModelSpecificationGroupUserEditting - KEY - Để xác định xem người dùng đang editting hay không
        // - Gía trị ngươc lại với IsModelSpecificationGroupReadOnly
        // - Cho phép nút Ok và Cancel hiện lên khi IsModelSpecificationGroupUserEditting = true
        private bool _isModelSpecificationGroupUserEditting;
        public bool IsModelSpecificationGroupUserEditting
        {
            get { return _isModelSpecificationGroupUserEditting; }
            set
            {
                _isModelSpecificationGroupUserEditting = value;
                NotifyOfPropertyChange(() => IsModelSpecificationGroupUserEditting);
                IsModelSpecificationGroupReadOnly = !value;
                IsModelSpecificationGroupNewButtonEnable = !value;
                IsSpecificationNewButtonEnable = !value;
                SetModelSpecificationGroupDeleteButtonEnable();
            }
        }
        // IsModelSpecificationGroupReadOnly - Để xác định xem người dùng có thể chỉnh sửa ModelSpecificationGroupDataGrid hay không
        // - Cho phép nút Edit hiện lên khi IsModelSpecificationGroupReadOnly = true
        private bool _isModelSpecificationGroupReadOnly;
        public bool IsModelSpecificationGroupReadOnly
        {
            get { return _isModelSpecificationGroupReadOnly; }
            set
            {
                _isModelSpecificationGroupReadOnly = value;
                NotifyOfPropertyChange(() => IsModelSpecificationGroupReadOnly);

            }
        }
        // IsModelSpecificationGroupEnable - Để xác định xem người dùng có thể chỉnh sửa ModelSpecificationGroupDataGrid hay không
        private bool _isModelSpecificationGroupEnable;
        public bool IsModelSpecificationGroupEnable
        {
            get { return _isModelSpecificationGroupEnable; }
            set
            {
                _isModelSpecificationGroupEnable = value;
                NotifyOfPropertyChange(() => IsModelSpecificationGroupEnable);
            }
        }

        // Button Properties

        // IsModelSpecificationGroupEditButtonEnable - Để xác định xem nút Edit có được enable hay không
        // - Nếu ModelSpecificationGroupDataGrid không có dữ liệu thì nút Edit sẽ không được enable
        private bool _isModelSpecificationGroupEditButtonEnable;
        public bool IsModelSpecificationGroupEditButtonEnable
        {
            get { return _isModelSpecificationGroupEditButtonEnable; }
            set
            {
                _isModelSpecificationGroupEditButtonEnable = value;
                NotifyOfPropertyChange(() => IsModelSpecificationGroupEditButtonEnable);
            }
        }
        // IsModelSpecificationGroupDeleteButtonEnable - Để xác định xem nút Delete có được enable hay không
        // - Nếu SelectedModelSpecificationGroup = null thì nút Delete sẽ không được enable
        private bool _isModelSpecificationGroupDeleteButtonEnable;
        public bool IsModelSpecificationGroupDeleteButtonEnable
        {
            get { return _isModelSpecificationGroupDeleteButtonEnable; }
            set
            {
                _isModelSpecificationGroupDeleteButtonEnable = value;
                NotifyOfPropertyChange(() => IsModelSpecificationGroupDeleteButtonEnable);
            }
        }
        // IsModelSpecificationGroupNewButtonEnable - Để xác định xem nút New có được enable hay không
        // - Nếu IsModelSpecificationGroupUserEditting = true thì nút New sẽ không được enable
        // - Nếu IsModelSpecificationGroupUserEditting = true thì nút New của Model sẽ không được enable
        private bool _isModelSpecificationGroupNewButtonEnable;
        public bool IsModelSpecificationGroupNewButtonEnable
        {
            get { return _isModelSpecificationGroupNewButtonEnable; }
            set
            {
                _isModelSpecificationGroupNewButtonEnable = value;
                NotifyOfPropertyChange(() => IsModelSpecificationGroupNewButtonEnable);
            }
        }
        // Lưu trạng thái của các nút
        private bool _tempIsModelSpecificationGroupNewButtonEnable;
        private bool _tempIsModelSpecificationGroupEditButtonEnable;
        private bool _tempIsModelSpecificationGroupDeleteButtonEnable;
        private bool _tempIsModelSpecificationGroupEnable;
        #endregion
        //-------------------------------------------------------------------------------------
        #region ModelSpecification Setting Properties
        private List<ModelSpecification> _tempSpecification; // Dùng để lưu trữ dữ liệu khi người dùng editting

        private ModelSpecification _selectedSpecification;
        public ModelSpecification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
                NotifyOfPropertyChange(() => SelectedSpecification);
                SetSpecificationDeleteButtonEnable();
                IsSpecificationEditButtonEnable = SelectedSpecification != null;
            }
        }
        // IsSpecificationUserEditting - KEY - Để xác định xem người dùng đang editting hay không
        // - Gía trị ngươc lại với IsSpecificationReadOnly
        // - Cho phép nút Ok và Cancel hiện lên khi IsSpecificationUserEditting = true
        private bool _isSpecificationUserEditting;
        public bool IsSpecificationUserEditting
        {
            get { return _isSpecificationUserEditting; }
            set
            {
                _isSpecificationUserEditting = value;
                NotifyOfPropertyChange(() => IsSpecificationUserEditting);
                IsSpecificationReadOnly = !value;
                IsSpecificationNewButtonEnable = !value;
                SetSpecificationDeleteButtonEnable();
            }
        }
        // IsSpecificationReadOnly - Để xác định xem người dùng có thể chỉnh sửa SpecificationDataGrid hay không
        // - Cho phép nút Edit hiện lên khi IsSpecificationReadOnly = true
        private bool _isSpecificationReadOnly;
        public bool IsSpecificationReadOnly
        {
            get { return _isSpecificationReadOnly; }
            set
            {
                _isSpecificationReadOnly = value;
                NotifyOfPropertyChange(() => IsSpecificationReadOnly);
            }
        }
        // IsSpecificationEnable - Để xác định xem người dùng có thể chỉnh sửa SpecificationDataGrid hay không
        private bool _isSpecificationEnable;
        public bool IsSpecificationEnable
        {
            get { return _isSpecificationEnable; }
            set
            {
                _isSpecificationEnable = value;
                NotifyOfPropertyChange(() => IsSpecificationEnable);
            }
        }


        // Button Properties

        // IsSpecificationEditButtonEnable - Để xác định xem nút Edit có được enable hay không
        // - Nếu SpecificationDataGrid không có dữ liệu thì nút Edit sẽ không được enable
        private bool _isSpecificationEditButtonEnable;
        public bool IsSpecificationEditButtonEnable
        {
            get { return _isSpecificationEditButtonEnable; }
            set
            {
                _isSpecificationEditButtonEnable = value;
                NotifyOfPropertyChange(() => IsSpecificationEditButtonEnable);
            }
        }
        // IsSpecificationDeleteButtonEnable - Để xác định xem nút Delete có được enable hay không
        // - Nếu SelectedSpecification = null thì nút Delete sẽ không được enable
        private bool _isSpecificationDeleteButtonEnable;
        public bool IsSpecificationDeleteButtonEnable
        {
            get { return _isSpecificationDeleteButtonEnable; }
            set
            {
                _isSpecificationDeleteButtonEnable = value;
                NotifyOfPropertyChange(() => IsSpecificationDeleteButtonEnable);
            }
        }
        // IsSpecificationNewButtonEnable - Để xác định xem nút New có được enable hay không
        // - Nếu IsSpecificationUserEditting = true thì nút New sẽ không được enable
        // - Nếu ModelFiles = null thì nút New sẽ không được enable
        private bool _isSpecificationNewButtonEnable;
        public bool IsSpecificationNewButtonEnable
        {
            get { return _isSpecificationNewButtonEnable; }
            set
            {
                _isSpecificationNewButtonEnable = value;
                NotifyOfPropertyChange(() => IsSpecificationNewButtonEnable);
            }
        }
        // Lưu trạng thái của các nút
        private bool _tempIsSpecificationNewButtonEnable;
        private bool _tempIsSpecificationEditButtonEnable;
        private bool _tempIsSpecificationDeleteButtonEnable;
        private bool _tempIsSpecificationEnable;
        #endregion
        //-------------------------------------------------------------------------------------
        // Constructor
        private readonly SpecificationViewModel _specificationViewModel;
        public ModelSpecificationViewModel(SpecificationViewModel specificationViewModel, Model model)
        {
            IsHeaderRowVisible = true;

            IsModelSpecificationGroupUserEditting = false;
            IsSpecificationUserEditting = false;

            IsSpecificationNewButtonEnable = false;
            IsSpecificationEnable = true;
            IsModelSpecificationGroupEnable = true;

            // Gán giá trị cho các properties
            _specificationViewModel = specificationViewModel;
            Model = model;
            LoadData();
            ToggleViewAllSpecification();
        }
        //-------------------------------------------------------------------------------------
        // Xem toàn bộ dữ liệu
        public void ToggleViewAllSpecification()
        {
            SelectedModelSpecificationGroup = null;
        }
        // Load Data
        public void LoadData()
        {
            ModelSpecificationGroups = new ObservableCollection<ModelSpecificationGroup>(CachesServices.Instance.ModelSpecificationGroups.Where(x => x.ModelId == Model.Id).OrderBy(x => x.Name));
            ModelFiles = new ObservableCollection<ModelFile>(CachesServices.Instance.ModelFiles.Where(x => x.ModelId == Model.Id));
        }
        // Load Specification theo Selected ModelSpecificationGroup
        public void LoadSpecification()
        {
            ModelFiles = new ObservableCollection<ModelFile>(CachesServices.Instance.ModelFiles.Where(x => x.ModelId == Model.Id));

            if (SelectedModelSpecificationGroup != null && SpecificationSearchText != null)
            {
                switch (SelectedSpecificationFilter)
                {
                    case "Tính năng kỹ thuật":
                        ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id && x.ModelSpecificationGroupId == SelectedModelSpecificationGroup.Id && x.Specification.ToLower().Contains(SpecificationSearchText.ToLower())).OrderBy(x => x.Specification));
                        break;
                    case "Trích dẫn":
                        ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id && x.ModelSpecificationGroupId == SelectedModelSpecificationGroup.Id && x.QuoteLine.ToLower().Contains(SpecificationSearchText.ToLower())).OrderBy(x => x.Specification));
                        break;
                    case "Trang tham khảo":
                        ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id && x.ModelSpecificationGroupId == SelectedModelSpecificationGroup.Id && x.ReferenceLine.ToLower().Contains(SpecificationSearchText.ToLower())).OrderBy(x => x.Specification));
                        break;
                    default:
                        ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id && x.ModelSpecificationGroupId == SelectedModelSpecificationGroup.Id && x.Specification.ToLower().Contains(SpecificationSearchText.ToLower())).OrderBy(x => x.Specification));
                        break;
                }
            }
            else if (SelectedModelSpecificationGroup == null && SpecificationSearchText != null)
            {
                switch (SelectedSpecificationFilter)
                {
                    case "Tính năng kỹ thuật":
                        ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id && x.Specification.ToLower().Contains(SpecificationSearchText.ToLower())).OrderBy(x => x.Specification));
                        break;
                    case "Trích dẫn":
                        ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id && x.QuoteLine.ToLower().Contains(SpecificationSearchText.ToLower())).OrderBy(x => x.Specification));
                        break;
                    case "Trang tham khảo":
                        ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id && x.ReferenceLine.ToLower().Contains(SpecificationSearchText.ToLower())).OrderBy(x => x.Specification));
                        break;
                    default:
                        ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id && x.Specification.ToLower().Contains(SpecificationSearchText.ToLower())).OrderBy(x => x.Specification));
                        break;
                }
            }
            else if (SelectedModelSpecificationGroup != null && SpecificationSearchText == null)
            {
                ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id && x.ModelSpecificationGroupId == SelectedModelSpecificationGroup.Id).OrderBy(x => x.Specification));
            }
            else
            {
                ModelSpecifications = new ObservableCollection<ModelSpecification>(CachesServices.Instance.ModelSpecifications.Where(x => x.ModelId == Model.Id).OrderBy(x => x.Specification));
            }

        }
        // Methods
        public void SetModelSpecificationGroupDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedModelSpecificationGroup != null và IsModelSpecificationGroupUserEditting = false
            IsModelSpecificationGroupDeleteButtonEnable = SelectedModelSpecificationGroup != null && !IsModelSpecificationGroupUserEditting;
        }
        public void SetSpecificationDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedSpecification != null và IsSpecificationUserEditting = false
            IsSpecificationDeleteButtonEnable = SelectedSpecification != null && !IsSpecificationUserEditting;
        }
        //-------------------------------------------------------------------------------------
        // Files Handling
        public async Task UploadImageCommand()
        {
            // Mở hộp thoại chọn file
            var openFileDialog = new OpenFileDialog();

            openFileDialog = new Microsoft.Win32.OpenFileDialog();
            // Lọc các file ảnh
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp, *.dib, *.gif, *.tif, *.tiff)|*.jpg;*.jpeg;*.jpe;*.jfif;*.png;*.bmp;*.dib;*.gif;*.tif;*.tiff";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();

            Model model = new Model();
            model = Model;

            model.ImagePath = openFileDialog.FileName;
            model.Image = System.IO.File.ReadAllBytes(openFileDialog.FileName);
            model.Manufacturer = null;
            model.ModelType = null;

            await new SaveDataServices().SaveModelAsync(model);
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            this.Model = CachesServices.Instance.Models.FirstOrDefault(x => x.Id == Model.Id);
            // Cập nhật lại dữ liệu trong Specification
            LoadData();
        }
        public async Task UploadFile()
        {
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(new ModelFileSelectorViewModel(this,Model.Id));
            Model = CachesServices.Instance.Models.FirstOrDefault(x => x.Id == Model.Id);
            // Update Caches
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            Model = CachesServices.Instance.Models.FirstOrDefault(x => x.Id == Model.Id);
            LoadData();
        }
        // Double Click to OpenFile
        public void OpenFile()
        {
            try
            {
                new FileServices().OpenFile(SelectedModelFile.FilePath);
            }
            catch (System.Exception ex)
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = ex.Message,
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                windowManager.ShowDialogAsync(customMessageBoxViewModel);
            }

        }
        public async Task ViewFile()
        {
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(new ModelFileEditViewModel(SelectedModelFile));
            // Update Caches
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            Model = CachesServices.Instance.Models.FirstOrDefault(x => x.Id == Model.Id);
            LoadData();
        }
        public async Task DeleteFile()
        {
            var windowManager = new WindowManager();
            // Kiểm tra có Specification nào đang sử dụng File này không
            if (CachesServices.Instance.ModelSpecifications.Any(x => x.ModelFileId == SelectedModelFile.Id))
            {
                var errorMessageBox = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = "File này đang được sử dụng, không thể xóa",
                    IsInformation = true
                };
                await windowManager.ShowDialogAsync(errorMessageBox);
                return;
            }

            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "Xác nhận!!",
                TxtMessage = "Bạn có muốn xóa File này không?",
                IsConfirmation = true
            };
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                if (SelectedModelFile != null)
                {
                    await new DeleteDataServices().DeleteModelFileAsync(SelectedModelFile);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    Model = CachesServices.Instance.Models.FirstOrDefault(x => x.Id == Model.Id);
                    LoadData();
                    SelectedModelFile = null;
                }
            }
        }
        public void OpenFolder()
        {
            try
            {
                // Đường dẫn tới thư mục bạn muốn mở
                string folderPath = System.IO.Path.Combine(Properties.Settings.Default.SaveFileFolderPath, Model.Manufacturer.Name, Model.Name);

                // Mở thư mục bằng Process
                Process.Start(new ProcessStartInfo()
                {
                    FileName = folderPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra
                MessageBox.Show($"An error occurred while trying to open the folder: {ex.Message}");
            }
        }

        //-------------------------------------------------------------------------------------
        #region Button Click ModelSpecificationGroup
        // Edit Button Click
        public void bnModelSpecificationGroupEdit()
        {
            // Lưu trữ dữ liệu ban đầu
            _tempModelSpecificationGroup = ModelSpecificationGroups.ToList();
            IsModelSpecificationGroupUserEditting = true;
            // Lưu trạng thái của các nút Specification
            _tempIsSpecificationDeleteButtonEnable = IsSpecificationDeleteButtonEnable;
            _tempIsSpecificationNewButtonEnable = IsSpecificationNewButtonEnable;
            _tempIsSpecificationEditButtonEnable = IsSpecificationEditButtonEnable;
            _tempIsSpecificationEnable = IsSpecificationEnable;
            // Khóa các nút Specification
            IsSpecificationDeleteButtonEnable = false;
            IsSpecificationNewButtonEnable = false;
            IsSpecificationEditButtonEnable = false;
            IsSpecificationEnable = false;
        }
        // Save Button Click
        public async void bnModelSpecificationGroupSave()
        {
            // Lưu dữ liệu hiển thị
            IsModelSpecificationGroupUserEditting = false;

            ObservableCollection<ModelSpecificationGroup> temp = new ObservableCollection<ModelSpecificationGroup>(ModelSpecificationGroups);
            foreach (var item in temp)
            {
                item.ModelId = Model.Id;
                item.Model = null;
                item.IsBase = false;
            }
            // Lưu dữ liệu vào Database
            await new SaveDataServices().SaveModelSpecificationGroupListAsync(temp.ToList());
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            LoadData();
            // Trả lại trạng thái của các nút Specification
            IsSpecificationDeleteButtonEnable = _tempIsSpecificationDeleteButtonEnable;
            IsSpecificationNewButtonEnable = _tempIsSpecificationNewButtonEnable;
            IsSpecificationEditButtonEnable = _tempIsSpecificationEditButtonEnable;
            IsSpecificationEnable = _tempIsSpecificationEnable;
        }
        // Cancel Button Click
        public void bnModelSpecificationGroupCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu
            SelectedModelSpecificationGroup = null;
            ModelSpecificationGroups = new ObservableCollection<ModelSpecificationGroup>(_tempModelSpecificationGroup);
            IsModelSpecificationGroupUserEditting = false;
            // Trả lại trạng thái của các nút Specification
            IsSpecificationDeleteButtonEnable = _tempIsSpecificationDeleteButtonEnable;
            IsSpecificationNewButtonEnable = _tempIsSpecificationNewButtonEnable;
            IsSpecificationEditButtonEnable = _tempIsSpecificationEditButtonEnable;
            IsSpecificationEnable = _tempIsSpecificationEnable;
        }
        // New Button Click
        public void bnModelSpecificationGroupNew()
        {
            _tempModelSpecificationGroup = ModelSpecificationGroups.ToList(); // Lưu trữ dữ liệu ban đầu
            ModelSpecificationGroups.Add(new ModelSpecificationGroup
            {
                Name = "Nhóm tính năng mới",
            });
            NotifyOfPropertyChange(() => ModelSpecificationGroups);
            SelectedModelSpecificationGroup = ModelSpecificationGroups[ModelSpecificationGroups.Count - 1];
            NotifyOfPropertyChange(() => SelectedModelSpecificationGroup);
            IsModelSpecificationGroupUserEditting = true;
            // Lưu trạng thái của các nút Specification
            _tempIsSpecificationDeleteButtonEnable = IsSpecificationDeleteButtonEnable;
            _tempIsSpecificationNewButtonEnable = IsSpecificationNewButtonEnable;
            _tempIsSpecificationEditButtonEnable = IsSpecificationEditButtonEnable;
            _tempIsSpecificationEnable = IsSpecificationEnable;
            // Khóa các nút Specification
            IsSpecificationDeleteButtonEnable = false;
            IsSpecificationNewButtonEnable = false;
            IsSpecificationEditButtonEnable = false;
            IsSpecificationEnable = false;
        }
        // Delete Button Click
        public async void bnModelSpecificationGroupDelete()
        {
            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "Xác nhận!!",
                TxtMessage = "Xác nhận xóa " + SelectedModelSpecificationGroup.Name + " ?",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {
                if (SelectedModelSpecificationGroup != null)
                {
                    // Chuyển các ModelSpecification của ModelSpecificationGroup cần xóa về ModelSpecificationGroup mặc định
                    foreach (var item in ModelSpecifications)
                    {
                        item.ModelSpecificationGroupId = CachesServices.Instance.ModelSpecificationGroups.Find(x => x.IsBase && x.ModelId == Model.Id).Id;
                        item.ModelSpecificationGroup = null;
                        item.Model = null;
                        item.ModelFile = null;
                    }
                    await new SaveDataServices().SaveModelSpecificationListAsync(ModelSpecifications.ToList());
                    await new DeleteDataServices().DeleteModelSpecificationGroupAsync(SelectedModelSpecificationGroup);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    LoadData();
                    SelectedModelSpecificationGroup = null;
                }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------
        // Button Methods
        #region Button Click Specification
        // Edit Button Click
        public void bnSpecificationEdit()
        {
            // Lưu trữ dữ liệu ban đầu
            _tempSpecification = ModelSpecifications.Select(c => c.Clone()).ToList();
            IsSpecificationUserEditting = true;
            // Lưu trạng thái của các nút ModelSpecificationGroup
            _tempIsModelSpecificationGroupDeleteButtonEnable = IsModelSpecificationGroupDeleteButtonEnable;
            _tempIsModelSpecificationGroupNewButtonEnable = IsModelSpecificationGroupNewButtonEnable;
            _tempIsModelSpecificationGroupEditButtonEnable = IsModelSpecificationGroupEditButtonEnable;
            _tempIsModelSpecificationGroupEnable = IsModelSpecificationGroupEnable;
            // Deactive các nút ModelSpecificationGroup
            IsModelSpecificationGroupDeleteButtonEnable = false;
            IsModelSpecificationGroupNewButtonEnable = false;
            IsModelSpecificationGroupEditButtonEnable = false;
            IsModelSpecificationGroupEnable = false;
        }
        // Save Button Click
        public async void bnSpecificationSave()
        {
            // Lưu dữ liệu hiển thị

            foreach (var item in ModelSpecifications)
            {
                if (item.ModelFile == null)
                {
                    var customMessageBoxViewSpecification = new CustomMessageBoxViewModel
                    {
                        Message = "Error",
                        TxtMessage = "Chưa chọn file tham chiếu, vui lòng chọn file tham chiếu",
                        IsInformation = true
                    };
                    var windowManager = new WindowManager();
                    await windowManager.ShowDialogAsync(customMessageBoxViewSpecification);
                    return;
                }
            }
            ObservableCollection<ModelSpecification> temp = new ObservableCollection<ModelSpecification>(ModelSpecifications);
            foreach (var item in temp)
            {
                item.ModelId = Model.Id;
                item.Model = null;
                if (SelectedModelSpecificationGroup != null)
                {
                    item.ModelSpecificationGroupId = SelectedModelSpecificationGroup.Id;
                }
                item.ModelSpecificationGroup = null;
                item.ModelFileId = item.ModelFile.Id;
                item.ModelFile = null;
            }
            // Lưu dữ liệu hiển thị
            IsModelSpecificationGroupUserEditting = false;
            ModelSpecificationGroup tempSelectedModelSpecificationGroup = SelectedModelSpecificationGroup;
            // Lưu dữ liệu hiển thị
            IsSpecificationUserEditting = false;
            // Lưu dữ liệu vào database
            await new SaveDataServices().SaveModelSpecificationListAsync(temp.ToList());
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Cập nhật lại dữ liệu trong Specification
            LoadData();
            // Cập nhật lại dữ liệu trong Model
            SelectedModelSpecificationGroup = tempSelectedModelSpecificationGroup;
            // Trả lại trạng thái của các nút ModelSpecificationGroup
            IsModelSpecificationGroupDeleteButtonEnable = _tempIsModelSpecificationGroupDeleteButtonEnable;
            IsModelSpecificationGroupNewButtonEnable = _tempIsModelSpecificationGroupNewButtonEnable;
            IsModelSpecificationGroupEditButtonEnable = _tempIsModelSpecificationGroupEditButtonEnable;
            IsModelSpecificationGroupEnable = _tempIsModelSpecificationGroupEnable;
        }
        // Cancel Button Click
        public void bnSpecificationCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu
            ModelSpecifications = new ObservableCollection<ModelSpecification>(_tempSpecification.Select(c => c.Clone()).ToList());
            IsSpecificationUserEditting = false;
            // Trả lại trạng thái của các nút ModelSpecificationGroup
            IsModelSpecificationGroupDeleteButtonEnable = _tempIsModelSpecificationGroupDeleteButtonEnable;
            IsModelSpecificationGroupNewButtonEnable = _tempIsModelSpecificationGroupNewButtonEnable;
            IsModelSpecificationGroupEditButtonEnable = _tempIsModelSpecificationGroupEditButtonEnable;
            IsModelSpecificationGroupEnable = _tempIsModelSpecificationGroupEnable;
        }
        // New Button Click
        public void bnSpecificationNew()
        {
            if (SelectedModelSpecificationGroup == null)
            {
                var customMessageBoxViewSpecification = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = "Chưa chọn nhóm tính năng, vui lòng chọn nhóm tính năng",
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                windowManager.ShowDialogAsync(customMessageBoxViewSpecification);
                return;
            }
            if (ModelFiles.Count == 0)
            {
                var customMessageBoxViewSpecification = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = "Không có file tham chiếu, vui lòng tải lên file tham chiếu",
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                windowManager.ShowDialogAsync(customMessageBoxViewSpecification);
                return;
            }
            _tempSpecification = ModelSpecifications.Select(c => c.Clone()).ToList(); // Lưu trữ dữ liệu ban đầu
            ModelSpecifications.Add(new ModelSpecification
            {
                Specification = "Tính năng mới",
                ModelId = Model.Id,
                Model = Model,
                ModelSpecificationGroupId = SelectedModelSpecificationGroup.Id,
                ModelSpecificationGroup = SelectedModelSpecificationGroup
            });
            NotifyOfPropertyChange(() => ModelSpecifications);
            SelectedSpecification = ModelSpecifications[ModelSpecifications.Count - 1];
            IsSpecificationUserEditting = true;
            // Lưu trạng thái của các nút ModelSpecificationGroup
            _tempIsModelSpecificationGroupDeleteButtonEnable = IsModelSpecificationGroupDeleteButtonEnable;
            _tempIsModelSpecificationGroupNewButtonEnable = IsModelSpecificationGroupNewButtonEnable;
            _tempIsModelSpecificationGroupEditButtonEnable = IsModelSpecificationGroupEditButtonEnable;
            _tempIsModelSpecificationGroupEnable = IsModelSpecificationGroupEnable;
            // Deactive các nút ModelSpecificationGroup
            IsModelSpecificationGroupDeleteButtonEnable = false;
            IsModelSpecificationGroupNewButtonEnable = false;
            IsModelSpecificationGroupEditButtonEnable = false;
            IsModelSpecificationGroupEnable = false;
        }
        // Delete Button Click
        public async void bnSpecificationDelete()
        {
            ModelSpecificationGroup tempSelectedModelSpecificationGroup = SelectedModelSpecificationGroup;
            // Bật hội thoại xác nhận
            var customMessageBoxViewSpecification = new CustomMessageBoxViewModel
            {
                Message = "Xác nhận!!",
                TxtMessage = "Bạn muốn xóa tính năng này?",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewSpecification);
            if (customMessageBoxViewSpecification.DialogResult == MessageBoxResult.Yes)
            {
                if (SelectedSpecification != null)
                {
                    await new DeleteDataServices().DeleteModelSpecificationAsync(SelectedSpecification);
                    // Cập nhật lại dữ liệu trong Caches lấy từ database
                    CachesServices.ResetInstance();
                    await CachesServices.Instance.GetAllData();
                    // Cập nhật lại dữ liệu trong Specification
                    LoadData();
                    // Cập nhật lại dữ liệu trong Model
                    SelectedModelSpecificationGroup = tempSelectedModelSpecificationGroup;
                    SelectedSpecification = null;
                }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------
        // Move Specification
        
        public async void OnGroupSelected(RoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement fe)
            {
                if (fe.DataContext is ModelSpecificationGroup group)
                {
                    // Chuyển SelectedModelSpecification sang group mới
                    if (SelectedSpecification != null)
                    {
                        ModelSpecification temp = SelectedSpecification;
                        temp.ModelSpecificationGroupId = group.Id;
                        // Lưu dữ liệu vào database
                        temp.ModelSpecificationGroup = null;
                        temp.ModelFile = null;
                        temp.Model = null;

                        ModelSpecificationGroup tempSelectedModelSpecificationGroup = SelectedModelSpecificationGroup;
                        await new SaveDataServices().SaveModelSpecificationAsync(temp);

                        SelectedSpecification = null;
                        // Cập nhật lại dữ liệu trong Caches lấy từ database
                        CachesServices.ResetInstance();
                        await CachesServices.Instance.GetAllData();
                        // Cập nhật lại dữ liệu trong Specification
                        LoadData();

                        SelectedModelSpecificationGroup = tempSelectedModelSpecificationGroup;

                    }
                }
            }
        }
        
    }
    
}
