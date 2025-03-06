using PEWeldingApp.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PEWeldingApp.Views
{
    public partial class CalcVariantWindow : Window
    {
        public CalcVariant calcVariant { get; private set; }
        Organization organization;
        public CalcVariantWindow(CalcVariant calc, MainWindow mw, Organization organization)
        {
            InitializeComponent();
            this.calcVariant = calc;
            DataContext = calcVariant;
            Owner = mw;
            this.organization = organization;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.calcVariant.OrganizationId = organization.Id;
            this.calcVariant.Type = ((ComboBoxItem)TypeCB.SelectedItem).Content.ToString();
            DialogResult = true;

        }
        private void CloseApp(object sender, RoutedEventArgs e) => this.Close();
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();
        private void CodeField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text.Length < 7)
            {
                e.Handled = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.calcVariant.Type == "Контактная сварка")
                TypeCB.SelectedIndex = 1;
            else
                TypeCB.SelectedIndex = 0;
        }
    }
}
