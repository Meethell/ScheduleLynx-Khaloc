using Client.ViewModels;
using Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Views
{
    // MainView.xaml.cs
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            // Lấy ViewModel từ DataContext và gọi OnStartup
            if (this.DataContext is MainViewModel vm)
            {
                vm.OnStartup();
            }
            // Chỉ nên gọi 1 lần
            this.Loaded -= MainView_Loaded;
        }
    }
}
