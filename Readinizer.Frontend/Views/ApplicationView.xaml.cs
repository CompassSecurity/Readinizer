using System.Windows;
using Readinizer.Frontend.ViewModels;

namespace Readinizer.Frontend.Views
{
    /// <summary>
    /// Interaction logic for ApplicationView.xaml
    /// </summary>
    public partial class ApplicationView : Window
    {
        public ApplicationView(ApplicationViewModel applicationViewModel)
        {
            InitializeComponent();
            DataContext = applicationViewModel;
        }
    }
}
