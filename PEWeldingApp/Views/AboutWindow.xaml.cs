using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Input;

namespace PEWeldingApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void MinimizeMaximizeApp(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.WindowState != WindowState.Maximized)
                {
                    MinMaxIcon.Kind = PackIconKind.WindowRestore;

                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    MinMaxIcon.Kind = PackIconKind.WindowMaximize;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                try
                {
                    if (this.WindowState != WindowState.Maximized)
                    {
                        MinMaxIcon.Kind = PackIconKind.WindowRestore;

                        this.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        this.WindowState = WindowState.Normal;
                        MinMaxIcon.Kind = PackIconKind.WindowMaximize;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void CloseApp(object sender, RoutedEventArgs e) => this.Close();
        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void AboutTV_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            switch (TextOutside.Text)
            {
                case "О программе":
                    TextInside.Text = AboutProg();
                    break;
                case "Системные требования":
                    TextInside.Text = SystemV();
                    break;
                case "Об организации":
                    TextInside.Text = AboutOrg();
                    break;
                case "Предприятия":
                    TextInside.Text = OrganizationInf();
                    break;
                case "Источники выбросов":
                    TextInside.Text = EmSourceInf();
                    break;
                case "Расчетная форма":
                    TextInside.Text = CalcFormInf();
                    break;
                case "Справочник веществ":
                    TextInside.Text = AboutEmissions();
                    break;
                case "Настройки расчета":
                    TextInside.Text = WeldSettingsInfo();
                    break;
                case "Настройки приложения":
                    TextInside.Text = AppSettingsInfo();
                    break;
                default:
                    TextInside.Text = "";
                    break;
            }
        }

        string AboutProg()
        {
            string txt = "Программа «РВСГ-Эколог» предназначена для расчета максимально-разовых и валовых (годовых) выбросов загрязняющих веществ при производстве сварки полиэтиленовой геомембраны в соответствии с " +
                "Расчетная инструкция (методика) «Удельные показатели образования вредных веществ, выделяющихся в атмосферу от основных видов технологического оборудования для предприятий радиоэлектронного комплекса»" +
                " (утверждена Федеральным агентством по промышленности Российской Федерации, 2006 год)" +
                " \nПрм первом запуске программы необходимо произвести настройки приложения и расчета, после чего сохранить настройки.";
            return txt;
        }
        string SystemV()
        {
            string txt = "Программа предназначена для работы в операционной системе MS WINDOWS 7/8/10/11";
            return txt;
        }
        string AboutOrg()
        {
            string txt = "ООО «НК «Роснефть»- НТЦ» \n" + "350000, Краснодарский край, город Краснодар, Красная ул., д. 54";
            return txt;
        }
        string OrganizationInf()
        {
            string txt = "Окно Предприятия предназначено для работы с данными предприятий. С помощью соответствующих пунктов меню, кнопок на панели управления. Добавление и изменение данных предприятия" +
                "осуществляется через промежуточные формы, в которых проверяется корректность введенной информации. \nУдаление осуществляется после предварительного диалога, при этом удаляются все иерархически связанные источники" +
                "выбросов, а также весь каталог с файлами данного предприятия.\n\nС помощью аналогичного инструментария предоставляется возможность использования различных функций: \n" +
                "Двойное нажатие левой кнопкой мыши - переход к списку источников выбросов.\n" +
                "Для настроки расчетов и приложения в верхней панели присутствует меню.\n" +
                "При добавлении нового предприятия обязательными полями ввода является код и наименование.";
            return txt;
        }
        string EmSourceInf()
        {
            string txt = "Окно Источники выбросов предназначено для работы с данными источников выброса. \n" +
                "С помощью соответствующих пунктов меню, кнопок на панели управления Вы можете проводить операции добавления, редактирования или изменения информации. Добавление или изменение источника осуществляется через промежуточные формы, в которых проверятся корректность введенной информации." +
                " Удаление осуществляется после предварительного диалога, при этом удаляются все иерархически связанные источники выделений.\n\n" +
                "Типы источников выбросов:\n - Экструзионная сварка\n - Контактная сварка";
            return txt;
        }
        string CalcFormInf()
        {
            string txt = "Окно расчетной формы предназначено для проведения расчета по источнику выбросов. \r\n\r\n" +
                "При некорректном вводе значений исходных данных расчет произведен не будет или будет выдано сообщение об ошибке. " +
                "Исходные данные для расчета по каждому источнику выбросов сохраняются в базе данных, расчет же производится непосредственно из расчетной формы. \n" +
                "Печать отчета возможна как из вкладки Источники выбросов (выгрузка по предприятию), так и из расчетной формы (выгрузка по источнику)";
            return txt;
        }
        string AboutEmissions()
        {
            string txt = "Справочник веществ содержит информацию о веществах, выделяющихся при производстве работ по сварке полиэтиленовой пленки. Редактирование справочника не предусмотрено.";
            return txt;
        }
        string WeldSettingsInfo()
        {
            string txt = "Окно Настройки расчета предназначено для редактирования общих настроек расчета по пользовательским данным. \nПри первом запуске программы необходимо заполнить все поля и выполнить сохранение.\nПри последующих запусках будут использоваться сохраненные данные.";
            return txt;
        }
        string AppSettingsInfo()
        {
            string txt = "Окно Настройки программы предназначено для общих настроек отчетов и печати.\nПри первом запуске программы необходимо заполнить все поля и выполнить сохранение.";
            return txt;
        }
    }
}
