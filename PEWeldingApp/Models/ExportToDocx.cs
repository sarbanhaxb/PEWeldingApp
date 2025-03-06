using System.Diagnostics;
using System.Windows;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace PEWeldingApp.Models
{
    public class ExportToDocx
    {
        public static void PrintResult(List<CalcVariant> calcVariants, Organization organization)
        {
            string FontFamily = AppSetting.GetSettings().FontFamily;

            Xceed.Document.NET.Licenser.LicenseKey = "WDN40-NHN7A-4T74D-P4FA";
            Xceed.Words.NET.Licenser.LicenseKey = "WDN40-NHN7A-4T74D-P4FA";

            int FontSize = AppSetting.GetSettings().FontSize;

            var document = DocX.Create("TEMP");

            string stringFormatGS = "{0:f" + AppSetting.GetSettings().RoundCoefmr.ToString() + "}";
            string stringFormatTY = "{0:f" + AppSetting.GetSettings().RoundCoefval.ToString() + "}";

            List<Emission> emissions;


            document.InsertParagraph("Расчет произведен программой «Расчет выбросов от сварки геомембраны»").Font(FontFamily).Bold().FontSize(FontSize).Alignment = Alignment.center;
            document.InsertParagraph();
            document.InsertParagraph("Расчет выбросов загрязняющих веществ в соответствии «Расчетная инструкция (методика) «Удельные показатели образования вредных веществ, выделяющихся в атмосферу от основных видов технологического оборудования для предприятий радиоэлектронного комплекса» (утверждена Федеральным агентством по промышленности Российской Федерации, 2006 год)").Font(FontFamily).FontSize(FontSize - 1).Bold().Italic().Alignment = Alignment.center;
            document.InsertParagraph();


            emissions = new List<Emission>() {
                    new Emission() { Code = "0337", Title = "Углерода оксид (Углерод окись;\nуглерод моноокись; угарный газ)", Gsec=0, Tyear=0 },
                    new Emission() { Code = "1317", Title = "Ацетальдегид (Уксусный альдегид)", Gsec=0, Tyear=0 },
                    new Emission() {Code = "1325", Title = "Формальдегид (Муравьиный альдегид,\nоксометан, метиленоксид)", Gsec=0, Tyear=0},
                    new Emission() {Code = "1555", Title = "Этановая кислота (Метанкарбоновая кислота)", Gsec=0, Tyear=0}
                };


            foreach (CalcVariant calcVariant in calcVariants)
            {
                float Gsv = calcVariant.Type == "Экструзионная сварка" ? WeldingSetting.GetSettings().EfficienceE : WeldingSetting.GetSettings().EfficienceC;

                document.InsertParagraph($"Предприятие: {organization.Title}, Площадка: {calcVariant.Place}").Font(FontFamily).FontSize(FontSize - 1).Bold().Italic().Alignment = Alignment.center;
                document.InsertParagraph($"Источник: {calcVariant.Title}, номер источника: {calcVariant.Number}").Font(FontFamily).FontSize(FontSize - 1).Bold().Italic().Alignment = Alignment.center;
                document.InsertParagraph();
                document.InsertParagraph($"Тип источника: {calcVariant.Type}").Font(FontFamily).FontSize(FontSize - 1).Bold().Italic().Alignment = Alignment.center;
                document.InsertParagraph();
                document.InsertParagraph("Результаты расчета").Bold().Font(FontFamily).FontSize(FontSize).Alignment = Alignment.center;
                document.InsertParagraph();

                float m1 = (float)Math.Round(Gsv * calcVariant.Density * Math.Round(calcVariant.SeamWidth * calcVariant.SeamLength, 3) * calcVariant.SeamThick * calcVariant.SeamNum, 3);
                float Km = (float)Math.Round(Math.Round((calcVariant.SeamWidth + 0.25 * calcVariant.SeamLength) * calcVariant.SeamThick, 3) / Math.Round(calcVariant.SeamWidth * calcVariant.SeamLength, 3), 3);
                float Kt = calcVariant.Type == "Экструзионная сварка" ? WeldingSetting.GetSettings().TimeCoeffE : WeldingSetting.GetSettings().TimeCoeffC;
                float m3 = (float)Math.Round(Km * Kt * m1, 3);
                float T = (float)Math.Round(calcVariant.WorkVol / calcVariant.SeamLength, 3);
                float G = (float)Math.Round(m3 * T / 1000, AppSetting.GetSettings().RoundCoefval);
                float M = (float)Math.Round(m3 * 1000 / 3600, AppSetting.GetSettings().RoundCoefmr);


                var table = document.AddTable(5, 4);

                table.Rows[0].Cells[0].Paragraphs[0].Append("Код\n в-ва").Font(FontFamily).FontSize(FontSize - 2).Bold().Alignment = Alignment.center;
                table.Rows[0].Cells[1].Paragraphs[0].Append("Название\nвещества").Font(FontFamily).FontSize(FontSize - 2).Bold().Alignment = Alignment.center;
                table.Rows[0].Cells[2].Paragraphs[0].Append("Макс. выброс\n(г/с)").Font(FontFamily).FontSize(FontSize - 2).Bold().Alignment = Alignment.center;
                table.Rows[0].Cells[3].Paragraphs[0].Append("Валовый выброс\n(т/год)").Font(FontFamily).FontSize(FontSize - 2).Bold().Alignment = Alignment.center;

                table.Rows[1].Cells[0].Paragraphs[0].Append("0337").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[1].Cells[1].Paragraphs[0].Append("Углерода оксид (Углерод окись;\nуглерод моноокись; угарный газ)").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[1].Cells[2].Paragraphs[0].Append(string.Format(stringFormatGS, 0.3 * calcVariant.Gsec)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;
                table.Rows[1].Cells[3].Paragraphs[0].Append(string.Format(stringFormatTY, 0.3 * calcVariant.Tyear)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;

                table.Rows[2].Cells[0].Paragraphs[0].Append("1317").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[2].Cells[1].Paragraphs[0].Append("Ацетальдегид (Уксусный альдегид)").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[2].Cells[2].Paragraphs[0].Append(string.Format(stringFormatGS, 0.202 * calcVariant.Gsec)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;
                table.Rows[2].Cells[3].Paragraphs[0].Append(string.Format(stringFormatTY, 0.202 * calcVariant.Tyear)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;


                table.Rows[3].Cells[0].Paragraphs[0].Append("1325").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[3].Cells[1].Paragraphs[0].Append("Формальдегид (Муравьиный альдегид,\nоксометан, метиленоксид)").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[3].Cells[2].Paragraphs[0].Append(string.Format(stringFormatGS, 0.282 * calcVariant.Gsec)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;
                table.Rows[3].Cells[3].Paragraphs[0].Append(string.Format(stringFormatTY, 0.282 * calcVariant.Tyear)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;


                table.Rows[4].Cells[0].Paragraphs[0].Append("1555").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[4].Cells[1].Paragraphs[0].Append("Этановая кислота (Метанкарбоновая кислота)").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[4].Cells[2].Paragraphs[0].Append(string.Format(stringFormatGS, 0.216 * calcVariant.Gsec)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;
                table.Rows[4].Cells[3].Paragraphs[0].Append(string.Format(stringFormatTY, 0.216 * calcVariant.Tyear)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;


                if (WeldingSetting.GetSettings().ParWeld)
                {
                    emissions.Where(o => o.Code == "0337").First().Gsec += (float)Math.Round(0.3 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr);
                    emissions.Where(o => o.Code == "0337").First().Tyear += (float)Math.Round(0.3 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefval);
                    emissions.Where(o => o.Code == "1317").First().Gsec += (float)Math.Round(0.202 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr);
                    emissions.Where(o => o.Code == "1317").First().Tyear += (float)Math.Round(0.202 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefval);
                    emissions.Where(o => o.Code == "1325").First().Gsec += (float)Math.Round(0.282 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr);
                    emissions.Where(o => o.Code == "1325").First().Tyear += (float)Math.Round(0.282 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefval);
                    emissions.Where(o => o.Code == "1555").First().Gsec += (float)Math.Round(0.216 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr);
                    emissions.Where(o => o.Code == "1555").First().Tyear += (float)Math.Round(0.216 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefval);
                }
                else
                {
                    emissions.Where(o => o.Code == "0337").First().Gsec = emissions.Where(o => o.Code == "0337").First().Gsec < (float)Math.Round(0.3 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr) ? (float)Math.Round(0.3 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr) : emissions.Where(o => o.Code == "0337").First().Gsec;
                    emissions.Where(o => o.Code == "0337").First().Tyear += (float)Math.Round(0.3 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefval);
                    emissions.Where(o => o.Code == "1317").First().Gsec = emissions.Where(o => o.Code == "1317").First().Gsec < (float)Math.Round(0.202 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr) ? (float)Math.Round(0.202 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr) : emissions.Where(o => o.Code == "1317").First().Gsec; ;
                    emissions.Where(o => o.Code == "1317").First().Tyear += (float)Math.Round(0.202 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefval);
                    emissions.Where(o => o.Code == "1325").First().Gsec = emissions.Where(o => o.Code == "1325").First().Gsec < (float)Math.Round(0.282 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr) ? (float)Math.Round(0.282 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr) : emissions.Where(o => o.Code == "1325").First().Gsec; ;
                    emissions.Where(o => o.Code == "1325").First().Tyear += (float)Math.Round(0.282 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefval);
                    emissions.Where(o => o.Code == "1555").First().Gsec = emissions.Where(o => o.Code == "1555").First().Gsec < (float)Math.Round(0.216 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr) ? (float)Math.Round(0.216 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr) : emissions.Where(o => o.Code == "1555").First().Gsec; ;
                    emissions.Where(o => o.Code == "1555").First().Tyear += (float)Math.Round(0.216 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefval);
                }

                table.AutoFit = AutoFit.Contents;
                table.Alignment = Alignment.center;

                Xceed.Document.NET.Border border = new Xceed.Document.NET.Border();
                border.Tcbs = BorderStyle.Tcbs_double;

                table.SetBorder(TableBorderType.Top, border);
                table.SetBorder(TableBorderType.Left, border);
                table.SetBorder(TableBorderType.Right, border);

                document.InsertTable(table);


                document.InsertParagraph();
                document.InsertParagraph("Расчетные формулы, исходные данные").Bold().FontSize(FontSize).Font(FontFamily).Alignment = Alignment.center;
                document.InsertParagraph("Расчет массы расплавленной пленки:").Bold().FontSize(FontSize).Font(FontFamily).Alignment = Alignment.both;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("m").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" = ").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("G").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("св").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("·").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("g").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" S ").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("·").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" h ").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("·").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" n ").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(", кг/час").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("     (58)").Font(FontFamily).FontSize(FontSize).Bold().Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("G").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("св").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" = ").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"{Gsv} швов в час").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" - производительность сварочного аппарата").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"g = {calcVariant.Density} кг/м").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("3").Font(FontFamily).Script(Script.superscript).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" - плотность пленки (по технологическим данным)").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("S = а·в - площадь свариваемого шва, м").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("2").Font(FontFamily).Script(Script.superscript).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("     (59)").Font(FontFamily).FontSize(FontSize).Bold().Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"а = {calcVariant.SeamWidth} м - ширина шва (по технологическим данным)").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"в = {calcVariant.SeamLength} м - длина шва (равна максимальной производительности сварочного аппарата в час по длине шва по технологическим данным)").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"S = {calcVariant.SeamWidth}·{calcVariant.SeamLength} = {Math.Round(calcVariant.SeamWidth * calcVariant.SeamLength, 3)} м").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("2").Font(FontFamily).Script(Script.superscript).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"h = {calcVariant.SeamThick} м - толщина свариваемого шва (по технологическим данным)").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"n = {calcVariant.SeamNum} шт - количество швов за один проход").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("m").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {Gsv}·{calcVariant.Density}·{Math.Round(calcVariant.SeamWidth * calcVariant.SeamLength, 3)}·{calcVariant.SeamThick}·{calcVariant.SeamNum} = {m1} кг/час").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph("Масса паров, выделяющихся в воздушную среду:").Bold().FontSize(FontSize).Font(FontFamily).Alignment = Alignment.both;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("m").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("3").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" = ").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("K").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("m").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("·").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("K").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("t").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("·").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("m").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(", кг/час").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("    (60)").Font(FontFamily).FontSize(FontSize).Bold().Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("K").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("m").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" = S").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("/S").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("2").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("S").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = ({calcVariant.SeamWidth} + 0,25·{calcVariant.SeamLength})·{calcVariant.SeamThick} = {Math.Round((calcVariant.SeamWidth + 0.25 * calcVariant.SeamLength) * calcVariant.SeamThick, 3)} м").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("2").Font(FontFamily).FontSize(FontSize).Script(Script.superscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" - площадь свариваемого шва, с которого выделяются вредные вещества").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("S").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("2").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {calcVariant.SeamWidth}·{calcVariant.SeamLength} = {Math.Round(calcVariant.SeamWidth * calcVariant.SeamLength, 3)} м").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("2").Font(FontFamily).FontSize(FontSize).Script(Script.superscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append(" - площадь свариваемого шва").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("K").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("m").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {Math.Round((calcVariant.SeamWidth + 0.25 * calcVariant.SeamLength) * calcVariant.SeamThick, 3)}·{Math.Round(calcVariant.SeamWidth * calcVariant.SeamLength, 3)} = {Km}").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("м").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("2").Font(FontFamily).FontSize(FontSize).Script(Script.superscript).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("K").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("t").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {Kt} - коэффициент, учитывающий временной фактор выделения вредностей, б/р").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("m").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("3").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {Km}·{Kt}·{m1} = {m3} кг/час").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph("Максимально-разовый выброс загрязняющих веществ определяется по формуле:").Bold().FontSize(FontSize).Font(FontFamily).Alignment = Alignment.both;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"M = m").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("3").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"·1000/3600 = {Math.Round(m3 * 1000 / 3600, AppSetting.GetSettings().RoundCoefmr)} г/сек").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph("Массовая доля по веществам принята согласно таблице 14.5 методики.").FontSize(FontSize).Font(FontFamily).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("M").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("0337").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {0.202} · {calcVariant.Gsec} = {string.Format(stringFormatGS, Math.Round(0.300 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr))} г/сек").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("M").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1317").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {0.300} · {calcVariant.Gsec} = {string.Format(stringFormatGS, Math.Round(0.202 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr))} г/сек").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("M").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1325").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {0.282} · {calcVariant.Gsec} = {string.Format(stringFormatGS, Math.Round(0.282 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr))} г/сек").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("M").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1555").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {0.216} · {calcVariant.Gsec} = {string.Format(stringFormatGS, Math.Round(0.216 * calcVariant.Gsec, AppSetting.GetSettings().RoundCoefmr))} г/сек").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph("Валовый выброс загрязняющих веществ определяется по формуле:").Bold().FontSize(FontSize).Font(FontFamily).Alignment = Alignment.both;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"G = m").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("3").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"·T/1000, т/год").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"T = V / p, час - время сварочных работ").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"V = {calcVariant.WorkVol} п.м. - длина свариваемого полотна").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"p = {calcVariant.SeamLength} м/час - производительность сварочного аппарата в час").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"T = {calcVariant.WorkVol} / {calcVariant.SeamLength} = {T}, час").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append($"G = {m3}·{T}/1000 = {G}т/год").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("M").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("0337").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {0.202} · {calcVariant.Gsec} = {string.Format(stringFormatTY, Math.Round(0.300 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefmr))} т/год").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("M").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1317").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {0.300} · {calcVariant.Gsec} = {string.Format(stringFormatTY, Math.Round(0.202 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefmr))} т/год").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("M").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1325").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {0.282} · {calcVariant.Gsec} = {string.Format(stringFormatTY, Math.Round(0.282 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefmr))} т/год").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.InsertParagraph();
                document.Paragraphs[document.Paragraphs.Count - 1].Append("M").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append("1555").Font(FontFamily).FontSize(FontSize).Script(Script.subscript).Alignment = Alignment.left;
                document.Paragraphs[document.Paragraphs.Count - 1].Append($" = {0.216} · {calcVariant.Gsec} = {string.Format(stringFormatTY, Math.Round(0.216 * calcVariant.Tyear, AppSetting.GetSettings().RoundCoefmr))} т/год").Font(FontFamily).FontSize(FontSize).Alignment = Alignment.left;
            }

            if (calcVariants.Count > 1)
            {
                document.InsertParagraph("Итоговые результаты расчета").Bold().Font(FontFamily).FontSize(FontSize).Alignment = Alignment.center;
                var table = document.AddTable(5, 4);
                document.InsertParagraph();

                table.Rows[0].Cells[0].Paragraphs[0].Append("Код\n в-ва").Font(FontFamily).FontSize(FontSize - 2).Bold().Alignment = Alignment.center;
                table.Rows[0].Cells[1].Paragraphs[0].Append("Название\nвещества").Font(FontFamily).FontSize(FontSize - 2).Bold().Alignment = Alignment.center;
                table.Rows[0].Cells[2].Paragraphs[0].Append("Макс. выброс\n(г/с)").Font(FontFamily).FontSize(FontSize - 2).Bold().Alignment = Alignment.center;
                table.Rows[0].Cells[3].Paragraphs[0].Append("Валовый выброс\n(т/год)").Font(FontFamily).FontSize(FontSize - 2).Bold().Alignment = Alignment.center;

                table.Rows[1].Cells[0].Paragraphs[0].Append("0337").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[1].Cells[1].Paragraphs[0].Append("Углерода оксид (Углерод окись;\nуглерод моноокись; угарный газ)").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[1].Cells[2].Paragraphs[0].Append(string.Format(stringFormatGS, emissions.Where(o => o.Code == "0337").First().Gsec)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;
                table.Rows[1].Cells[3].Paragraphs[0].Append(string.Format(stringFormatTY, emissions.Where(o => o.Code == "0337").First().Tyear)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;

                table.Rows[2].Cells[0].Paragraphs[0].Append("1317").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[2].Cells[1].Paragraphs[0].Append("Ацетальдегид (Уксусный альдегид)").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[2].Cells[2].Paragraphs[0].Append(string.Format(stringFormatGS, emissions.Where(o => o.Code == "1317").First().Gsec)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;
                table.Rows[2].Cells[3].Paragraphs[0].Append(string.Format(stringFormatTY, emissions.Where(o => o.Code == "1317").First().Tyear)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;

                table.Rows[3].Cells[0].Paragraphs[0].Append("1325").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[3].Cells[1].Paragraphs[0].Append("Формальдегид (Муравьиный альдегид,\nоксометан, метиленоксид)").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[3].Cells[2].Paragraphs[0].Append(string.Format(stringFormatGS, emissions.Where(o => o.Code == "1325").First().Gsec)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;
                table.Rows[3].Cells[3].Paragraphs[0].Append(string.Format(stringFormatTY, emissions.Where(o => o.Code == "1325").First().Tyear)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;

                table.Rows[4].Cells[0].Paragraphs[0].Append("1555").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[4].Cells[1].Paragraphs[0].Append("Этановая кислота (Метанкарбоновая кислота)").Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.both;
                table.Rows[4].Cells[2].Paragraphs[0].Append(string.Format(stringFormatGS, emissions.Where(o => o.Code == "1555").First().Gsec)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;
                table.Rows[4].Cells[3].Paragraphs[0].Append(string.Format(stringFormatTY, emissions.Where(o => o.Code == "1555").First().Tyear)).Font(FontFamily).FontSize(FontSize - 2).Alignment = Alignment.right;

                table.AutoFit = AutoFit.Contents;
                table.Alignment = Alignment.center;

                Xceed.Document.NET.Border border = new Xceed.Document.NET.Border();
                border.Tcbs = BorderStyle.Tcbs_double;

                table.SetBorder(TableBorderType.Top, border);
                table.SetBorder(TableBorderType.Left, border);
                table.SetBorder(TableBorderType.Right, border);

                document.InsertTable(table);
            }
            try
            {
                document.Save();
                var p = new Process();
                p.StartInfo = new ProcessStartInfo("TEMP.docx")
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
