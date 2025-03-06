using PEWeldingApp.Models;
using PEWeldingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PEWeldingApp.Views
{
    public partial class CalcWindow : Window
    {
        CalcVariant calcVariant;
        Organization organization;
        public CalcWindow(CalcVariant calcVariant, Window window, Organization organization)
        {
            InitializeComponent();
            this.organization = organization;
            this.calcVariant = calcVariant;
            Owner = window;
            DataContext = new CalcVM(calcVariant, organization);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) => EmissionSourceBlock.Text = calcVariant.Number.ToString() + $"({organization.Title})";
        private void CloseApp(object sender, RoutedEventArgs e) => this.Close();
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void TextBlock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textbox && !e.Text.All(ch => char.IsDigit(ch) || ch == '.' || ch == ','))
            {
                e.Handled = true;
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Handled && (e.Key == Key.OemComma))
            {
                e.Handled = true;
            }
        }
    }
}