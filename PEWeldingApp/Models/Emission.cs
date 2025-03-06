using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PEWeldingApp.Models
{
    public class Emission : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? CAS { get; set; }
        public string? PDKmr { get; set; }
        public string? PDKss { get; set; }
        public string? PDKsg { get; set; }
        public int ClassOfDanger { get; set; }
        public float Coeff { get; set; }
        public float Tyear { get; set; }
        public float Gsec { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}