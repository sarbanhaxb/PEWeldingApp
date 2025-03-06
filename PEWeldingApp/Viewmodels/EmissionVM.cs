using Microsoft.EntityFrameworkCore;
using PEWeldingApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace PEWeldingApp.ViewModels
{
    public class EmissionViewModel
    {
        readonly ApplicationContext db = new ApplicationContext();
        ObservableCollection<Emission> Emissions;
        public ICollectionView EmissionsCSV { get; private set; }


        public EmissionViewModel()
        {
            db.Database.EnsureCreated();
            db.Emissions.Load();
            Emissions = db.Emissions.Local.ToObservableCollection();
            EmissionsCSV = CollectionViewSource.GetDefaultView(Emissions);
        }

    }
}
