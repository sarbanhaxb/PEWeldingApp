using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PEWeldingApp.Models
{
    public class CalcVariant : INotifyPropertyChanged
    {

        public int Id { get; set; }
        public int OrganizationId { get; set; } //внешний ключ

        public string number = "0";
        public string? Number
        {
            get => number; set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        } //номер источника
        public Organization Organization { get; set; } //навигационное поле

        public float Gsec { get; set; } //грамм в секунду
        public float Tyear { get; set; } //тонны в год

        //тип сварки (экструзионная/контактная)
        string? type;
        public string? Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(SeamLength));
                OnPropertyChanged(nameof(SeamWidth));
                OnPropertyChanged(nameof(Density));
                OnPropertyChanged(nameof(SeamThick));
            }
        }

        //длина свариваемого шва, м (равняется производительности сварочного аппарата в м/час)
        float seamLength;
        public float SeamLength
        {
            get => seamLength;
            set
            {
                seamLength = value;
                OnPropertyChanged(nameof(SeamLength));
            }
        }

        //ширина свариваемого шва, м
        float seamWidth;
        public float SeamWidth
        {
            get => seamWidth;
            set
            {
                seamWidth = value;
                OnPropertyChanged(nameof(SeamWidth));
            }
        }

        //толщина свариваемого шва, м
        float seamThick;
        public float SeamThick
        {
            get => seamThick;
            set
            {
                seamThick = value;
                OnPropertyChanged(nameof(SeamThick));
            }
        }

        //плотность свариваемого материала кг/м3 (плотность геомембраны)
        float density;
        public float Density
        {
            get => density;
            set
            {
                density = value;
                OnPropertyChanged(nameof(Density));
            }
        }

        //номер площадки
        int place;
        public int Place
        {
            get => place;
            set
            {
                place = value;
                OnPropertyChanged(nameof(Place));
            }
        }

        //количество швов за один проход, шт (иногда может быть больше 1)
        int seamNum;
        public int SeamNum
        {
            get => seamNum;
            set
            {
                seamNum = value;
                OnPropertyChanged(nameof(SeamNum));
            }
        }

        //Объем работ в п.м.
        float workVol;
        public float WorkVol
        {
            get => workVol;
            set
            {
                workVol = value;
                OnPropertyChanged(nameof(WorkVol));
            }
        }

        //Название источника
        string? title;
        public string? Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        //Устанавливает значения объекту по умолчанию
        public void SetDefaultValue()
        {
            WeldingSetting ws = WeldingSetting.GetSettings();
            this.seamLength = this.type == "Экструзионная сварка" ? ws.WeldingSpeedE : ws.WeldingSpeedC;
            this.seamThick = this.Type == "Экструзионная сварка" ? ws.SeamThickE : ws.SeamThickC;
            this.seamWidth = this.Type == "Экструзионная сварка" ? ws.SeamWidthE : ws.SeamWidthC;
            this.Density = ws.Denisty;
            this.Gsec = 0;
            this.Tyear = 0;
            this.workVol = 0;
            this.seamNum = 0;
        }
    }
}