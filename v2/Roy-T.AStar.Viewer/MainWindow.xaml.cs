using System.Windows;

namespace Roy_T.AStar.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
