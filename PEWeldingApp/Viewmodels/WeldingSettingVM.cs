using PEWeldingApp.Models;
using System.ComponentModel;

namespace PEWeldingApp.ViewModels
{
    public class WeldingSettingVM : INotifyPropertyChanged
    {
        public WeldingSetting WeldingSetting { get; set; }
        public WeldingSettingVM()
        {
            WeldingSetting = WeldingSetting.GetSettings();
        }

        RelayCommand? saveChanges;
        public RelayCommand SaveChanges
        {
            get
            {
                return saveChanges ??
                    (saveChanges = new RelayCommand(obj => WeldingSetting.Save()));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}