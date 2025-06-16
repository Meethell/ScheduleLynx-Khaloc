using Caliburn.Micro;
using Client.Entities.TicketEntities;
using Client.ViewModels;
using System.Diagnostics;
using Client.Entities.ModelEntities;
using System.Threading.Tasks;
using System.Linq;

namespace Client.Services
{
    public class FileServices
    {
        // TicketFile
        private TicketFile _ticketFile = new TicketFile();
        public TicketFile TicketFile
        {
            get { return _ticketFile; }
            set { _ticketFile = value; }
        }

        // ModelFile
        private ModelFile _modelFile = new ModelFile();
        public ModelFile ModelFile
        {
            get { return _modelFile; }
            set { _modelFile = value; }
        }
        public FileServices() { }

        // Open Ticket file dialog
        public TicketFile OpenTicketFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();

            TicketFile.FilePath = openFileDialog.FileName;
            // Bỏ đuôi file
            TicketFile.FileName = openFileDialog.SafeFileName.Substring(0, openFileDialog.SafeFileName.LastIndexOf('.'));
            TicketFile.SafeFileName = openFileDialog.SafeFileName;
            TicketFile.FileExtension = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf('.') + 1);
            // Xử lý trường hợp open file dialog bị cancel
            if (TicketFile.FilePath == "")
            {
                return null;
            }
            return TicketFile;
        }
        // Open Model file dialog
        public ModelFile OpenModelFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();

            ModelFile.FilePath = openFileDialog.FileName;
            ModelFile.FileName = openFileDialog.SafeFileName;
            ModelFile.SafeFileName = openFileDialog.SafeFileName;
            ModelFile.FileExtension = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf('.') + 1);
            // Xử lý trường hợp open file dialog bị cancel
            if (ModelFile.FilePath == "")
            {
                return null;
            }
            return ModelFile;
        }

        // Copy File to server
        public async Task<bool> SaveModelFile(ModelFile modelFile)
        {
            string sourcePath = modelFile.FilePath;
            string destinationPath = System.IO.Path.Combine(Properties.Settings.Default.SaveFileFolderPath, modelFile.Model.Manufacturer.Name, modelFile.Model.Name, ModelFile.SafeFileName);
            // Kiểm tra đường dẫn có tồn tại không nếu không thì tạo mới
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(destinationPath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destinationPath));
            }
            // Try catch to handle exception
            try
            {
                System.IO.File.Copy(sourcePath, destinationPath, true);

                // Save ModelFile to Database
                modelFile.FilePath = destinationPath;
                modelFile.Model = null;
                await new SaveDataServices().SaveModelFileAsync(modelFile);
                return true;
            }
            catch (System.IO.IOException ex) // Show io exception
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = ex.Message,
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return false;
            }
        }

        // Save Ticket File
        public async Task<bool> SaveTicketFile(TicketFile ticketFile)
        {
            string sourcePath = ticketFile.FilePath;
            // Tìm Ticket Number trong TicketId
            var ticketNumber = CachesServices.Instance.Tickets
                .FirstOrDefault(t => t.Id == ticketFile.TicketId)?.TicketNumber;

            string destinationPath = System.IO.Path.Combine(Properties.Settings.Default.SaveFileFolderPath, "Tiket File","Ticket " + ticketNumber.ToString(), ticketFile.SafeFileName);
            // Kiểm tra đường dẫn có tồn tại không nếu không thì tạo mới
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(destinationPath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destinationPath));
            }
            // Try catch to handle exception
            try
            {
                System.IO.File.Copy(sourcePath, destinationPath, true);

                // Save TicketFile to Database
                ticketFile.FilePath = destinationPath;
                ticketFile.Ticket = null;
                await new SaveDataServices().SaveTicketFileAsync(ticketFile);
                return true;
            }
            catch (System.IO.IOException ex) // Show io exception
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = ex.Message,
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return false;
            }
        }

        // Delete file
        public async void DeleteFile(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    var customMessageBoxViewModel = new CustomMessageBoxViewModel
                    {
                        Message = "Error",
                        TxtMessage = "File not found",
                        IsInformation = true
                    };
                    var windowManager = new WindowManager();
                    await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                    return;
                }
            }
            catch (System.IO.IOException ex) // Show io exception
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = ex.Message,
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
                return;
            }
                
        }
        // Open file
        public async void OpenFile(string filePath)
        {
            // Try catch to handle exception display message box with custom message box
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "Error",
                    TxtMessage = ex.Message,
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            }
        }
    }
}
