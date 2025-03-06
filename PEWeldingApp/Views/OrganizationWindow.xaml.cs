using PEWeldingApp.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PEWeldingApp.Views
{
    public partial class OrganizationWindow : Window
    {
        public Organization Organization { get; private set; }
        public OrganizationWindow(Organization organization, MainWindow mw)
        {
            InitializeComponent();
            this.Organization = organization;
            this.Organization.CalcVariants = new List<CalcVariant>();
            DataContext = Organization;
            Owner = mw;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (Organization.Code != "Код организации" && Organization.Title != "Наименование организации")
                DialogResult = true;
        }
        private void CloseApp(object sender, RoutedEventArgs e) => this.Close();
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void CodeField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CodeField.Text == "Код организации")
            {
                CodeField.Text = "";
                CodeField.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void CodeField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CodeField.Text == "")
            {
                CodeField.Text = "Код организации";
                CodeField.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#979392"));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();
        private void TitleField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TitleField.Text == "Наименование организации")
            {
                TitleField.Text = "";
                TitleField.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        private void TitleField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TitleField.Text == "")
            {
                TitleField.Text = "Наименование организации";
                TitleField.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#979392"));
            }
        }
        private void CodeField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text.Length < 7)
            {
                e.Handled = true;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (TitleField.Text == "Наименование организации")
                TitleField.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#979392"));
            if (CodeField.Text == "Код организации")
                CodeField.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#979392"));
        }
    }
}
