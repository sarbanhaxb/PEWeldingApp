using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace PEWeldingApp.Models
{
    public class AppSetting : INotifyPropertyChanged
    {
        // Округление м/р выброса
        int _roundCoefmr;
        public int RoundCoefmr
        {
            get => _roundCoefmr;
            set
            {
                _roundCoefmr = value;
                OnPropertyChanged(nameof(RoundCoefmr));
            }
        }

        // Округление валового выброса
        int _roundCoefval;
        public int RoundCoefval
        {
            get => _roundCoefval;
            set
            {
                _roundCoefval = value;
                OnPropertyChanged(nameof(RoundCoefval));
            }
        }

        // Размер шрифта
        int _fontSize;
        public int FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        //Шрифт 
        string _fontFamily;
        public string FontFamily
        {
            get => _fontFamily;
            set
            {
                _fontFamily = value;
                OnPropertyChanged(nameof(FontFamily));
            }
        }

        //ссылка на рабочий каталог
        string _pathToDataBase;

        public string PathToDataBase
        {
            get => _pathToDataBase;
            set
            {
                _pathToDataBase = value;
                OnPropertyChanged(nameof(PathToDataBase));
            }
        }

        public void Save()
        {
            string fileName = Global.AppSettingsFile;

            if (File.Exists(fileName))
                File.Delete(fileName);

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                JsonSerializer.Serialize(fs, this);
                fs.Close();
            }
        }
        public static AppSetting GetSettings()
        {
            AppSetting? settings = null;
            string fileName = Global.AppSettingsFile;
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    settings = JsonSerializer.Deserialize<AppSetting>(fs);
                    fs.Close();
                }
            }
            else settings = new AppSetting();

            return settings;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}