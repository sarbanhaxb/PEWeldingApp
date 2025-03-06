using MaterialDesignThemes.Wpf;
using PEWeldingApp.Models;
using PEWeldingApp.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PEWeldingApp
{
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        public MainWindow()
        {
            InitializeComponent();
            Trace.WriteLine($"asdsad: {AppSetting.GetSettings().PathToDataBase}");

            DataContext = new MainViewModel(this);
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
        private void organizationData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
                new EmissionSourcesWindow(((Organization)((DataGrid)sender).SelectedItem), this).ShowDialog();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e) => new EmissionWindow(this).Show();
        private void Button_Click(object sender, RoutedEventArgs e) => new WeldingSettingsWindow(this).ShowDialog();
        private void Button_Click1(object sender, RoutedEventArgs e) => new AppSettingsWin(this).ShowDialog();
        private void MenuItem_Click_1(object sender, RoutedEventArgs e) => new WeldingSettingsWindow(this).ShowDialog();

        private void OnNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }
    }
}