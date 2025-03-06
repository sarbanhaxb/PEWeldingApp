using PEWeldingApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;


namespace PEWeldingApp.ViewModels
{
    public class CalcVM : INotifyPropertyChanged
    {
        public CalcVariant calcVariant;
        ApplicationContext db = new ApplicationContext();
        WeldingSetting WeldingSetting { get; set; }

        Organization organization;
        public ObservableCollection<Emission> Emissions { get; set; } = new ObservableCollection<Emission>();
        public ICollectionView EmissionsCSV { get; set; }

        public CalcVM(CalcVariant calcVariant, Organization organization)
        {
            this.calcVariant = calcVariant;
            this.organization = organization;
            this.WeldingSetting = WeldingSetting.GetSettings();

            UpdateResult();

            EmissionsCSV = CollectionViewSource.GetDefaultView(Emissions);
        }

        private void UpdateResult()
        {
            Emissions.Clear();

            Emissions.Add(new Emission()
            {
                Code = "0337",
                Title = "Углерода оксид (Углерод окись; углерод моноокись; угарный газ)",
                Gsec = (float)Math.Round((calcVariant.Gsec * 0.3), AppSetting.GetSettings().RoundCoefmr),
                Tyear = (float)Math.Round((calcVariant.Tyear * 0.3), AppSetting.GetSettings().RoundCoefval)
            });
            Emissions.Add(new Emission()
            {
                Code = "1317",
                Title = "Ацетальдегид (Уксусный альдегид)",
                Gsec = (float)Math.Round((calcVariant.Gsec * 0.202), AppSetting.GetSettings().RoundCoefmr),
                Tyear = (float)Math.Round((calcVariant.Tyear * 0.202), AppSetting.GetSettings().RoundCoefval)
            });
            Emissions.Add(new Emission()
            {
                Code = "1325",
                Title = "Формальдегид (Муравьиный альдегид, оксометан, метиленоксид)",
                Gsec = (float)Math.Round((calcVariant.Gsec * 0.282), AppSetting.GetSettings().RoundCoefmr),
                Tyear = (float)Math.Round((calcVariant.Tyear * 0.282), AppSetting.GetSettings().RoundCoefval)
            });
            Emissions.Add(new Emission()
            {
                Code = "1555",
                Title = "Этановая кислота (Метанкарбоновая кислота)",
                Gsec = (float)Math.Round((calcVariant.Gsec * 0.216), AppSetting.GetSettings().RoundCoefmr),
                Tyear = (float)Math.Round((calcVariant.Tyear * 0.216), AppSetting.GetSettings().RoundCoefval)
            });
        }

        public CalcVariant CalcVariant
        {
            get => calcVariant;
            set
            {
                calcVariant = value;
                OnPropertyChanged(nameof(CalcVariant));
            }
        }

        RelayCommand? doCalcResult;

        public RelayCommand DoCalcResult
        {
            get
            {
                return doCalcResult ??
                    (doCalcResult = new RelayCommand((o) =>
                    {
                        calcVariant.Gsec = CountGsec(calcVariant);
                        calcVariant.Tyear = CountTyear(calcVariant);

                        UpdateResult();
                    }));
            }
        }

        RelayCommand? printResult;

        public RelayCommand PrintResult
        {
            get
            {
                return printResult ??
                    (printResult = new RelayCommand((o) =>
                    {
                        calcVariant.Gsec = CountGsec(calcVariant);
                        calcVariant.Tyear = CountTyear(calcVariant);
                        ExportToDocx.PrintResult(new List<CalcVariant>() { calcVariant }, organization);
                    }));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private float CountGsec(CalcVariant calcVariant)
        {
            float Gsec = 0;
            float Gsv = calcVariant.Type == "Экструзионная сварка" ? WeldingSetting.EfficienceE : WeldingSetting.EfficienceC; // Gсв
            float g = calcVariant.Density; // плотность пленки
            float a = calcVariant.SeamWidth; // ширина шва
            float b = calcVariant.SeamLength; // длина шва
            float S = (float)Math.Round(a * b, 3); // площадь свариваемого шва
            float h = calcVariant.SeamThick; // толщина шва
            float n = calcVariant.SeamNum; // количество швов

            float m1 = (float)Math.Round(Gsv * g * S * h * n, 3);

            float S1 = (float)Math.Round((a + 0.25 * b) * h, 3);
            float S2 = (float)Math.Round(a * b, 3);
            float Km = (float)Math.Round(S1 / S2, 3);
            float Kt = calcVariant.Type == "Экструзионная сварка" ? WeldingSetting.TimeCoeffE : WeldingSetting.TimeCoeffC;

            float m3 = (float)Math.Round(Km * Kt * m1, 3);
            Gsec = (float)Math.Round(m3 * 1000 / 3600, AppSetting.GetSettings().RoundCoefmr);

            return Gsec;
        }

        private float CountTyear(CalcVariant calcVariant)
        {
            float Gsv = calcVariant.Type == "Экструзионная сварка" ? WeldingSetting.EfficienceE : WeldingSetting.EfficienceC; // Gсв
            float g = calcVariant.Density; // плотность пленки
            float a = calcVariant.SeamWidth; // ширина шва
            float b = calcVariant.SeamLength; // длина шва
            float S = (float)Math.Round(a * b, 3); // площадь свариваемого шва
            float h = calcVariant.SeamThick; // толщина шва
            float n = calcVariant.SeamNum; // количество швов

            float m1 = (float)Math.Round(Gsv * g * S * h * n, 3);

            float S1 = (float)Math.Round((a + 0.25 * b) * h, 3);
            float S2 = (float)Math.Round(a * b, 3);
            float Km = (float)Math.Round(S1 / S2, 3);
            float Kt = calcVariant.Type == "Экструзионная сварка" ? WeldingSetting.TimeCoeffE : WeldingSetting.TimeCoeffC;

            float m3 = (float)Math.Round(Km * Kt * m1, 3);
            float time = (float)Math.Round(calcVariant.WorkVol / calcVariant.SeamLength, 3);
            float t_god = (float)Math.Round(m3 * time / 1000, AppSetting.GetSettings().RoundCoefval);

            return t_god;

        }
    }
}