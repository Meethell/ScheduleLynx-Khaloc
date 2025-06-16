using Caliburn.Micro;
using Client.Entities.TicketEntities;
using Client.Services;
using System.Threading.Tasks;
using Client.Entities.ModelEntities;

namespace Client.ViewModels
{
    public class NewIssueWindowViewModel : Screen
    {

        private bool _result;
        public bool Result
        {
            get { return _result; }
            set
            {
                _result = value;
                NotifyOfPropertyChange(() => Result);
            }
        }
        // Properties
        private Issue _issue = new Issue();
        public Issue Issue
        {
            get => _issue;
            set
            {
                _issue = value;
                NotifyOfPropertyChange(() => Issue);
            }
        }

        // Contructor
        public NewIssueWindowViewModel(Model model)
        {
            Issue.Model = model;
            Issue.ModelId = model.Id;
        }

        // Methods
        //Button actions
        public async Task bnSave()
        {
            // Kiểm tra Title
            if (string.IsNullOrWhiteSpace(Issue.Title))
            {
                // Hiển thị thông báo lỗi
               var customMessageBoxViewModel = new CustomMessageBoxViewModel
               {
                   Message = "Lỗi",
                   TxtMessage = "Chưa điền tiêu đề cho Issue",
                   IsInformation = true
               };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return;
            }
            else
            {
                // Lưu Issue vào database
                Issue.Model = null;
                // Lưu vào database
                await new SaveDataServices().SaveIssueAsync(Issue);
                // Cập nhật lại Cache
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                Result = true;
                await TryCloseAsync();
            }
        }
        public async Task bnCancel()
        {
            Result = false;
            await TryCloseAsync();
        }

    }
}
