using PEWeldingApp.Models;
using System.ComponentModel;
using System.Windows.Controls;

namespace PEWeldingApp.ViewModels
{
    public class AppSettingVM : INotifyPropertyChanged
    {
        public AppSetting AppSetting { get; set; }
        IDialogService dialogService;

        public AppSettingVM(IDialogService dialogService)
        {
            AppSetting = AppSetting.GetSettings();
            this.dialogService = dialogService;
        }

        RelayCommand? saveChanges;
        public RelayCommand SaveChanges
        {
            get
            {
                return saveChanges ??
                    (saveChanges = new RelayCommand(obj => AppSetting.Save()));
            }
        }

        RelayCommand openDB;

        public RelayCommand OpenDB
        {
            get
            {
                return openDB ??
                    (openDB = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (dialogService.OpenFileDialog() == true)
                            {
                                string filePath = dialogService.FilePath;
                                if (obj is TextBox tb)
                                {
                                    tb.Text = filePath;
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            dialogService.ShowMessage(ex.Message);
                        }
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
