using System.ComponentModel;
using System.IO;
using System.Text.Json;


namespace PEWeldingApp.Models
{
    public class WeldingSetting : INotifyPropertyChanged
    {
        // Производительность сварочного аппарата (пачек в час) при экструзионной сварке. Принимается за единицу по умолчанию, т.к. производительность корректируется длиной шва
        float efficienceE;
        public float EfficienceE
        {
            get => efficienceE;
            set
            {
                efficienceE = value;
                OnPropertyChanged(nameof(efficienceE));
            }
        }

        // Скорость экструзионной сварки, она же принимается в качестве длины шва.
        float weldingSpeedE;
        public float WeldingSpeedE
        {
            get => weldingSpeedE;
            set
            {
                weldingSpeedE = value;
                OnPropertyChanged(nameof(WeldingSpeedE));
            }
        }

        // Коэффициент, учитывающий временной фактор выделения вредностей (Kt). Из методики. Для экструзионной сварки.
        float timeCoeffE;
        public float TimeCoeffE
        {
            get => timeCoeffE;
            set
            {
                timeCoeffE = value;
                OnPropertyChanged(nameof(TimeCoeffE));
            }
        }

        // Производительность сварочного аппарата (пачек в час) при контактной сварке. Принимается за единицу по умолчанию, т.к. производительность корректируется длиной шва
        float efficienceC;
        public float EfficienceC
        {
            get => efficienceC;
            set
            {
                efficienceC = value;
                OnPropertyChanged(nameof(EfficienceC));
            }
        }

        // Скорость контактной сварки, она же принимается в качестве длины шва.
        float weldingSpeedC;
        public float WeldingSpeedC
        {
            get => weldingSpeedC;
            set
            {
                weldingSpeedC = value;
                OnPropertyChanged(nameof(WeldingSpeedC));
            }
        }

        // Коэффициент, учитывающий временной фактор выделения вредностей (Kt). Из методики. Для контатной сварки.
        float timeCoeffC;
        public float TimeCoeffC
        {
            get => timeCoeffC;
            set
            {
                timeCoeffC = value;
                OnPropertyChanged(nameof(TimeCoeffC));
            }
        }

        // Плотность пленки, кг/м3
        float density;
        public float Denisty
        {
            get => density;
            set
            {
                density = value;
                OnPropertyChanged(nameof(Denisty));
            }
        }

        //Толщина свариваемого шва экструзионной сварки, м
        float seamThickE;
        public float SeamThickE
        {
            get => seamThickE;
            set
            {
                seamThickE = value;
                OnPropertyChanged(nameof(SeamThickE));
            }
        }

        //Толщина свариваемого шва контактной сварки, м
        float seamThickC;
        public float SeamThickC
        {
            get => seamThickC;
            set
            {
                seamThickC = value;
                OnPropertyChanged(nameof(SeamThickC));
            }
        }

        //Ширина свариваемого шва экструзионной сварки, м
        float seamWidthE;
        public float SeamWidthE
        {
            get => seamWidthE;
            set
            {
                seamWidthE = value;
                OnPropertyChanged(nameof(SeamWidthE));
            }
        }

        //Ширина свариваемого шва контактной сварки, м
        float seamWidthC;
        public float SeamWidthC
        {
            get => seamWidthC;
            set
            {
                seamWidthC = value;
                OnPropertyChanged(nameof(SeamWidthC));
            }
        }

        //сварка идет параллельно
        bool parWeld;
        public bool ParWeld
        {
            get => parWeld;
            set
            {
                parWeld = value;
                OnPropertyChanged(nameof(ParWeld));
            }
        }


        public void Save()
        {
            string fileName = Global.WeldingSettingsFile;

            if (File.Exists(fileName))
                File.Delete(fileName);

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                JsonSerializer.Serialize(fs, this);
                fs.Close();
            }
        }
        public static WeldingSetting GetSettings()
        {
            WeldingSetting? settings = null;
            string fileName = Global.WeldingSettingsFile;
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    settings = JsonSerializer.Deserialize<WeldingSetting>(fs);
                    fs.Close();
                }
            }
            else settings = new WeldingSetting();

            return settings;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
