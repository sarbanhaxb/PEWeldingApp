using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PEWeldingApp.Models;
using PEWeldingApp.Views;
using System.Windows.Data;
using System.Diagnostics;
using System.Windows;


namespace PEWeldingApp
{
    public class MainViewModel
    {
        readonly ApplicationContext db = new ApplicationContext();
        public ObservableCollection<Organization> Organizations;
        public MainWindow window;
        public ICollectionView OrganizationsCSV { get; private set; }

        public MainViewModel(MainWindow mainWindow)
        {
            if (db.Database.EnsureCreated())
            {
                Emission CorbonOxide = new Emission
                {
                    Code = "0337",
                    Title = "Углерода оксид (Углерод окись; углерод моноокись; угарный газ)",
                    CAS = "630-08-0",
                    PDKmr = "5",
                    PDKss = "3",
                    PDKsg = "3",
                    ClassOfDanger = 4
                };
                Emission Acwtal = new Emission
                {
                    Code = "1317",
                    Title = "Ацетальдегид (Уксусный альдегид)",
                    CAS = "75-07-0",
                    PDKmr = "0,01",
                    PDKss = "-",
                    PDKsg = "0,005",
                    ClassOfDanger = 3
                };
                Emission Acwtal1 = new Emission
                {
                    Code = "1325",
                    Title = "Формальдегид (Муравьиный альдегид, оксометан, метиленоксид)",
                    CAS = "50-00-0",
                    PDKmr = "0,05",
                    PDKss = "0,01",
                    PDKsg = "0,003",
                    ClassOfDanger = 2
                };
                Emission Acwtal2 = new Emission
                {
                    Code = "1555",
                    Title = "Этановая кислота (Метанкарбоновая кислота)",
                    CAS = "64-19-7",
                    PDKmr = "0,2",
                    PDKss = "0,06",
                    PDKsg = "-",
                    ClassOfDanger = 3
                };

                db.Emissions.Add(CorbonOxide);
                db.Emissions.Add(Acwtal);
                db.Emissions.Add(Acwtal1);
                db.Emissions.Add(Acwtal2);
                db.SaveChanges();
            }

            db.Organizations.Load();
            Organizations = db.Organizations.Local.ToObservableCollection();
            OrganizationsCSV = CollectionViewSource.GetDefaultView(Organizations);
            window = mainWindow;
        }

        RelayCommand? addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand((o) =>
                    {
                        OrganizationWindow addOrganizationWindow = new OrganizationWindow(new Organization(), window);

                        if (addOrganizationWindow.ShowDialog() == true)
                        {
                            Organization organization = addOrganizationWindow.Organization;
                            db.Organizations.Add(organization);
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
                        Organization? organization = selectedItem as Organization;
                        if (organization == null) return;
                        MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            db.Organizations.Remove(organization);
                            db.SaveChanges();
                        }
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
                        Organization organizations = selectedItem as Organization;
                        if (organizations == null) return;

                        Organization vm = new Organization
                        {
                            Id = organizations.Id,
                            Code = organizations.Code,
                            Title = organizations.Title
                        };
                        OrganizationWindow addOrganizationWindow = new OrganizationWindow(vm, window);

                        if (addOrganizationWindow.ShowDialog() == true)
                        {
                            organizations.Code = addOrganizationWindow.Organization.Code;
                            organizations.Title = addOrganizationWindow.Organization.Title;
                            db.Entry(organizations).State = EntityState.Modified;
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
    }
}