using PEWeldingApp.Models;
using PEWeldingApp.ViewModels;
using System.Windows;
using System.Windows.Input;


namespace PEWeldingApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AppSettingsWin.xaml
    /// </summary>
    public partial class AppSettingsWin : Window
    {
        public AppSettingsWin(Window win)
        {
            InitializeComponent();
            DataContext = new AppSettingVM(new DefaultDialogSerice());
            Owner = win;
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
