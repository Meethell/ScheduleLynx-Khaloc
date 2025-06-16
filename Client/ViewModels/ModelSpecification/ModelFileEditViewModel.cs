using Caliburn.Micro;
using Client.Services;
using Client.Entities.ModelEntities;

namespace Client.ViewModels
{
    public class ModelFileEditViewModel : Screen
    {
        private ModelFile _modelFile;
        public ModelFile ModelFile
        {
            get { return _modelFile; }
            set { _modelFile = value; }
        }
        // Constructor
        public ModelFileEditViewModel(ModelFile modelFile)
        {
            ModelFile = modelFile;
        }
        public async void ButtonOk()
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(ModelFile.FileName)|| string.IsNullOrEmpty(ModelFile.FileCode)|| string.IsNullOrEmpty(ModelFile.FileType))
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = "Chưa điền đầy đủ thông tin!",
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return;
            }
            ModelFile.Model = null;
            await new SaveDataServices().SaveModelFileAsync(ModelFile);
            await TryCloseAsync();
        }


        public void ButtonCancel()
        {
            TryCloseAsync();
        }
    }
}
