using System.Windows;

namespace Viewer
{    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            InitializeComponent();

            // Poor man's data binding
            this.DataContext = new MainWindowViewModel();
        }
    }
}
