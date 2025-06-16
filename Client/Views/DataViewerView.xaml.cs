using Client.ViewModels;
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
    /// <summary>
    /// Interaction logic for DataViewerView.xaml
    /// </summary>
    public partial class DataViewerView : UserControl
    {
        public DataViewerView()
        {
            InitializeComponent();
        }
        private void DgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as DataViewerViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedItems = dgData.SelectedItems.Cast<dynamic>().ToList();
            }
        }
    }
}
