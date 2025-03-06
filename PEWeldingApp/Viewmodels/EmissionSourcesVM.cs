using Microsoft.EntityFrameworkCore;
using PEWeldingApp.Models;
using PEWeldingApp.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PEWeldingApp.ViewModels
{
    public class EmissionSourcesVM : INotifyPropertyChanged
    {
        readonly ApplicationContext db = new ApplicationContext();
        ObservableCollection<CalcVariant> CalcVariants;
        public ICollectionView CalcVariantsCSV { get; private set; }
        MainWindow mainWindow;
        Organization organization;

        public EmissionSourcesVM(Organization organization, MainWindow mw)
        {
            this.organization = organization;
            this.mainWindow = mw;
            db.Database.EnsureCreated();
            db.CalcVariants.Where(obj => obj.OrganizationId == this.organization.Id).Load();
            CalcVariants = db.CalcVariants.Local.ToObservableCollection();
            CalcVariantsCSV = CollectionViewSource.GetDefaultView(CalcVariants);
        }

        RelayCommand? addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand((o) =>
                    {
                        CalcVariantWindow calcVariantWindow = new CalcVariantWindow(new CalcVariant(), mainWindow, organization);

                        if (calcVariantWindow.ShowDialog() == true)
                        {
                            CalcVariant calcVariant = calcVariantWindow.calcVariant;
                            calcVariant.SetDefaultValue();
                            db.CalcVariants.Add(calcVariant);
                            db.SaveChanges();
                        }
                    }));
            }
        }

        RelayCommand? deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                    (deleteCommand = new RelayCommand((selectedItem) =>
                    {
                        CalcVariant? calcVariant = selectedItem as CalcVariant;
                        if (calcVariant == null) return;
                        MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            db.CalcVariants.Remove(calcVariant);
                            db.SaveChanges();
                        }
                    }));
            }
        }

        RelayCommand? sendMessage;

        public RelayCommand SendMessage
        {
            get
            {
                return sendMessage ??
                    (sendMessage = new RelayCommand(o =>
                    {
                        string recipient = "OE_Shustov@ntc.rosneft.ru";
                        string subject = "Программа: расчет выбросов от сварки геомембраны";
                        string body = "Добрый день!\n";
                        string mailtoUri = $"mailto:{recipient}?subject={subject}&body={body}";
                        Process.Start(new ProcessStartInfo(mailtoUri) { UseShellExecute = true });
                    }));
            }
        }

        RelayCommand? editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand((selectedItem) =>
                    {
                        CalcVariant calcVariant = selectedItem as CalcVariant;
                        if (calcVariant == null) return;

                        CalcVariant vm = new CalcVariant
                        {
                            Id = calcVariant.Id,
                            OrganizationId = calcVariant.OrganizationId,
                            Place = calcVariant.Place,
                            Number = calcVariant.Number,
                            Type = calcVariant.Type,
                            Title = calcVariant.Title
                        };
                        CalcVariantWindow calcVariantWindow = new CalcVariantWindow(vm, mainWindow, organization);

                        if (calcVariantWindow.ShowDialog() == true)
                        {
                            calcVariant.Number = calcVariantWindow.calcVariant.Number;
                            calcVariant.Type = calcVariantWindow.calcVariant.Type;
                            calcVariant.Title = calcVariantWindow.calcVariant.Title;
                            calcVariant.Place = calcVariantWindow.calcVariant.Place;
                            calcVariant.SeamLength = calcVariant.Type == "Экструзионная сварка" ? WeldingSetting.GetSettings().WeldingSpeedE : WeldingSetting.GetSettings().WeldingSpeedC;
                            calcVariant.SeamThick = calcVariant.Type == "Экструзионная сварка" ? WeldingSetting.GetSettings().SeamThickE : WeldingSetting.GetSettings().SeamThickC;
                            calcVariant.SeamWidth = calcVariant.Type == "Экструзионная сварка" ? WeldingSetting.GetSettings().SeamWidthE : WeldingSetting.GetSettings().SeamWidthC;
                            db.Entry(calcVariant).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }));
            }
        }

        RelayCommand? calcCommand;

        public RelayCommand CalcCommand
        {
            get
            {
                return calcCommand ??
                    (calcCommand = new RelayCommand((selectedItem) =>
                    {
                        if (selectedItem is CalcVariant calcVariant)
                        {
                            CalcWindow calcVariantWindow = new CalcWindow(calcVariant, mainWindow, organization);

                            if (calcVariantWindow.ShowDialog() == false)
                            {
                                db.Entry(calcVariant).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }));
            }
        }

        RelayCommand printResult;

        public RelayCommand PrintResult
        {
            get
            {
                return printResult ??
                    (printResult = new RelayCommand((dg) =>
                    {
                        List<CalcVariant> calcVariants = new List<CalcVariant>();
                        if (dg is DataGrid dataGrid)
                        {
                            foreach (CalcVariant calcVariant in dataGrid.Items)
                            {
                                calcVariants.Add(calcVariant);
                            }
                        }

                        ExportToDocx.PrintResult(calcVariants, organization);
                    }));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
