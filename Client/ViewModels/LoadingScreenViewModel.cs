using Caliburn.Micro;
using System.Threading;

namespace Client.ViewModels
{
    public class LoadingScreenViewModel : Screen
    {
        //Sinleton
        private static LoadingScreenViewModel _instance;
        public static LoadingScreenViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoadingScreenViewModel();
                }
                return _instance;
            }
        }

        //Constructor
        public LoadingScreenViewModel()
        {
            _instance = this;
        }
    }
}
