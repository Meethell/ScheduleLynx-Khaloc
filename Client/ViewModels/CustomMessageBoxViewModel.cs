using Caliburn.Micro;
using System.Windows;

namespace Client.ViewModels
{
    public class CustomMessageBoxViewModel : Screen
    {
        
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        private string _txtMessage;
        public string TxtMessage
        {
            get { return _txtMessage; }
            set
            {
                _txtMessage = value;
                NotifyOfPropertyChange(() => _txtMessage);
            }
        }

        public bool IsConfirmation { get; set; } = false;
        public bool IsInformation { get; set; } = false;

        public MessageBoxResult DialogResult { get; private set; }

        public void btnYes()
        {
            DialogResult = MessageBoxResult.Yes;
            TryCloseAsync();
        }

        public void btnNo()
        {
            DialogResult = MessageBoxResult.No;
            TryCloseAsync();
        }

        public void btnOk()
        {
            DialogResult = MessageBoxResult.OK;
            TryCloseAsync();
        }        
    }
}
