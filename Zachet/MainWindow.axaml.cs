using Avalonia.Controls;
using Zachet.ViewModels;

namespace Zachet
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
