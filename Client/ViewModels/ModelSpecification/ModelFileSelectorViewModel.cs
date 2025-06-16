using Caliburn.Micro;
using Client.Services;
using System.Linq;
using System.Threading.Tasks;
using Client.Entities.ModelEntities;

namespace Client.ViewModels
{
    public class ModelFileSelectorViewModel : Screen
    {
        private ModelFile _modelFile = new ModelFile();
        public ModelFile ModelFile
        {
            get { return _modelFile; }
            set
            {
                _modelFile = value;
                NotifyOfPropertyChange(() => ModelFile);
            }
        }

        private readonly int _modelId;
        private readonly FileServices fileServices = new FileServices();
        private readonly ModelSpecificationViewModel _modelSpecificationViewModel;
        // Constructor
        public ModelFileSelectorViewModel(ModelSpecificationViewModel modelSpecificationViewModel, int modelId)
        {
            _modelSpecificationViewModel = modelSpecificationViewModel;
            _modelId = modelId;
        }
        
        // Method 
        public async Task BrowseFileCommandAsync()
        {
            ModelFile = fileServices.OpenModelFile();
            if (ModelFile != null)
            {
                ModelFile duplicateFile = CachesServices.Instance.ModelFiles.FirstOrDefault(x => x.SafeFileName == ModelFile.SafeFileName);
                // Kiểm tra file đã tồn tại chưa
                if (duplicateFile == null) { return; }
                if (duplicateFile.ModelId == _modelId)
                {
                    // Show custom message box
                    // Thông báo lỗi
                    var customMessageBoxViewModel = new CustomMessageBoxViewModel
                    {
                        Message = "Lỗi",
                        TxtMessage = "File đã tồn tại",
                        IsInformation = true
                    };
                    var windowManager = new WindowManager();
                    await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                    await this.TryCloseAsync();
                }
            }
        }
        
        public async void ButtonOk()
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(ModelFile.FileName) || string.IsNullOrEmpty(ModelFile.FileCode) || string.IsNullOrEmpty(ModelFile.FileType))
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Lỗi",
                    TxtMessage = "Chưa điền đầy đủ thông tin!",
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return;
            }
            ModelFile.Model = _modelSpecificationViewModel.Model;
            ModelFile.ModelId = _modelSpecificationViewModel.Model.Id;
            // Lưu ModelFiel vào Database
            fileServices.SaveModelFile(ModelFile);
            await TryCloseAsync();
        }


        public void ButtonCancel()
        {
            TryCloseAsync();
        }
    }
}
