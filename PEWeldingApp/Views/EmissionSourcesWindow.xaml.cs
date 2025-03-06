using MaterialDesignThemes.Wpf;
using PEWeldingApp.Models;
using PEWeldingApp.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace PEWeldingApp.Views
{
    /// <summary>
    /// Логика взаимодействия для EmissionSourcesWindow.xaml
    /// </summary>
    public partial class EmissionSourcesWindow : Window
    {
        MainWindow thisMainWindow;
        Organization organization;
        public EmissionSourcesWindow(Organization organization, MainWindow mainWindow)
        {
            InitializeComponent();
            Owner = mainWindow;
            OrgText.Text = $"({organization.Title})";
            DataContext = new EmissionSourcesVM(organization, mainWindow);
            this.thisMainWindow = mainWindow;
            this.organization = organization;
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void MinimizeMaximizeApp(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.WindowState != WindowState.Maximized)
                {
                    MinMaxIcon.Kind = PackIconKind.WindowRestore;

                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    MinMaxIcon.Kind = PackIconKind.WindowMaximize;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                try
                {
                    if (this.WindowState != WindowState.Maximized)
                    {
                        MinMaxIcon.Kind = PackIconKind.WindowRestore;

                        this.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        this.WindowState = WindowState.Normal;
                        MinMaxIcon.Kind = PackIconKind.WindowMaximize;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void CloseApp(object sender, RoutedEventArgs e) => this.Close();
        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void CalcSettingsOpen(object sender, RoutedEventArgs e) => new WeldingSettingsWindow(thisMainWindow).ShowDialog();
    }
}