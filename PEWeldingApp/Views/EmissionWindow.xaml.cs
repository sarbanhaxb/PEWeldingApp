using PEWeldingApp.ViewModels;
using System.Windows;
using System.Windows.Input;


namespace PEWeldingApp.Views
{
    public partial class EmissionWindow : Window
    {
        public EmissionWindow(MainWindow mw)
        {
            InitializeComponent();
            DataContext = new EmissionViewModel();
            Owner = mw;
        }

        private void CloseApp(object sender, RoutedEventArgs e) => this.Close();
        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
