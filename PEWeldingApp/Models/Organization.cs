using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PEWeldingApp.Models
{
    public class Organization : INotifyPropertyChanged
    {
        public int Id { get; set; }
        string? title;
        string? code;
        public List<CalcVariant> CalcVariants { get; set; }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Code
        {
            get => code;
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        public Organization()
        {
            this.code = "Код организации";
            this.title = "Наименование организации";
            CalcVariants = new List<CalcVariant>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}