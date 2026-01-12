

#if ANDROID
using Android.App;
using Android.Content;
using Android.Provider;
using System;
using System.Threading.Tasks;
#endif
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Income.Common;
using Income.Database;
using Income.Database.Models.SCH0_0;
using Income.Database.Models.HIS_2026;
using System.Diagnostics;
using System.Globalization;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using FontSize = DocumentFormat.OpenXml.Wordprocessing.FontSize;
using Income.Database.Queries;
using BootstrapBlazor.Components;

namespace Income.Common
{
    public class PrintHelper
    {

        //public Query SCH_0_0_Quaries = new();
        private readonly DBQueries _dbQueries;
        public PrintHelper(DBQueries dbQueries)
        {
            _dbQueries = dbQueries;
        }//Constructor...

        Tbl_Sch_0_0_Block_0_1? block_0_1 = new();
        List<Tbl_Sch_0_0_Block_2_1>? block_2_1 = new();
        List<Tbl_Sch_0_0_Block_2_2>? block_2_2 = new();
        Tbl_Sch_0_0_Block_4? block_4 = new();
        List<Tbl_Sch_0_0_Block_5>? block_5 = new();
        List<Tbl_Sch_0_0_Block_7>? block_7 = new();
        Tbl_Sch_0_0_FieldOperation? block_11 = new();//sch00

        Tbl_Block_1? blockhis_1 = new();
        List<Tbl_Block_3>? blockhis_3 = new();
        Tbl_Block_4? blockhis_4 = new();
        List<Tbl_Block_4_Q5>? blockhis_4Q5 = new();
        List<Tbl_Block_5>? blockhis_5 = new();
        List<Tbl_Block_6>? blockhis_6 = new();
        List<Tbl_Block_8_Q6>? blockhis_8_Q6 = new();
        Tbl_Block_8 blockhis_8 = new();
        Tbl_Block_9a blockhis_9a = new();
        Tbl_Block_9b blockhis_9b = new();
        Tbl_Block_10? blockhis_10 = new();
        Tbl_Block_11a? block_11a = new();
        List<Tbl_Block_11b>? block_11b_list = new();
        Tbl_Block_B? blockHis_B = new();
        Tbl_Block_A? blockHis_A = new();
        Tbl_Block_7a? blockHis_7a = new();
        List<Tbl_Block_7a_1>? blockhis_7a_1 = new();
        Tbl_Block_7b? blockHis_7b = new();
        List<Tbl_Block_7c_NIC>? blockhis_7c_Nic = new();
        List<Tbl_Block_7c_Q10>? blockhis_7c_Q10 = new();
        Tbl_Block_7c? blockHis_7c = new();
        List<Tbl_Block_7d>? blockhis_7d= new();

        string reportType { get; set; } = string.Empty;
        byte[]? generatedDoc2026 = null;

        //int hhdId = 0;
        //public PrintHelper(ScheduleZeroDatabase _scheduleZeroDatabase, ScheduleTenOFourDatabase sch104Db)
        //{
        //    _ScheduleZeroDatabase = _scheduleZeroDatabase;
        //    _sch104Db = sch104Db;
        //}
        public async Task<int> GenerateReportAsync(int FsuId , string reportType)
        {
            try
            {
                //hhdId = HhdId;
                string printDir;
                string downloadsDir;
                var fileName = $"{FsuId}.docx";

                //Stream templateStream;
                Stream sch00TemplateStream=null;
                Stream sch26TemplateStream=null;


#if WINDOWS
        // Access from Resources/Template folder in Windows
        var templatePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Template", "IncomeSch00Template.docx");
        var template2026Path = Path.Combine(AppContext.BaseDirectory, "Resources", "Template", "TemplateHis2026.docx");
        if (reportType == "Sch00")
            {
                sch00TemplateStream = File.OpenRead(templatePath);
            }
            else
            {
                sch00TemplateStream = File.OpenRead(templatePath);
                sch26TemplateStream = File.OpenRead(template2026Path);
            }
        //sch00TemplateStream = File.OpenRead(templatePath);
        //sch26TemplateStream = File.OpenRead(template2026Path);
#else
                if (reportType=="Sch00")
                {
                    sch00TemplateStream = await FileSystem.OpenAppPackageFileAsync("Resources/Template/IncomeSch00Template.docx");
                }
                else
                {
                    sch00TemplateStream = await FileSystem.OpenAppPackageFileAsync("Resources/Template/IncomeSch00Template.docx");
                    sch26TemplateStream = await FileSystem.OpenAppPackageFileAsync("Resources/Template/TemplateHis2026.docx");
                }
                   
#endif
               
                    var generatedDoc00 = await FillDocument00(sch00TemplateStream);
                if (generatedDoc00 == null || generatedDoc00.Length == 0)
                    return 0;
                if (reportType != "Sch00")
                {
                     generatedDoc2026 = await FillDocument26(sch26TemplateStream);

                    if (generatedDoc2026 == null || generatedDoc2026.Length == 0)
                        return 0;
                }
               
                string fsuId = SessionStorage.SelectedFSUId.ToString();
                string datePart = DateTime.Now.ToString("ddMMMyyyy", CultureInfo.InvariantCulture);
                string SCH00fileName = $"Income_Sch 00_{fsuId}_{datePart}.docx";
                string SCH2026fileName = $"Income_Sch 2026_{fsuId}_{datePart}.docx";
#if WINDOWS
                downloadsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

#elif ANDROID
                downloadsDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments)?.AbsolutePath ?? string.Empty;
#else
                downloadsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif
                printDir = Path.Combine(downloadsDir, "Print");
                if (!Directory.Exists(printDir))
                    Directory.CreateDirectory(printDir);

                string targetPath00 = Path.Combine(printDir, SCH00fileName);
                //string targetPath2026 = Path.Combine(printDir, SCH2026fileName);
                await File.WriteAllBytesAsync(targetPath00, generatedDoc00);
                if (reportType != "Sch00")
                {
                    string targetPath2026 = Path.Combine(printDir, SCH2026fileName);
                    await File.WriteAllBytesAsync(targetPath2026, generatedDoc2026);
                }
                //await File.WriteAllBytesAsync(targetPath2026, generatedDoc2026);
                return 1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating report: {ex.Message}");
                return 0;
            }
        }

        public async Task<byte[]> FillDocument00(Stream templatePath00)
        {
            try
            {
                using var ms = new MemoryStream();
                await templatePath00.CopyToAsync(ms);
                ms.Position = 0;
                var commonQueries = new CommonQueries();
                //var dbQueries = new DBQueries();
                block_0_1 = await _dbQueries.FetchBlock1();
                block_2_1 = await _dbQueries.FetchSCH0Block2_1Data();
                block_2_2 = await _dbQueries.FetchSCH0Block2_2Data();
                block_4 = await _dbQueries.GetBlock4();
                block_5 = await _dbQueries.FetchSCH0Block5Data();
                //block_7 = await dbQueries.GetBlock7Data();
                block_7 = await _dbQueries.Get_SCH0_0_Block_5A_HouseHoldBy_FSUP(SessionStorage.SelectedFSUId);
                block_11 = await _dbQueries.FetchBlock2();


                using (var doc = WordprocessingDocument.Open(ms, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;

                    FillBlock0_1(body, block_0_1);
                    FillBlock2_1(body, block_2_1);
                    FillBlock2_2(body, block_2_2);
                    //FillBlock2_1_And_2_2(body, block_2_1, block_2_2);
                    FillBlock4(body, block_4);
                    FillBlock5(body, block_5);
                    FillBlock7(body, block_7);
                    FillBlock7Remarks(body, block_0_1?.remarks_block_7);
                    FillBlock0_8(body, block_7);
                    FillBlock11(body, block_11);

                    // Save explicitly (safe)
                    doc.MainDocumentPart.Document.Save();
                }

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating document: {ex.Message}");
                return null;
            }
        }



        /// <summary>
        ///  Works even if cells are merged 
        ///  Always gives the code/number column
        ///  Does not break if Word layout changes
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>

        private TableCell GetLastCell(TableRow row)
        {
            return row.Elements<TableCell>().Last();
        }
        private void ReplaceCellText(TableCell cell, string newText)
        {
            cell.RemoveAllChildren<Paragraph>();

            var run = new Run(
                new RunProperties(
                    new FontSize() { Val = "20" },
                    new RunFonts() { Ascii = "Calibri" },
                    new Color() { Val = "000000" }
                ),
               new Text(newText)
            );

            var paragraph = new Paragraph(run);
            cell.AppendChild(paragraph);
        }
        private void FillBlock0_1(Body body, Tbl_Sch_0_0_Block_0_1 block01)
        {
            void SafeReplace(List<TableRow> rows, int index, string value)
            {
                if (rows.Count > index)
                    ReplaceCellText(GetLastCell(rows[index]), value ?? "");
            }

            // ---------- BLOCK [0] ----------
            var table0 = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[0]"));

            if (table0 != null)
            {
                var rows0 = table0.Elements<TableRow>().ToList();

                SafeReplace(rows0, 2, block01?.Block_0_1);
                SafeReplace(rows0, 3, block01?.Block_0_2);
                SafeReplace(rows0, 4, block01?.Block_0_3);
                SafeReplace(rows0, 5, block01?.Block_0_4);
                SafeReplace(rows0, 6, block01?.Block_0_5);
                SafeReplace(rows0, 7, block01?.Block_0_6);
            }

            // ---------- BLOCK [1] ----------
            var table1 = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[1]"));

            if (table1 != null)
            {
                var rows1 = table1.Elements<TableRow>().ToList();

                SafeReplace(rows1, 2, (block01?.Block_1_1 ?? ""));
                SafeReplace(rows1, 3, "");
                SafeReplace(rows1, 4, block01?.Block_1_3);
                SafeReplace(rows1, 5, (block01?.Block_1_4 ?? 0).ToString());
                SafeReplace(rows1, 6, (block01?.Block_1_5 ?? 0).ToString());
                SafeReplace(rows1, 7, block01?.Block_1_6);
                SafeReplace(rows1, 8, block01?.Block_1_7);
                SafeReplace(rows1, 9, block01?.Block_1_8);
                SafeReplace(rows1, 10, block01?.Block_1_9);
                SafeReplace(rows1, 11, (block01?.Block_1_10 ?? 0).ToString());
                SafeReplace(rows1, 12, block01?.Block_1_11);
                SafeReplace(rows1, 13, (block01?.Block_1_12 ?? 0).ToString());
                SafeReplace(rows1, 14, (block01?.Block_1_13 ?? 0).ToString());
                SafeReplace(rows1, 15, (block01?.Block_1_14 ?? 0).ToString());
                SafeReplace(rows1, 16, (block01?.Block_1_15 ?? 0).ToString());
                SafeReplace(rows1, 17, (block01?.Block_1_16 ?? 0).ToString());
                SafeReplace(rows1, 18, (block01?.Block_1_17 ?? 0).ToString());
                SafeReplace(rows1, 18, (block01?.remarks_block_1_17));
                SafeReplace(rows1, 19, block01?.remarks);
            }

            // ---------- BLOCK [9] ----------
            var table9 = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower()
                    .Contains("[9] remarks by field enumerator"));

            if (table9 != null)
            {
                var rows9 = table9.Elements<TableRow>().ToList();
                SafeReplace(rows9, 1, block01?.EnumeratorRemarks);
            }

            // ---------- BLOCK [10] ----------
            //var table10 = body.Elements<Table>()
            //    .FirstOrDefault(t => t.InnerText.ToLower()
            //        .Contains("[10] remarks by field supervisor"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[10] remarks by field supervisor"));
            var table10 = body.Elements<Table>().ElementAtOrDefault(11);

            if (table10 != null)
            {
                var rows10 = table10.Elements<TableRow>().ToList();
                SafeReplace(rows10, 1, block01?.SupervisorRemarks);
            }

        }//Block0_1...
        private void FillBlock2_1(Body body, List<Tbl_Sch_0_0_Block_2_1> list)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[2.1]"));

            if (table == null || list == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            TableRow templateRow = rows[2];
            TableRow remarksRow = rows.Last();

            // clear previous data rows
            for (int i = rows.Count - 2; i >= 2; i--)
                rows[i].Remove();

            int serial = 1;

            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.serial_no.ToString()!);
                ReplaceCellText(cells[1], item.hamlet_name ?? "");
                ReplaceCellText(cells[2], item.percentage?.ToString() ?? "");

                table.InsertBefore(row, remarksRow);
                serial++;
            }

            // ✅ remarks taken from existing block_0_1 field
            ReplaceCellText(
                GetLastCell(remarksRow),
                block_0_1?.remarks ?? ""
            );
        }

        private void FillBlock2_2(Body body,List<Tbl_Sch_0_0_Block_2_2> list)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[2.2]"));

            if (table == null || list == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // first data row as template
            TableRow templateRow = rows[2];
            TableRow remarksRow = rows.Last();

            // remove existing data rows
            for (int i = rows.Count - 2; i >= 2; i--)
                rows[i].Remove();

            int serial = 1;

            foreach (var item in list.OrderBy(x => x.serial_number))
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.serial_number.ToString()!);
                ReplaceCellText(cells[1], item.serial_no_of_hamlets_in_su ?? "");
                ReplaceCellText(cells[2], item.Percentage?.ToString() ?? "");
                ReplaceCellText(cells[3], item.is_selected ? "✔" : "");

                table.InsertBefore(row, remarksRow);
                serial++;
            }

            // ✅ remarks from Block-0.1 (no parameter needed)
            ReplaceCellText(
                GetLastCell(remarksRow),
                block_0_1?.remarks ?? ""
            );
        }

        private void FillBlock4(Body body, Tbl_Sch_0_0_Block_4? block4)
        {
            if (block4 == null)
                return;

            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[4]"));

            if (table == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            ReplaceCellText(rows[2].Elements<TableCell>().Last(), block4.sample_su_number?.ToString() ?? "");

            ReplaceCellText(rows[3].Elements<TableCell>().Last(), block4.approximate_population_su?.ToString() ?? "");

            ReplaceCellText(rows[4].Elements<TableCell>().Last(), block4.number_of_sub_division_of_su_to_be_formed?.ToString() ?? "");

            ReplaceCellText(GetLastCell(rows.Last()), block_0_1?.remarks_block_4 ?? "");
        }


        private void FillBlock5(Body body, List<Tbl_Sch_0_0_Block_5> list)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[5]"));

            if (table == null || list == null || !list.Any())
                return;

            var rows = table.Elements<TableRow>().ToList();
            var templateRow = rows[3];
            var remarksRow = rows.Last();

            // Remove old data rows (keep remarks)
            for (int i = rows.Count - 2; i >= 3; i--)
                rows[i].Remove();

            foreach (var item in list)
            {
                var newRow = (TableRow)templateRow.CloneNode(true);
                var cells = newRow.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.serial_number?.ToString() ?? "");
                ReplaceCellText(cells[1], item.Percentage?.ToString() ?? "");
                ReplaceCellText(cells[2], item.IsChecked == true ? "✔" : "");

                table.InsertBefore(newRow, remarksRow);
            }

            // Set remarks
            ReplaceCellText(
                GetLastCell(remarksRow),
                block_0_1?.remarks_block_5 ?? ""
            );
        }

        private void FillBlock7(Body body, List<Tbl_Sch_0_0_Block_7> list)
        {
            if (list == null || list.Count == 0)
                return;

            var table = body.Elements<Table>()
                .First(t => t.InnerText.ToLower().Contains("[7] list of households"));

            var rows = table.Elements<TableRow>().ToList();

            // ✅ template row is ALWAYS the last row
            TableRow templateRow = rows.Last();

            int serial = 1;

            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.Block_7_1.ToString()!);
                ReplaceCellText(cells[1], item.Block_7_2 ?? "");
                ReplaceCellText(cells[2], item.is_household?.ToString() ?? "");
                ReplaceCellText(cells[3], item.Block_7_3?.ToString() ?? "");
                ReplaceCellText(cells[4], item.Block_7_4 ?? "");
                ReplaceCellText(cells[5], item.Block_7_5?.ToString() ?? "");
                ReplaceCellText(cells[6], item.Block_7_6?.ToString() ?? "");
                ReplaceCellText(cells[7], item.Block_7_7?.ToString() ?? "");
                ReplaceCellText(cells[8], item.Block_7_8?.ToString() ?? "");
                ReplaceCellText(cells[9], item.Block_7_9?.ToString() ?? "");
                ReplaceCellText(cells[10], item.SSS.ToString());
                ReplaceCellText(cells[11], item.isSelected ? "✔" : "");

                table.AppendChild(row);
                serial++;
            }

            // remove original blank template row
            templateRow.Remove();
        }

        private void FillBlock7Remarks(Body body,string remarks)
        {
            //if (string.IsNullOrWhiteSpace(remarks))
            //    return;

            // Block-7 remarks table (2 columns, 1 row)
            var table = body.Elements<Table>()
                .First(t => t.InnerText.ToLower().Contains("[7]remarks"));

            var row = table.Elements<TableRow>().First();
            var cells = row.Elements<TableCell>().ToList();

            // 2nd column = remarks value
            ReplaceCellText(cells[1], remarks);
        }

        private void FillBlock0_8(Body body, List<Tbl_Sch_0_0_Block_7> block0_8)
        {
            int population = 0;

            int sss_11_listed_hhd = 0;
            int sss_12_listed_hhd = 0;
            int sss_21_listed_hhd = 0;
            int sss_22_listed_hhd = 0;
            int sss_31_listed_hhd = 0;
            int sss_32_listed_hhd = 0;
            int sss_40_listed_hhd = 0;
            int sss_90_listed_hhd = 0;

            int sss_11_selected_hhd = 0;
            int sss_12_selected_hhd = 0;
            int sss_21_selected_hhd = 0;
            int sss_22_selected_hhd = 0;
            int sss_31_selected_hhd = 0;
            int sss_32_selected_hhd = 0;
            int sss_40_selected_hhd = 0;
            int sss_90_selected_hhd = 0;

            int sss_11_originally_selected_hhd = 0;
            int sss_12_originally_selected_hhd = 0;
            int sss_21_originally_selected_hhd = 0;
            int sss_22_originally_selected_hhd = 0;
            int sss_31_originally_selected_hhd = 0;
            int sss_32_originally_selected_hhd = 0;
            int sss_40_originally_selected_hhd = 0;
            int sss_90_originally_selected_hhd = 0;

            int sss_11_substituted_hhd = 0;
            int sss_12_substituted_hhd = 0;
            int sss_21_substituted_hhd = 0;
            int sss_22_substituted_hhd = 0;
            int sss_31_substituted_hhd = 0;
            int sss_32_substituted_hhd = 0;
            int sss_40_substituted_hhd = 0;
            int sss_90_substituted_hhd = 0;

            int sss_11_col_8_total_hhd = 0;
            int sss_12_col_8_total_hhd = 0;
            int sss_21_col_8_total_hhd = 0;
            int sss_22_col_8_total_hhd = 0;
            int sss_31_col_8_total_hhd = 0;
            int sss_32_col_8_total_hhd = 0;
            int sss_40_col_8_total_hhd = 0;
            int sss_90_col_8_total_hhd = 0;

            int sss_11_casualty_hhd = 0;
            int sss_12_casualty_hhd = 0;
            int sss_21_casualty_hhd = 0;
            int sss_22_casualty_hhd = 0;
            int sss_31_casualty_hhd = 0;
            int sss_32_casualty_hhd = 0;
            int sss_40_casualty_hhd = 0;
            int sss_90_casualty_hhd = 0;

            int total_listed = 0;
            int total_selected = 0;
            int total_originally_selected = 0;
            int total_substituted = 0;
            int total_col_8_total = 0;
            int total_casualty = 0;

            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[8]"));
            var table = body.Elements<Table>().ElementAtOrDefault(9);
            if (table != null)
            {
                var cells = table.Descendants<TableCell>().ToList();

                if (block0_8 != null && block0_8.Count > 0)
                {
                    population = block0_8.Sum(x => x.Block_7_5.GetValueOrDefault());
                    int sector = SessionStorage.FSU_Sector;
                    ReplaceCellText(cells[31], population.ToString());
                    if (SessionStorage.FSU_Sector == 1)
                    {
                        ReplaceCellText(cells[30], "HIS - Rural Sector\n(Feb 2026 - Jan 2027)");
                        ReplaceCellText(cells[32], "11");
                        ReplaceCellText(cells[41], "12");
                        ReplaceCellText(cells[50], "21");
                        ReplaceCellText(cells[59], "22");
                        ReplaceCellText(cells[68], "31");
                        ReplaceCellText(cells[77], "32");
                        ReplaceCellText(cells[86], "40");
                        ReplaceCellText(cells[95], "90");
                    }
                    else
                    {
                        ReplaceCellText(cells[30], "HIS - Urban Sector\n(Feb 2026 - Jan 2027))");
                        ReplaceCellText(cells[32], "11");
                        ReplaceCellText(cells[41], "121");
                        ReplaceCellText(cells[50], "122");
                        ReplaceCellText(cells[59], "21");
                        ReplaceCellText(cells[68], "221");
                        ReplaceCellText(cells[77], "222");
                        ReplaceCellText(cells[86], "40");
                        ReplaceCellText(cells[95], "90");
                    }

                    if (sector == 1)
                    {
                        var sss_11_total_hhd = block0_8.Where(x => x.SSS == 11);
                        var sss_12_total_hhd = block0_8.Where(x => x.SSS == 12);
                        var sss_21_total_hhd = block0_8.Where(x => x.SSS == 21);
                        var sss_22_total_hhd = block0_8.Where(x => x.SSS == 22);
                        var sss_31_total_hhd = block0_8.Where(x => x.SSS == 31);
                        var sss_32_total_hhd = block0_8.Where(x => x.SSS == 32);
                        var sss_40_total_hhd = block0_8.Where(x => x.SSS == 40);
                        var sss_90_total_hhd = block0_8.Where(x => x.SSS == 90);


                        sss_11_listed_hhd = sss_11_total_hhd?.Count() ?? 0;
                        sss_12_listed_hhd = sss_12_total_hhd?.Count() ?? 0;
                        sss_21_listed_hhd = sss_21_total_hhd?.Count() ?? 0;
                        sss_22_listed_hhd = sss_22_total_hhd?.Count() ?? 0;
                        sss_31_listed_hhd = sss_31_total_hhd?.Count() ?? 0;
                        sss_32_listed_hhd = sss_32_total_hhd?.Count() ?? 0;
                        sss_40_listed_hhd = sss_40_total_hhd?.Count() ?? 0;
                        sss_90_listed_hhd = sss_90_total_hhd?.Count() ?? 0;

                        sss_11_selected_hhd = sss_11_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_12_selected_hhd = sss_12_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_21_selected_hhd = sss_21_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_22_selected_hhd = sss_22_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_31_selected_hhd = sss_31_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_32_selected_hhd = sss_32_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_40_selected_hhd = sss_40_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_90_selected_hhd = sss_90_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;


                        sss_11_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 11);
                        sss_12_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 12);
                        sss_21_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 21);
                        sss_22_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 22);
                        sss_31_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 31);
                        sss_32_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 32);
                        sss_40_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 40);
                        sss_90_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 90);


                        sss_11_substituted_hhd = GetFilteredSubstitution(block0_8, 11);
                        sss_12_substituted_hhd = GetFilteredSubstitution(block0_8, 12);
                        sss_21_substituted_hhd = GetFilteredSubstitution(block0_8, 21);
                        sss_22_substituted_hhd = GetFilteredSubstitution(block0_8, 22);
                        sss_31_substituted_hhd = GetFilteredSubstitution(block0_8, 31);
                        sss_32_substituted_hhd = GetFilteredSubstitution(block0_8, 32);
                        sss_40_substituted_hhd = GetFilteredSubstitution(block0_8, 40);
                        sss_90_substituted_hhd = GetFilteredSubstitution(block0_8, 90);

                        sss_11_col_8_total_hhd = sss_11_originally_selected_hhd + sss_11_substituted_hhd;
                        sss_12_col_8_total_hhd = sss_12_originally_selected_hhd + sss_12_substituted_hhd;
                        sss_21_col_8_total_hhd = sss_21_originally_selected_hhd + sss_21_substituted_hhd;
                        sss_22_col_8_total_hhd = sss_22_originally_selected_hhd + sss_22_substituted_hhd;
                        sss_31_col_8_total_hhd = sss_31_originally_selected_hhd + sss_31_substituted_hhd;
                        sss_32_col_8_total_hhd = sss_32_originally_selected_hhd + sss_32_substituted_hhd;
                        sss_40_col_8_total_hhd = sss_40_originally_selected_hhd + sss_40_substituted_hhd;
                        sss_90_col_8_total_hhd = sss_90_originally_selected_hhd + sss_90_substituted_hhd;

                        sss_11_casualty_hhd = sss_11_selected_hhd - sss_11_col_8_total_hhd;
                        sss_12_casualty_hhd = sss_12_selected_hhd - sss_12_col_8_total_hhd;
                        sss_21_casualty_hhd = sss_21_selected_hhd - sss_21_col_8_total_hhd;
                        sss_22_casualty_hhd = sss_22_selected_hhd - sss_22_col_8_total_hhd;
                        sss_31_casualty_hhd = sss_31_selected_hhd - sss_31_col_8_total_hhd;
                        sss_32_casualty_hhd = sss_32_selected_hhd - sss_32_col_8_total_hhd;
                        sss_40_casualty_hhd = sss_40_selected_hhd - sss_40_col_8_total_hhd;
                        sss_90_casualty_hhd = sss_90_selected_hhd - sss_90_col_8_total_hhd;

                        total_listed = sss_11_listed_hhd + sss_12_listed_hhd + sss_21_listed_hhd + sss_22_listed_hhd + sss_31_listed_hhd + sss_32_listed_hhd + sss_40_listed_hhd + sss_90_listed_hhd;
                        total_selected = sss_11_selected_hhd + sss_12_selected_hhd + sss_21_selected_hhd + sss_22_selected_hhd + sss_31_selected_hhd + sss_32_selected_hhd + sss_40_selected_hhd + sss_90_selected_hhd;
                        total_originally_selected = sss_11_originally_selected_hhd + sss_12_originally_selected_hhd + sss_21_originally_selected_hhd + sss_22_originally_selected_hhd + sss_31_originally_selected_hhd + sss_32_originally_selected_hhd + sss_40_originally_selected_hhd + sss_90_originally_selected_hhd;
                        total_substituted = sss_11_substituted_hhd + sss_12_substituted_hhd + sss_21_substituted_hhd + sss_22_substituted_hhd + sss_31_substituted_hhd + sss_32_substituted_hhd + sss_40_substituted_hhd + sss_90_substituted_hhd;
                        total_col_8_total = sss_11_col_8_total_hhd + sss_12_col_8_total_hhd + sss_21_col_8_total_hhd + sss_22_col_8_total_hhd + sss_31_col_8_total_hhd + sss_32_col_8_total_hhd + sss_40_col_8_total_hhd + sss_90_col_8_total_hhd;
                        total_casualty = sss_11_casualty_hhd + sss_12_casualty_hhd + sss_21_casualty_hhd + sss_22_casualty_hhd + sss_31_casualty_hhd + sss_32_casualty_hhd + sss_40_casualty_hhd + sss_90_casualty_hhd;
                    }

                    else if (sector == 2)
                    {
                        var sss_11_total_hhd = block0_8.Where(x => x.SSS == 11);
                        var sss_121_total_hhd = block0_8.Where(x => x.SSS == 121);
                        var sss_122_total_hhd = block0_8.Where(x => x.SSS == 122);
                        var sss_21_total_hhd = block0_8.Where(x => x.SSS == 21);
                        var sss_221_total_hhd = block0_8.Where(x => x.SSS == 221);
                        var sss_222_total_hhd = block0_8.Where(x => x.SSS == 222);
                        var sss_40_total_hhd = block0_8.Where(x => x.SSS == 40);
                        var sss_90_total_hhd = block0_8.Where(x => x.SSS == 90);
                        sss_11_listed_hhd = sss_11_total_hhd?.Count() ?? 0;
                        sss_12_listed_hhd = sss_121_total_hhd?.Count() ?? 0;
                        sss_21_listed_hhd = sss_122_total_hhd?.Count() ?? 0;
                        sss_22_listed_hhd = sss_21_total_hhd?.Count() ?? 0;
                        sss_31_listed_hhd = sss_221_total_hhd?.Count() ?? 0;
                        sss_32_listed_hhd = sss_222_total_hhd?.Count() ?? 0;
                        sss_40_listed_hhd = sss_40_total_hhd?.Count() ?? 0;
                        sss_90_listed_hhd = sss_90_total_hhd?.Count() ?? 0;

                        sss_11_selected_hhd = sss_11_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_12_selected_hhd = sss_121_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_21_selected_hhd = sss_122_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_22_selected_hhd = sss_21_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_31_selected_hhd = sss_221_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_32_selected_hhd = sss_222_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_40_selected_hhd = sss_40_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;
                        sss_90_selected_hhd = sss_90_total_hhd?.Where(x => x.isInitialySelected == true).Count() ?? 0;


                        sss_11_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 11);
                        sss_12_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 121);
                        sss_21_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 122);
                        sss_22_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 21);
                        sss_31_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 221);
                        sss_32_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 222);
                        sss_40_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 40);
                        sss_90_originally_selected_hhd = GetFilteredCountOnInitialySelected(block0_8, 90);


                        sss_11_substituted_hhd = GetFilteredSubstitution(block0_8, 11);
                        sss_12_substituted_hhd = GetFilteredSubstitution(block0_8, 121);
                        sss_21_substituted_hhd = GetFilteredSubstitution(block0_8, 122);
                        sss_22_substituted_hhd = GetFilteredSubstitution(block0_8, 21);
                        sss_31_substituted_hhd = GetFilteredSubstitution(block0_8, 221);
                        sss_32_substituted_hhd = GetFilteredSubstitution(block0_8, 222);
                        sss_40_substituted_hhd = GetFilteredSubstitution(block0_8, 40);
                        sss_90_substituted_hhd = GetFilteredSubstitution(block0_8, 90);

                        sss_11_col_8_total_hhd = sss_11_originally_selected_hhd + sss_11_substituted_hhd;
                        sss_12_col_8_total_hhd = sss_12_originally_selected_hhd + sss_12_substituted_hhd;
                        sss_21_col_8_total_hhd = sss_21_originally_selected_hhd + sss_21_substituted_hhd;
                        sss_22_col_8_total_hhd = sss_22_originally_selected_hhd + sss_22_substituted_hhd;
                        sss_31_col_8_total_hhd = sss_31_originally_selected_hhd + sss_31_substituted_hhd;
                        sss_32_col_8_total_hhd = sss_32_originally_selected_hhd + sss_32_substituted_hhd;
                        sss_40_col_8_total_hhd = sss_40_originally_selected_hhd + sss_40_substituted_hhd;
                        sss_90_col_8_total_hhd = sss_90_originally_selected_hhd + sss_90_substituted_hhd;

                        sss_11_casualty_hhd = sss_11_selected_hhd - sss_11_col_8_total_hhd;
                        sss_12_casualty_hhd = sss_12_selected_hhd - sss_12_col_8_total_hhd;
                        sss_21_casualty_hhd = sss_21_selected_hhd - sss_21_col_8_total_hhd;
                        sss_22_casualty_hhd = sss_22_selected_hhd - sss_22_col_8_total_hhd;
                        sss_31_casualty_hhd = sss_31_selected_hhd - sss_31_col_8_total_hhd;
                        sss_32_casualty_hhd = sss_32_selected_hhd - sss_32_col_8_total_hhd;
                        sss_40_casualty_hhd = sss_40_selected_hhd - sss_40_col_8_total_hhd;
                        sss_90_casualty_hhd = sss_90_selected_hhd - sss_90_col_8_total_hhd;

                        total_listed = sss_11_listed_hhd + sss_12_listed_hhd + sss_21_listed_hhd + sss_22_listed_hhd + sss_31_listed_hhd + sss_32_listed_hhd + sss_40_listed_hhd + sss_90_listed_hhd;
                        total_selected = sss_11_selected_hhd + sss_12_selected_hhd + sss_21_selected_hhd + sss_22_selected_hhd + sss_31_selected_hhd + sss_32_selected_hhd + sss_40_selected_hhd + sss_90_selected_hhd;
                        total_originally_selected = sss_11_originally_selected_hhd + sss_12_originally_selected_hhd + sss_21_originally_selected_hhd + sss_22_originally_selected_hhd + sss_31_originally_selected_hhd + sss_32_originally_selected_hhd + sss_40_originally_selected_hhd + sss_90_originally_selected_hhd;
                        total_substituted = sss_11_substituted_hhd + sss_12_substituted_hhd + sss_21_substituted_hhd + sss_22_substituted_hhd + sss_31_substituted_hhd + sss_32_substituted_hhd + sss_40_substituted_hhd + sss_90_substituted_hhd;
                        total_col_8_total = sss_11_col_8_total_hhd + sss_12_col_8_total_hhd + sss_21_col_8_total_hhd + sss_22_col_8_total_hhd + sss_31_col_8_total_hhd + sss_32_col_8_total_hhd + sss_40_col_8_total_hhd + sss_90_col_8_total_hhd;
                        total_casualty = sss_11_casualty_hhd + sss_12_casualty_hhd + sss_21_casualty_hhd + sss_22_casualty_hhd + sss_31_casualty_hhd + sss_32_casualty_hhd + sss_40_casualty_hhd + sss_90_casualty_hhd;
                    }


                    ReplaceCellText(cells[33], sss_11_listed_hhd.ToString());
                    ReplaceCellText(cells[34], sss_11_selected_hhd.ToString());
                    ReplaceCellText(cells[35], sss_11_originally_selected_hhd.ToString());
                    ReplaceCellText(cells[36], sss_11_substituted_hhd.ToString());
                    ReplaceCellText(cells[37], sss_11_col_8_total_hhd.ToString());
                    ReplaceCellText(cells[38], sss_11_casualty_hhd.ToString());

                    ReplaceCellText(cells[42], sss_12_listed_hhd.ToString());
                    ReplaceCellText(cells[43], sss_12_selected_hhd.ToString());
                    ReplaceCellText(cells[44], sss_12_originally_selected_hhd.ToString());
                    ReplaceCellText(cells[45], sss_12_substituted_hhd.ToString());
                    ReplaceCellText(cells[46], sss_12_col_8_total_hhd.ToString());
                    ReplaceCellText(cells[47], sss_12_casualty_hhd.ToString());

                    ReplaceCellText(cells[51], sss_21_listed_hhd.ToString());
                    ReplaceCellText(cells[52], sss_21_selected_hhd.ToString());
                    ReplaceCellText(cells[53], sss_21_originally_selected_hhd.ToString());
                    ReplaceCellText(cells[54], sss_21_substituted_hhd.ToString());
                    ReplaceCellText(cells[55], sss_21_col_8_total_hhd.ToString());
                    ReplaceCellText(cells[56], sss_21_casualty_hhd.ToString());

                    ReplaceCellText(cells[60], sss_22_listed_hhd.ToString());
                    ReplaceCellText(cells[61], sss_22_selected_hhd.ToString());
                    ReplaceCellText(cells[62], sss_22_originally_selected_hhd.ToString());
                    ReplaceCellText(cells[63], sss_22_substituted_hhd.ToString());
                    ReplaceCellText(cells[64], sss_22_col_8_total_hhd.ToString());
                    ReplaceCellText(cells[65], sss_22_casualty_hhd.ToString());

                    ReplaceCellText(cells[69], sss_31_listed_hhd.ToString());
                    ReplaceCellText(cells[70], sss_31_selected_hhd.ToString());
                    ReplaceCellText(cells[71], sss_31_originally_selected_hhd.ToString());
                    ReplaceCellText(cells[72], sss_31_substituted_hhd.ToString());
                    ReplaceCellText(cells[73], sss_31_col_8_total_hhd.ToString());
                    ReplaceCellText(cells[74], sss_31_casualty_hhd.ToString());

                    ReplaceCellText(cells[78], sss_32_listed_hhd.ToString());
                    ReplaceCellText(cells[79], sss_32_selected_hhd.ToString());
                    ReplaceCellText(cells[80], sss_32_originally_selected_hhd.ToString());
                    ReplaceCellText(cells[81], sss_32_substituted_hhd.ToString());
                    ReplaceCellText(cells[82], sss_32_col_8_total_hhd.ToString());
                    ReplaceCellText(cells[83], sss_32_casualty_hhd.ToString());

                    ReplaceCellText(cells[87], sss_40_listed_hhd.ToString());
                    ReplaceCellText(cells[88], sss_40_selected_hhd.ToString());
                    ReplaceCellText(cells[89], sss_40_originally_selected_hhd.ToString());
                    ReplaceCellText(cells[90], sss_40_substituted_hhd.ToString());
                    ReplaceCellText(cells[91], sss_40_col_8_total_hhd.ToString());
                    ReplaceCellText(cells[92], sss_40_casualty_hhd.ToString());

                    ReplaceCellText(cells[96], sss_90_listed_hhd.ToString());
                    ReplaceCellText(cells[97], sss_90_selected_hhd.ToString());
                    ReplaceCellText(cells[98], sss_90_originally_selected_hhd.ToString());
                    ReplaceCellText(cells[99], sss_90_substituted_hhd.ToString());
                    ReplaceCellText(cells[100], sss_90_col_8_total_hhd.ToString());
                    ReplaceCellText(cells[101], sss_90_casualty_hhd.ToString());

                    ReplaceCellText(cells[105], total_listed.ToString());
                    ReplaceCellText(cells[106], total_selected.ToString());
                    ReplaceCellText(cells[107], total_originally_selected.ToString());
                    ReplaceCellText(cells[108], total_substituted.ToString());
                    ReplaceCellText(cells[109], total_col_8_total.ToString());
                    ReplaceCellText(cells[110], total_casualty.ToString());

                }
            }

        }


        #region Helper Methods
        private static int GetFilteredSubstitution(List<Tbl_Sch_0_0_Block_7> list, int sss)
        {
            int counter = 0;
            var initialSelectedList = list.Where(entry => entry.SSS == sss && entry.isInitialySelected == true && entry.SubstitutionCount != 0).ToList();
            foreach (var item in initialSelectedList)
            {
                if (item.SubstitutionCount >= 1)
                {
                    var filteredData = list.SingleOrDefault(entry => entry.isSelected == true && entry.OriginalHouseholdID == item.Block_7_3 && entry.SubstitutedForID == item.Block_7_3);
                    if (filteredData != null && filteredData.status == "SUBSTITUTE")
                    {
                        counter++;
                    }
                }
            }
            return initialSelectedList != null ? initialSelectedList.Count : 0;
        }

        private static int GetFilteredCountOnInitialySelected(List<Tbl_Sch_0_0_Block_7> list, int sss, bool isInitialySelected = true)
        {
            return list.Count(entry => entry.SSS == sss && entry.isInitialySelected == isInitialySelected && entry.SubstitutionCount == 0 && entry.status != "CASUALTY");
        }

        #endregion


        private void FillBlock11(Body body, Tbl_Sch_0_0_FieldOperation? block11)
        {
            if (block11 == null)
                return;

            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[11]"));

            if (table == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // 1. Enumerator Name
            ReplaceCellText(GetLastCell(rows[2]), block11.enumerator_name ?? "");

            // 2.1 Date of start of field work
            ReplaceCellText(GetLastCell(rows[3]), block11.field_work_start_date?.ToString("dd-MM-yyyy") ?? "");

            // 2.2 Date of end of field work
            ReplaceCellText(GetLastCell(rows[4]), block11.field_work_end_date?.ToString("dd-MM-yyyy") ?? "");

            // 3. Total time taken (hours)
            ReplaceCellText(GetLastCell(rows[5]), block11.time_taken?.ToString() ?? "");

            // 4. Whether remark entered by JSO / SE / Supervisor
            ReplaceCellText(GetLastCell(rows[6]), !string.IsNullOrWhiteSpace(block_0_1?.SupervisorRemarks) ? "Yes" : "No");

            // 5.1 (i) In Block 9 / 10
            ReplaceCellText(GetLastCell(rows[7]), !string.IsNullOrWhiteSpace(block_0_1?.EnumeratorRemarks) || !string.IsNullOrWhiteSpace(block_0_1?.SupervisorRemarks)
                    ? "Yes"
                    : "No"
            );

            // 5.2 (ii) Elsewhere in the schedule
            ReplaceCellText(
                GetLastCell(rows[8]), /*string.IsNullOrWhiteSpace(block_0_1?.EnumeratorRemarks) && string.IsNullOrWhiteSpace(block_0_1?.SupervisorRemarks)*/ !string.IsNullOrEmpty(block_0_1?.remarks_block_2_2) || !string.IsNullOrEmpty(block_0_1?.remarks_block_3) ||
                                    !string.IsNullOrEmpty(block_0_1?.remarks_block_4) || !string.IsNullOrEmpty(block_0_1?.remarks_block_5) ||
                                    !string.IsNullOrEmpty(block_0_1?.remarks_block_6) || !string.IsNullOrEmpty(block_0_1?.remarks_block_7) ||
                                    !string.IsNullOrEmpty(block_0_1?.remarks_block_7_selection)
                    ? "Yes"
                    : "No"
            );

            // 6. Remarks
            ReplaceCellText(GetLastCell(rows[9]), block_0_1?.remarks_block_11 ?? ""
            );
        }


        public async Task<byte[]> FillDocument26(Stream sch26TemplateStream)
        {
            try
            {
                using var ms = new MemoryStream();
                await sch26TemplateStream.CopyToAsync(ms);
                ms.Position = 0;
                //var commonQueries = new CommonQueries();
                //var dbQueries = new DBQueries();
                var data = await _dbQueries.GetBlock7Data(SessionStorage.SelectedFSUId);
                List<Tbl_Sch_0_0_Block_7> selected = data.Where(k => k.isSelected == true).OrderBy(k => k.SSS).ThenBy(k => k.SSS_household_id).ToList();
                using (var doc = WordprocessingDocument.Open(ms, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;

                    var templateElements = body.Elements().ToList();

                    var templateCloneList = templateElements.Select(e => e.CloneNode(true)).ToList();

                    body.RemoveAllChildren();

                    foreach (var item in selected)
                    {
                        var currentBlock = templateCloneList.Select(e => e.CloneNode(true)).ToList();
                        var tempBody = new Body(currentBlock);
                        blockhis_1 = await _dbQueries.Fetch_SCH_HIS_Block1(item.Block_7_3 ?? 0);
                        blockhis_3 = await _dbQueries.Fetch_SCH_HIS_Block3(item.Block_7_3 ?? 0);
                        blockhis_4 = await _dbQueries.Fetch_SCH_HIS_Block4(item.Block_7_3 ?? 0);
                        blockhis_4Q5 = await _dbQueries.PrintFetch_SCH_HIS_Block4_NICList(item.Block_7_3 ?? 0);
                        blockhis_5 = await _dbQueries.Fetch_SCH_HIS_Block5(item.Block_7_3 ?? 0);
                        blockhis_6 = await _dbQueries.Fetch_SCH_HIS_Block6(item.Block_7_3 ?? 0);
                        blockhis_7a_1 = await _dbQueries.PrintFetch_SCH_HIS_Block7A_CodeList(item.Block_7_3 ?? 0);
                        blockHis_7a = await _dbQueries.Fetch_SCH_HIS_Block7A(item.Block_7_3 ?? 0);
                        blockHis_7b = await _dbQueries.Fetch_SCH_HIS_Block7B(item.Block_7_3 ?? 0);
                        blockHis_7c = await _dbQueries.Fetch_SCH_HIS_Block7C(item.Block_7_3 ?? 0);
                        blockhis_7c_Nic = await _dbQueries.PrintFetch_SCH_HIS_Block7_NICList(item.Block_7_3 ?? 0);
                        blockhis_7c_Q10 = await _dbQueries.PrintFetch_SCH_HIS_Block7_Q10_List(item.Block_7_3 ?? 0);
                        //blockhis_7d = await _dbQueries.Fetch_SCH_HIS_Block7D();
                        var fetchCropTask = _dbQueries.Fetch_SCH_HIS_Block7A_CodeList();
                        var fetchNICTask = _dbQueries.Fetch_SCH_HIS_Block7_NICList();
                        var fetchBlock4Task = _dbQueries.Fetch_SCH_HIS_Block4(item.Block_7_3 ?? 0);
                        var fetchBlock7DTask = _dbQueries.PrintFetch_SCH_HIS_Block7D(item.Block_7_3 ?? 0);
                        await Task.WhenAll(fetchCropTask, fetchNICTask, fetchBlock4Task, fetchBlock7DTask);
                        blockhis_7d = fetchBlock7DTask.Result ?? new List<Tbl_Block_7d>();
                        blockhis_8 = await _dbQueries.Fetch_SCH_HIS_Block8(item.Block_7_3 ?? 0);
                        blockhis_8_Q6 = await _dbQueries.Fetch_SCH_HIS_Block8_6(item.Block_7_3 ?? 0);
                        blockhis_9a = await _dbQueries.Fetch_SCH_HIS_Block9a(item.Block_7_3 ?? 0);
                        blockhis_9b = await _dbQueries.Fetch_SCH_HIS_Block9B(item.Block_7_3 ?? 0);
                        blockhis_10 = await _dbQueries.PrintFetch_SCH_HIS_Block10(item.Block_7_3 ?? 0);
                        block_11a = await _dbQueries.Fetch_SCH_HIS_Block11(item.Block_7_3 ?? 0);
                        block_11b_list = await _dbQueries.Fetch_SCH_HIS_Block11b(item.Block_7_3 ?? 0);
                        blockHis_A = await _dbQueries.PrintFetchSingleForFsuAndHhdAsyncA(item.Block_7_3 ?? 0);
                         blockHis_B = await _dbQueries.PrintFetchSingleForFsuAndHhdAsyncB(item.Block_7_3 ?? 0);

                        //using var ms = new MemoryStream(File.ReadAllBytes(templatePath))PrintFetchSingleForFsuAndHhdAsyncB
                        //using (var doc = WordprocessingDocument.Open(ms, true))
                        //{
                        //    var body = doc.MainDocumentPart.Document.Body;

                        FillBlockHis1(tempBody, blockhis_1);
                        FillBlockHis3(tempBody, blockhis_3);
                        FillBlock3Remarks(tempBody, blockhis_1?.block_3_remark);
                        FillBlockHis4_1(tempBody, blockhis_4);
                        FillBlockHis4_2(tempBody, blockhis_4Q5);
                        FillBlockHis4_2_Remarks(tempBody, blockhis_1?.block_4_remark);
                        FillBlockHis4_3(tempBody, blockhis_4);
                        FillBlockHis5(tempBody, blockhis_5, blockhis_3);
                        FillBlock5Remarks(tempBody, blockhis_1?.block_5_remark);
                        FillBlockHis6(tempBody, blockhis_6);
                        FillBlock6Remarks(tempBody, blockhis_1?.block_6_remark);
                        FillBlock7a_1(tempBody, blockhis_7a_1);
                        FillBlock7a_1Remarks(tempBody, blockhis_1?.block_7a_remark);
                        FillBlock7a_2(tempBody, blockHis_7a);
                        FillBlock7a_3459(tempBody, blockHis_7a, blockhis_1?.block_7a_remark);
                        FillBlock7b_76(tempBody, blockHis_7b);
                        FillBlock7b_77(tempBody, blockHis_7b);
                        FillBlock7b_789(tempBody, blockHis_7b, blockhis_1?.block_7b_remark);
                        FillBlock7c_9(tempBody, blockhis_7c_Nic);
                        FillBlock9cRemarks(tempBody, blockhis_1?.block_7c_remark);
                        FillBlock7c_10(tempBody, blockhis_7c_Q10);
                        FillBlock7c_11_9_Remarks(tempBody, blockHis_7c, blockhis_1?.block_7c_remark);
                        FillBlock7d(tempBody, blockhis_7d);
                        FillBlock7dRemarks(tempBody, blockhis_1?.block_7d_remark);
                        FillBlockHis8(tempBody, blockhis_8);
                        FillBlockHis8_1(tempBody, blockhis_8_Q6);
                        FillBlock8_1Remarks(tempBody, blockhis_1?.block_8b_remark);
                        FillBlockHis9A(tempBody, blockhis_9a);
                        FillBlockHis9B(tempBody, blockhis_9b);
                        FillBlock9B_5(tempBody, blockhis_9b);
                        FillBlockHis10(tempBody, blockhis_10);
                        FillBlockHis11a(tempBody, block_11a);
                        //FillBlockHis11b(body, block_11b_list);
                        FillBlockHis11b(tempBody, block_11b_list, blockhis_1?.block_11b_remark);
                        FillBlockHisA(tempBody, blockHis_A);
                        FillBlockHisB(tempBody, blockHis_B);


                        //    // Save explicitly (safe)
                        //    doc.MainDocumentPart.Document.Save();
                        // }
                        foreach (var el in currentBlock)
                        {
                            body.AppendChild(el.CloneNode(true));
                        }

                        if (item != selected.Last())
                        {
                            body.AppendChild(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));
                        }

                    }
                    doc.MainDocumentPart.Document.Save();
                }
                    return ms.ToArray();
            }
            
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating document: {ex.Message}");
                return null;
            }
        }


        private void SafeReplace(List<TableRow> rows, int rowIndex, int cellIndex, string value)
        {
            if (rows.Count > rowIndex)
            {
                var cells = rows[rowIndex].Elements<TableCell>().ToList();
                if (cells.Count > cellIndex)
                    ReplaceCellText(cells[cellIndex], value ?? "");
            }
        }
        private void FillBlockHis1(Body body, Tbl_Block_1 block1)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[1]"));

            if (table == null || block1 == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            SafeReplace(rows, 2, 2, block1.sector_name);             
            SafeReplace(rows, 3, 2, block1.state_name);              
            SafeReplace(rows, 4, 2, block1.district_name);           
            SafeReplace(rows, 5, 2, $"{block1.sub_district_name}, {block1.town_name}");       
            //SafeReplace(rows, 6, 2, block1.town_name);       
            SafeReplace(rows, 6, 2, block1.village_name);            
            SafeReplace(rows, 7, 2, block1.investigator_unit_no);            
            SafeReplace(rows, 8, 2, block1.sample_su_number);        
            SafeReplace(rows, 9, 2, block1.serial_number_sample_fsu);
            SafeReplace(rows, 10, 2, block1.sample_sub_division_number?.ToString());
            SafeReplace(rows, 11, 2, block1.sss_number?.ToString());
            SafeReplace(rows, 12, 2, block1.sample_hhd_number?.ToString());
            SafeReplace(rows, 13, 2, block1.survey_code?.ToString());
            SafeReplace(rows, 14, 2, block1.substitution_reason?.ToString());
            SafeReplace(rows, 15, 2, block1.block_1_remark);
        }
        
        

        private void FillBlockHis3(Body body,List<Tbl_Block_3> list)
        {
            if (list == null || list.Count == 0)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower().Contains("[3] demographic"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[3]"));
            var table = body.Elements<Table>().ElementAtOrDefault(1);


            var rows = table.Elements<TableRow>().ToList();

            // template row = last row (safe)
            TableRow templateRow = rows.Last();

            int serial = 1;

            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.serial_no.ToString());              // s. no.
                ReplaceCellText(cells[1], item.item_2 ?? "");              // name
                ReplaceCellText(cells[2], item.item_3?.ToString() ?? "");  // relation
                ReplaceCellText(cells[3], item.gender?.ToString() ?? "");  // gender
                ReplaceCellText(cells[4], item.age?.ToString() ?? "");     // age
                ReplaceCellText(cells[5], item.item_6?.ToString() ?? "");  // marital
                ReplaceCellText(cells[6], item.item_7?.ToString() ?? "");  // education
                ReplaceCellText(cells[7], item.item_8?.ToString() ?? "");  // 30d primary
                ReplaceCellText(cells[8], item.item_9?.ToString() ?? "");  // 30d secondary
                ReplaceCellText(cells[9], item.item_10?.ToString() ?? ""); // 365d primary
                ReplaceCellText(cells[10], item.item_11?.ToString() ?? "");// 365d secondary
                ReplaceCellText(cells[11], item.item_12?.ToString() ?? "");// beneficiary

                table.AppendChild(row);
                serial++;
            }

            // remove original blank template row
            templateRow.Remove();
        }

        private void FillBlock3Remarks(Body body,string remarks)
        {

            var table = body.Elements<Table>()
                .First(t => t.InnerText.ToLower().Contains("[3]remarks"));

            var row = table.Elements<TableRow>().First();
            var cells = row.Elements<TableCell>().ToList();

            // 2nd column = remarks text
            ReplaceCellText(cells[1], remarks);
        }
        private void FillBlockHis4_1(Body body, Tbl_Block_4 block4)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[4]"));

            if (table == null || block4 == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // Q4.1 – Household size
            SafeReplace(rows, 2, 2, block4.item_1?.ToString());

            // Q4.2 – Social group
            SafeReplace(rows, 3, 2, block4.item_2?.ToString());

            // Q4.3 – Religion
            SafeReplace(rows, 4, 2, block4.item_3?.ToString());

            // Q4.4 – Agricultural activity (multiple select → tick style)
            SafeReplace(rows, 5, 2,
                string.Join(", ",
                    new[]
                    {
                block4.item_4_1 == true ? "1" : null,
                block4.item_4_2 == true ? "2" : null,
                block4.item_4_3 == true ? "3" : null,
                block4.item_4_4 == true ? "4" : null,
                block4.item_4_5 == true ? "5" : null,
                block4.item_4_6 == true ? "6" : null
                    }.Where(x => x != null)
                )
            );
        }

        //private void FillBlockHis4_2(Body body,List<Tbl_Block_4_Q5> activities)
        //{
        //    var table = body.Elements<Table>()
        //        .FirstOrDefault(t => t.InnerText.Contains("[4.2]"));

        //    if (table == null || activities == null)
        //        return;

        //    var rows = table.Elements<TableRow>().ToList();

        //    TableRow templateRow = rows[3]; // first data row
        //    TableRow remarksOrEndRow = rows.Last();

        //    // clear old data rows
        //    for (int i = rows.Count - 2; i >= 3; i--)
        //        rows[i].Remove();

        //    int serial = 1;

        //    foreach (var act in activities)
        //    {
        //        var row = (TableRow)templateRow.CloneNode(true);
        //        var cells = row.Elements<TableCell>().ToList();

        //        ReplaceCellText(cells[0], serial.ToString());
        //        ReplaceCellText(cells[1], act.ActivityName ?? "");
        //        ReplaceCellText(cells[2], act.NicCode?.ToString() ?? "");
        //        ReplaceCellText(cells[3], act.BusinessSeasonal?.ToString() ?? "");
        //        ReplaceCellText(cells[4], act.NumberOfMonths?.ToString() ?? "");

        //        table.InsertBefore(row, remarksOrEndRow);
        //        serial++;
        //    }
        //    ReplaceCellText(
        //       GetLastCell(remarksOrEndRow),
        //       blockhis_1?.block_4_remark ?? ""
        //   );
        //}

        private void FillBlockHis4_2(Body body, List<Tbl_Block_4_Q5> activities)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[4.2] Household Characteristics"));

            if (table == null || activities == null || activities.Count == 0)
                return;

            var rows = table.Elements<TableRow>().ToList();

            TableRow templateRow = rows[2];

            // remove old data rows (keep header + (1)(2)(3)(4))
            for (int i = rows.Count - 1; i > 3; i--)
                rows[i].Remove();

            int serial = 1;

            foreach (var act in activities)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], act.SerialNumber.ToString()!);
                ReplaceCellText(cells[1], act.ActivityName ?? "");
                ReplaceCellText(cells[2], act.NicCode?.ToString() ?? "");
                ReplaceCellText(cells[3], act.BusinessSeasonal?.ToString() ?? "");
                ReplaceCellText(cells[4], act.NumberOfMonths?.ToString() ?? "");

                table.AppendChild(row);
                serial++;
            }
        }
        private void FillBlockHis4_2_Remarks(Body body, string remarks)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[4.2] remarks"));

            if (table == null)
                return;

            var row = table.Elements<TableRow>().Last();
            var cell = row.Elements<TableCell>().Last();

            ReplaceCellText(cell, remarks);
        }


        private void FillBlockHis4_3(Body body, Tbl_Block_4 block4)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[4.3]"));

            if (table == null || block4 == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            SafeReplace(rows, 2, 2, block4.item_6?.ToString());   // Own land
            SafeReplace(rows, 3, 2, block4.item_7?.ToString());   // Total land
            SafeReplace(rows, 4, 2, block4.item_8);               // Use of land
            SafeReplace(rows, 5, 2, block4.item_9?.ToString());   // Economic activity
            SafeReplace(rows, 6, 2, block4.item_11?.ToString());  // Dwelling type
            SafeReplace(rows, 7, 2, block4.item_12?.ToString());  // Carpet area
            SafeReplace(rows, 7, 2, block4.item_13?.ToString());  // Carpet area
            SafeReplace(rows, 7, 2, block4.item_14?.ToString());  // Carpet area
            SafeReplace(rows, 7, 2, block4.item_15?.ToString());  // Carpet area
            SafeReplace(rows, 8, 2, block4.item_16?.ToString());  // Loan outstanding

            // Remarks row (always last)
            ReplaceCellText(
                GetLastCell(rows.Last()),
                blockhis_1?.block_6_remark ?? ""
            );
        }

        private void FillBlockHis5( Body body,List<Tbl_Block_5> list, List<Tbl_Block_3> block3Members)
        {
            if (list == null || list.Count == 0)
                return;

            var table = body.Elements<Table>()
        .First(t => t.InnerText.ToLower()
            .Contains("[5]"));

            var rows = table.Elements<TableRow>().ToList();

            // last row = blank template row (safe)
            TableRow templateRow = rows.Last();
            
            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();
                string memberName =
                         block3Members?.FirstOrDefault(x => x.id == item.fk_block_3)?.item_2 ?? "";

                ReplaceCellText(cells[0], item.serial_number?.ToString() ?? "");
                ReplaceCellText(cells[1], memberName?.ToString() ?? "");
                ReplaceCellText(cells[2], item.item_1?.ToString() ?? "");
                ReplaceCellText(cells[3], item.item_2?.ToString() ?? "");
                ReplaceCellText(cells[4], item.item_3_i?.ToString() ?? "");
                ReplaceCellText(cells[5], item.item_3_ii?.ToString() ?? "");
                ReplaceCellText(cells[6], item.item_4_i?.ToString() ?? "");
                ReplaceCellText(cells[7], item.item_4_ii?.ToString() ?? "");
                ReplaceCellText(cells[8], item.item_5?.ToString() ?? "");
                ReplaceCellText(cells[9], item.item_6?.ToString() ?? "");
                ReplaceCellText(cells[10], item.item_7?.ToString() ?? "");
                ReplaceCellText(cells[11], item.item_8?.ToString() ?? "");
                ReplaceCellText(cells[12], item.item_9?.ToString() ?? "");

                table.AppendChild(row);
            }

            // remove original blank template row
            templateRow.Remove();
        }
        private void FillBlock5Remarks(Body body,string remarks)
        {

            var table = body.Elements<Table>()
                .First(t => t.InnerText.ToLower().Contains("[5]remarks"));

            var row = table.Elements<TableRow>().First();
            var cells = row.Elements<TableCell>().ToList();

            // 2nd column = remarks value
            ReplaceCellText(cells[1], remarks);
        }

        private void FillBlockHis6( Body body,List<Tbl_Block_6> list)
        {
            if (list == null || list.Count == 0)
                return;

            var table = body.Elements<Table>()
                .First(t => t.InnerText.ToLower()
                    .Contains("[6]"));

            var rows = table.Elements<TableRow>().ToList();

            // last row = blank template row
            TableRow templateRow = rows.Last();

            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.serial_number?.ToString() ?? "");
                ReplaceCellText(cells[1], item.item_1?.ToString() ?? "");
                ReplaceCellText(cells[2], item.item_2?.ToString() ?? "");
                ReplaceCellText(cells[3], item.item_3?.ToString() ?? "");
                ReplaceCellText(cells[4], item.item_4?.ToString() ?? "");

                table.AppendChild(row);
            }

            // remove original blank template row
            templateRow.Remove();
        }
        private void FillBlock6Remarks(Body body,string remarks)
        {

            var table = body.Elements<Table>()
                .First(t => t.InnerText.ToLower().Contains("[6]remarks"));

            var row = table.Elements<TableRow>().First();
            var cells = row.Elements<TableCell>().ToList();

            // 2nd column = remarks text
            ReplaceCellText(cells[1], remarks);
        }

        private void FillBlockHis8(Body body,Tbl_Block_8 block8)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[8]"));

            if (table == null || block8 == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // Assumption: Code/Entry column index = 3
            SafeReplace(rows, 2, 3, block8.item_1?.ToString()); // Q8.1
            SafeReplace(rows, 3, 3, block8.item_2?.ToString()); // Q8.2
            SafeReplace(rows, 4, 3, block8.item_3?.ToString()); // Q8.3
            SafeReplace(rows, 5, 3, block8.item_4?.ToString()); // Q8.4
            SafeReplace(rows, 6, 3, block8.item_5?.ToString()); // Q8.5
            var remarksRow = rows.Last();
            ReplaceCellText(
                GetLastCell(remarksRow),
                blockhis_1?.block_8a_remark ?? ""
            );
        }

        private void FillBlockHis8_1( Body body,List<Tbl_Block_8_Q6> list)
        {
            if (list == null || list.Count == 0)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[8.Q8.6]"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[8.Q8.6]"));
            var table = body.Elements<Table>().ElementAtOrDefault(25);

            var rows = table.Elements<TableRow>().ToList();

            // last row = blank template row (safe for Word merge)
            TableRow templateRow = rows.Last();

            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.serial_number?.ToString() ?? "");
                ReplaceCellText(cells[1], item.item_2?.ToString() ?? "");
                ReplaceCellText(cells[2], item.item_3?.ToString() ?? "");
                ReplaceCellText(cells[3], item.item_4?.ToString() ?? "");
                ReplaceCellText(cells[4], item.item_5?.ToString() ?? "");

                table.AppendChild(row);
            }

            // remove original blank template row
            templateRow.Remove();
        }

        private void FillBlock8_1Remarks( Body body, string remarks)
        {
            var table = body.Elements<Table>()
                .First(t => t.InnerText.ToLower().Contains("[8.1]remarks"));

            var row = table.Elements<TableRow>().First();
            var cells = row.Elements<TableCell>().ToList();

            // 2nd column = remarks text
            ReplaceCellText(cells[1], remarks);
        }


        //private void FillBlockHis8_1(Body body,List<Tbl_Block_8_Q6> list)
        //{
        //    var table = body.Elements<Table>()
        //        .FirstOrDefault(t => t.InnerText.ToLower().Contains("[8.1]"));

        //    if (table == null || list == null)
        //        return;

        //    var rows = table.Elements<TableRow>().ToList();

        //    // header rows end at index 1, data starts at 2
        //    TableRow templateRow = rows[2];
        //    TableRow remarksRow = rows.Last();

        //    // clear old rows (keep header + remarks)
        //    for (int i = rows.Count - 2; i >= 2; i--)
        //        rows[i].Remove();

        //    int serial = 1;

        //    foreach (var item in list)
        //    {
        //        var row = (TableRow)templateRow.CloneNode(true);
        //        var cells = row.Elements<TableCell>().ToList();
        //        int c = cells.Count;

        //        if (c > 0) ReplaceCellText(cells[0], (item.serial_number ?? serial).ToString());
        //        if (c > 1) ReplaceCellText(cells[1], ""); // Scheme name (if lookup later)
        //        if (c > 2) ReplaceCellText(cells[2], item.item_2?.ToString() ?? "");
        //        if (c > 3) ReplaceCellText(cells[3], item.item_3?.ToString() ?? "");
        //        if (c > 4) ReplaceCellText(cells[4], item.item_4?.ToString() ?? "");

        //        table.InsertBefore(row, remarksRow);
        //        serial++;
        //    }

        //    // Remarks (if present)
        //    ReplaceCellText(
        //        GetLastCell(remarksRow),
        //        blockhis_1?.block_8b_remark ?? ""
        //    );
        //}
        private void FillBlockHis9A(Body body, Tbl_Block_9a block9a)
        {
            //if (block9a == null || block9a == 0)
            //    return;
            //var table = body.Elements<Table>()
            //    .FirstOrDefault(t => t.InnerText.Contains("[9A_Q9.1]"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[9A_Q9.1]"));
            var table = body.Elements<Table>().ElementAtOrDefault(27);

            if (table == null || block9a == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // ---------- Q9.1 : Interest income (Amount column = index 2) ----------
            SafeReplace(rows, 2, 2, block9a.item_1_1?.ToString()); // saving/deposit
            SafeReplace(rows, 3, 2, block9a.item_1_2?.ToString()); // certificates/bonds
            SafeReplace(rows, 4, 2, block9a.item_1_3?.ToString()); // provident fund
            SafeReplace(rows, 5, 2, block9a.item_1_4?.ToString()); // loans
            SafeReplace(rows, 6, 2, block9a.item_1_5?.ToString()); // other
            SafeReplace(rows, 7, 2, block9a.item_1_6?.ToString()); // total interest

            // ---------- Q9.2 – Q9.4 (second table in same block) ----------
            var table2 = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[9A]"));

            if (table2 == null)
                return;

            var rows2 = table2.Elements<TableRow>().ToList();

            // Amount column index = 3
            SafeReplace(rows2, 1, 3, block9a.item_2?.ToString()); // Q9.2 Dividend
            SafeReplace(rows2, 2, 3, block9a.item_3?.ToString()); // Q9.3 Annuities
            SafeReplace(rows2, 3, 3, block9a.item_4?.ToString()); // Q9.4 Other receipts

            // ---------- Remarks (last row, last cell) ----------
            var remarksRow = rows2.Last();
            ReplaceCellText(
                GetLastCell(remarksRow),
                blockhis_1?.block_9a_remark ?? ""
            );
        }

        private void FillBlockHis9B(Body body, Tbl_Block_9b block9b)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[9B]"));

            if (table == null || block9b == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // Amount column index = 3

            // Q9.6 – Leasing out land / natural resources
            SafeReplace(rows, 1, 3, block9b.item_6?.ToString());

            // Q9.7 – Licensing intellectual property
            SafeReplace(rows, 2, 3, block9b.item_7?.ToString());

            // ---------- Remarks ----------
            var remarksRow = rows.Last();
            ReplaceCellText(
                GetLastCell(remarksRow),
               blockhis_1?.block_9b_remark ?? ""
            );
        }

        private  void FillBlock9B_5(Body body,Tbl_Block_9b data)
        {
            if (data == null)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[9B_9.5] Income from "));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[9B_Q9.5]"));
            var table = body.Elements<Table>().ElementAtOrDefault(29);

            var rows = table.Elements<TableRow>().ToList();

            // ---- Row 1 : Dwelling ----
            SafeReplace(rows, 3, 2, data.item_5_1_3?.ToString());
            SafeReplace(rows, 3, 3, data.item_5_1_4?.ToString());
            SafeReplace(rows, 3, 4, data.item_5_1_5?.ToString());

            // ---- Row 2 : Building (excluding dwelling) ----
            SafeReplace(rows, 4, 2, data.item_5_2_3?.ToString());
            SafeReplace(rows, 4, 3, data.item_5_2_4?.ToString());
            SafeReplace(rows, 4, 4, data.item_5_2_5?.ToString());

            // ---- Row 3 : Hired-out machinery & equipment ----
            SafeReplace(rows, 5, 2, data.item_5_3_3?.ToString());
            SafeReplace(rows, 5, 3, data.item_5_3_4?.ToString());
            SafeReplace(rows, 5, 4, data.item_5_3_5?.ToString());
        }


        private void FillBlockHis10(Body body, Tbl_Block_10 block10)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[10]"));

            if (table == null || block10 == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // Amount column index = 3
            // Food items
            SafeReplace(rows, 3, 3, block10.item_1?.ToString());   // Q10.1
            SafeReplace(rows, 4, 3, block10.item_2?.ToString());   // Q10.2
            SafeReplace(rows, 5, 3, block10.item_3?.ToString());   // Q10.3
            SafeReplace(rows, 6, 3, block10.item_4?.ToString());   // Q10.4
            SafeReplace(rows, 7, 3, block10.item_5?.ToString());   // Q10.5
            SafeReplace(rows, 8, 3, "");                           // Q10.91

            // row 10 = section header (skip)

            // Non-Food Consumable
            SafeReplace(rows, 10, 3, block10.item_6?.ToString());  // Q10.6
            SafeReplace(rows, 11, 3, block10.item_7?.ToString());  // Q10.7
            SafeReplace(rows, 12, 3, block10.item_8?.ToString());  // Q10.8
            SafeReplace(rows, 13, 3, block10.item_9?.ToString());  // Q10.9
            SafeReplace(rows, 14, 3, block10.item_10?.ToString()); // Q10.10
            SafeReplace(rows, 15, 3, block10.item_11?.ToString()); // Q10.11
            SafeReplace(rows, 16, 3, block10.item_12?.ToString()); // Q10.12
            SafeReplace(rows, 17, 3, block10.item_13?.ToString()); // Q10.13
            SafeReplace(rows, 18, 3, block10.item_14?.ToString()); // Q10.14
            SafeReplace(rows, 19, 3, "");                           // Q10.92

            // row 21 = section header (skip)

            // Durable goods
            SafeReplace(rows, 21, 3, block10.item_15?.ToString()); // Q10.15
            SafeReplace(rows, 22, 3, block10.item_16?.ToString()); // Q10.16
            SafeReplace(rows, 23, 3, block10.item_17?.ToString()); // Q10.17
            SafeReplace(rows, 24, 3, block10.item_18?.ToString()); // Q10.18
            SafeReplace(rows, 25, 3, block10.item_19?.ToString()); // Q10.19
            SafeReplace(rows, 26, 3, block10.item_20?.ToString()); // Q10.20
            SafeReplace(rows, 27, 3, block10.item_21?.ToString()); // Q10.21
            SafeReplace(rows, 28, 3, block10.item_22?.ToString()); // Q10.22
            SafeReplace(rows, 28, 3, block10.item_23?.ToString()); // Q10.23

            // ✅ Remarks (last row, last cell)
            var remarksRow = rows.Last();
            ReplaceCellText(
                GetLastCell(remarksRow),
                 blockhis_1?.block_10_remark ?? ""
            );
        }

        private void FillBlockHis11a(Body body, Tbl_Block_11a block11a)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[11a]"));

            if (table == null || block11a == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // Entry column index = 2
            SafeReplace(rows, 2, 2, block11a.item_1?.ToString()); // Q11.1
            SafeReplace(rows, 3, 2, block11a.item_2?.ToString()); // Q11.2
            SafeReplace(rows, 4, 2, block11a.item_3?.ToString()); // Q11.3
            SafeReplace(rows, 5, 2, block11a.item_4?.ToString()); // Q11.4
            SafeReplace(rows, 6, 2, block11a.item_5?.ToString()); // Q11.5
            SafeReplace(rows, 7, 2, block11a.item_6?.ToString()); // Q11.6

            // ✅ Remarks (last row, last cell)
            var remarksRow = rows.Last();
            ReplaceCellText(
                GetLastCell(remarksRow),
                blockhis_1?.block_11a_remark ?? ""
            );
        }
        //private void FillBlockHis11b(Body body,List<Tbl_Block_11b> list)
        //{
        //    var table = body.Elements<Table>()
        //        .FirstOrDefault(t => t.InnerText.Contains("[11b]"));

        //    if (table == null || list == null)
        //        return;

        //    var rows = table.Elements<TableRow>().ToList();

        //    // header rows end at index 1, data template at index 2
        //    TableRow templateRow = rows[3];
        //    TableRow remarksRow = rows.Last();

        //    // remove old data rows (keep header + remarks)
        //    for (int i = rows.Count - 2; i >= 3; i--)
        //        rows[i].Remove();

        //    foreach (var item in list)
        //    {
        //        var row = (TableRow)templateRow.CloneNode(true);
        //        var cells = row.Elements<TableCell>().ToList();
        //        int c = cells.Count;

        //        if (c > 0) ReplaceCellText(cells[0], item.item_1?.ToString() ?? "");
        //        if (c > 1) ReplaceCellText(cells[1], item.item_2 ?? "");
        //        if (c > 2) ReplaceCellText(cells[2], item.item_3?.ToString() ?? "");
        //        if (c > 3) ReplaceCellText(cells[3], item.item_4?.ToString() ?? "");

        //        table.InsertBefore(row, remarksRow);
        //    }

        //    // ✅ Remarks
        //    ReplaceCellText(
        //        GetLastCell(remarksRow),
        //        blockhis_1?.block_11b_remark ?? ""
        //    );
        //}

        private void FillBlockHis11b(Body body,List<Tbl_Block_11b> dataList,string remarksFromBlock1)
        {
            if (body == null || dataList == null)
                return;

            // ---------------- Main 11b Table ----------------
            var table = body.Elements<Table>()
               .FirstOrDefault(t => t.InnerText.Contains("[11b]"));

            if (table == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // Data starts after header rows
            int startRowIndex = 2;

            for (int i = 0; i < dataList.Count; i++)
            {
                int rowIndex = startRowIndex + i;
                if (rowIndex >= rows.Count)
                    break;

                var data = dataList[i];

                SafeReplace(rows, rowIndex, 0, data.item_1?.ToString()); // S. no
                SafeReplace(rows, rowIndex, 1, data.item_2);             // Name
                SafeReplace(rows, rowIndex, 2, data.item_3?.ToString()); // Yes / No
                SafeReplace(rows, rowIndex, 3, data.item_4?.ToString()); // Amount
            }

            // ---------------- 11b Remarks (from Block-1) ----------------
           
                var remarksTable = body.Elements<Table>()
                    .FirstOrDefault(t => t.InnerText.Contains("11b")
                                      && t.InnerText.ToLower().Contains("remarks"));

                if (remarksTable != null)
                {
                    var remarkRows = remarksTable.Elements<TableRow>().ToList();

                    // usually: [ Remarks | <value> ]
                    SafeReplace(remarkRows, 0, 1, remarksFromBlock1);
                }
           
        }


        private void FillBlockHisA(Body body, Tbl_Block_A blockA)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("Block A"));

            if (table == null || blockA == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // Entry column index = 2
            SafeReplace(rows, 2, 2, blockA.item_1?.ToString()); // A1
            SafeReplace(rows, 3, 2, blockA.item_2?.ToString()); // A2
            SafeReplace(rows, 4, 2, blockA.item_3?.ToString()); // A3
            SafeReplace(rows, 5, 2, blockA.item_4?.ToString()); // A4
            SafeReplace(rows, 6, 2, blockA.item_5?.ToString()); // A5
            SafeReplace(rows, 7, 2, blockA.item_6?.ToString()); // A6
            SafeReplace(rows, 8, 2, blockA.item_7?.ToString()); // A7
            SafeReplace(rows, 9, 2, blockA.item_8?.ToString()); // A8 (Total)

            // ✅ Remarks (A9)
            var remarksRow = rows.Last();
            ReplaceCellText(
                GetLastCell(remarksRow),
                blockhis_1?.block_a_remark ?? ""
            );
        }
        private void FillBlockHisB(Body body, Tbl_Block_B blockB)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("Block B"));

            if (table == null || blockB == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // Entry column index = 2
            SafeReplace(rows, 2, 2, blockB.item_1?.ToString()); // B1
            SafeReplace(rows, 3, 2, blockB.item_2?.ToString()); // B2
            SafeReplace(rows, 4, 2, blockB.item_3?.ToString()); // B3
            SafeReplace(rows, 5, 2, blockB.item_4?.ToString()); // B4 (Total)

            // ✅ Remarks (B5)
            var remarksRow = rows.Last();
            ReplaceCellText(
                GetLastCell(remarksRow),
                blockhis_1?.block_b_remark ?? ""
            );
        }

        private void FillBlock7a_1( Body body,List<Tbl_Block_7a_1> list)
        {
            if (list == null || list.Count == 0)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7a_Q7.1] income from self-employment"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[7a_Q7.1]"));
            var table = body.Elements<Table>().ElementAtOrDefault(11);

            var rows = table.Elements<TableRow>().ToList();
            var templateRow = rows.FirstOrDefault(r =>
                r.Elements<TableCell>()
                 .All(c => string.IsNullOrWhiteSpace(c.InnerText)));

            if (templateRow == null)
                return;
            var totalRow = rows.FirstOrDefault(r =>
                r.Elements<TableCell>()
                 .FirstOrDefault()?.InnerText.Trim()
                 .Equals("total", StringComparison.OrdinalIgnoreCase) == true);

            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                if (cells.Count < 9)
                    continue;

                ReplaceCellText(cells[0], item.serial_number.ToString());
                ReplaceCellText(cells[1], item.code?.ToString() ?? "");
                ReplaceCellText(cells[2], item.unit?.ToString() ?? "");
                ReplaceCellText(cells[3], item.item_4?.ToString() ?? "");
                ReplaceCellText(cells[4], item.item_5?.ToString() ?? "");
                ReplaceCellText(cells[5], item.item_6?.ToString() ?? "");
                ReplaceCellText(cells[6], item.item_7?.ToString() ?? "");
                ReplaceCellText(cells[7], item.item_8?.ToString() ?? "");
                ReplaceCellText(cells[8], item.item_9?.ToString() ?? "");

                // 🔹 insert BEFORE total row if exists, else append
                if (totalRow != null)
                    table.InsertBefore(row, totalRow);
                else
                    table.AppendChild(row);
            }

            // 🔹 remove original blank template row
            templateRow.Remove();
        }
        private void FillBlock7a_1Remarks(Body body,string remarks)
        {
            var table = body.Elements<Table>()
                .First(t => t.InnerText.ToLower()
                    .Contains("[7a_1]remarks"));

            var row = table.Elements<TableRow>().First();
            var cells = row.Elements<TableCell>().ToList();

            if (cells.Count > 1)
                ReplaceCellText(cells[1], remarks);
        }

        private void FillBlock7a_2(Body body,Tbl_Block_7a data)
        {
            if (data == null)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7a_Q7.2] list of households"));

            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[[7a_Q7.2] list of households"));
            var table = body.Elements<Table>().ElementAtOrDefault(13);
            var rows = table.Elements<TableRow>().ToList();

            SafeReplace(rows, 2, 2, data.item_2_1?.ToString());   // 01
            SafeReplace(rows, 3, 2, data.item_2_2?.ToString());   // 02
            SafeReplace(rows, 4, 2, data.item_2_3?.ToString());   // 03
            SafeReplace(rows, 5, 2, data.item_2_4?.ToString());   // 04
            SafeReplace(rows, 6, 2, data.item_2_5?.ToString());   // 05
            SafeReplace(rows, 7, 2, data.item_2_6?.ToString());   // 06
            SafeReplace(rows, 8, 2, data.item_2_7?.ToString());   // 07
            SafeReplace(rows, 9, 2, data.item_2_8?.ToString());   // 08
            SafeReplace(rows, 10, 2, data.item_2_9?.ToString());   // 09
            SafeReplace(rows, 11, 2, data.item_2_10?.ToString());  // 10
            SafeReplace(rows, 12, 2, data.item_2_11?.ToString());  // 11
            SafeReplace(rows, 13, 2, data.item_2_12?.ToString());  // 12
            SafeReplace(rows, 14, 2, data.item_2_13?.ToString());  // 13 (total)
        }
        private  void FillBlock7a_3459(Body body,Tbl_Block_7a data,string remarksFromBlock1)
        {
            if (data == null)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7a_Q7.3Q7.4Q7.5Q7.9]"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[7a_Q7.3Q7.4Q7.5Q7.9]"));
            var table = body.Elements<Table>().ElementAtOrDefault(14);

            var rows = table.Elements<TableRow>().ToList();
            SafeReplace(rows, 1, 2, data.item_3?.ToString());
            SafeReplace(rows, 2, 2, data.item_4?.ToString());
            SafeReplace(rows, 3, 2, data.item_5?.ToString());
            SafeReplace(rows, 4, 2, data.item_6?.ToString());
            SafeReplace(rows, rows.Count - 1, 1, remarksFromBlock1);
            
        }

        private  void FillBlock7b_76(Body body, Tbl_Block_7b data)
        {
            if (data == null) return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7b_Q7.6]"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[7b_Q7.6]"));
            var table = body.Elements<Table>().ElementAtOrDefault(15);

            var rows = table.Elements<TableRow>().ToList();

            SafeReplace(rows, 2, 2, data.item_6_1?.ToString());
            SafeReplace(rows, 3, 2, data.item_6_2?.ToString());
            SafeReplace(rows, 4, 2, data.item_6_3?.ToString());
            SafeReplace(rows, 5, 2, data.item_6_4?.ToString());
            SafeReplace(rows, 6, 2, data.item_6_5?.ToString());
            SafeReplace(rows, 7, 2, data.item_6_6?.ToString());
            SafeReplace(rows, 8, 2, data.item_6_7?.ToString());
            SafeReplace(rows, 9, 2, data.item_6_8?.ToString());
            SafeReplace(rows, 10, 2, data.item_6_9?.ToString());
            SafeReplace(rows, 11, 2, data.item_6_10?.ToString());
            SafeReplace(rows, 12, 2, data.item_6_11?.ToString());
            SafeReplace(rows, 13, 2, data.item_6_12?.ToString());
        }
        private  void FillBlock7b_77(Body body,Tbl_Block_7b data)
        {
            if (data == null) return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7b.Q7.7]"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[7b.Q7.7]"));
            var table = body.Elements<Table>().ElementAtOrDefault(16);

            var rows = table.Elements<TableRow>().ToList();

            SafeReplace(rows, 2, 2, data.item_7_1?.ToString());
            SafeReplace(rows, 3, 2, data.item_7_2?.ToString());
            SafeReplace(rows, 4, 2, data.item_7_3?.ToString());
            SafeReplace(rows, 5, 2, data.item_7_4?.ToString());
            SafeReplace(rows, 6, 2, data.item_7_5?.ToString());
            SafeReplace(rows, 7, 2, data.item_7_6?.ToString());
            SafeReplace(rows, 8, 2, data.item_7_7?.ToString());
            SafeReplace(rows, 9, 2, data.item_7_8?.ToString());
            SafeReplace(rows, 10, 2, data.item_7_9?.ToString());
            SafeReplace(rows, 11, 2, data.item_7_10?.ToString());
            SafeReplace(rows, 12, 2, data.item_7_11?.ToString());
            SafeReplace(rows, 13, 2, data.item_7_12?.ToString());
            SafeReplace(rows, 14, 2, data.item_7_13?.ToString()); // total
        }
        private  void FillBlock7b_789(Body body,Tbl_Block_7b data,string remarksFromBlock1)
        {
            if (data == null)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7b.Q7.8Q7.9]"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[7b.Q7.8Q7.9]"));
            var table = body.Elements<Table>().ElementAtOrDefault(17);

            var rows = table.Elements<TableRow>().ToList();


            // Q7.8
            SafeReplace(rows, 1, 2, data.item_8?.ToString());

            // Q7b.9 Gross Profit / Loss
            SafeReplace(rows, 2, 2, data.item_9?.ToString());

            // Remarks (from Block-1)
            if (!string.IsNullOrWhiteSpace(remarksFromBlock1))
            {
                SafeReplace(rows, rows.Count - 1, 1, remarksFromBlock1);
            }
        }

        private  void FillBlock7c_11_9_Remarks( Body body,Tbl_Block_7c data,string remarksFromBlock1)
        {
            if (data == null)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7c.11]"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[7c.Q7.11Q7c.9]"));
            var table = body.Elements<Table>().ElementAtOrDefault(21);

            var rows = table.Elements<TableRow>().ToList();

            SafeReplace(rows, 1, 2, data.item_7_11?.ToString());
            SafeReplace(rows, 2, 2, data.item_7_13?.ToString());
            SafeReplace(rows, 3, 2, data.item_7_12?.ToString());

          
            if (!string.IsNullOrWhiteSpace(remarksFromBlock1))
            {
                SafeReplace(rows, rows.Count - 1, 1, remarksFromBlock1);
            }
        }
        private  void FillBlock7c_9(Body body,List<Tbl_Block_7c_NIC> list)
        {
            if (list == null || list.Count == 0)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7c_Q7.9]"));

            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[7c_Q7.9]"));
            var table = body.Elements<Table>().ElementAtOrDefault(18);
            var rows = table.Elements<TableRow>().ToList();

            // last row = blank template row
            TableRow templateRow = rows.Last();

            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.SerialNumber?.ToString() ?? "");
                ReplaceCellText(cells[1], item.ActivityName ?? "");
                ReplaceCellText(cells[2], item.NicCode?.ToString() ?? "");
                ReplaceCellText(cells[3], item.GrossValue?.ToString() ?? "");

                table.AppendChild(row);
            }

            // remove original template
            templateRow.Remove();
        }

        private void FillBlock9cRemarks(Body body, string remarksFromBlock1)
        {
            var remarksTable = body.Elements<Table>()
                .First(t => t.InnerText.ToLower()
                    .Contains("[7_c]remarks"));

            var row = remarksTable.Elements<TableRow>().First();
            var cells = row.Elements<TableCell>().ToList();

            // second column = remarks value
            ReplaceCellText(cells[1], remarksFromBlock1);
        }

        private  void FillBlock7c_10(Body body,List<Tbl_Block_7c_Q10> list)
        {
            if (list == null || list.Count == 0)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7c.Q7.10]"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[7c.Q7.10]"));
            var table = body.Elements<Table>().ElementAtOrDefault(20);

            var rows = table.Elements<TableRow>().ToList();

            TableRow templateRow = rows.Last();

            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.serial_number.ToString());
                ReplaceCellText(cells[1], item.item_10_1?.ToString() ?? "");
                ReplaceCellText(cells[2], item.item_10_2?.ToString() ?? "");
                ReplaceCellText(cells[3], item.item_10_3?.ToString() ?? "");
                ReplaceCellText(cells[4], item.item_10_4?.ToString() ?? "");
                ReplaceCellText(cells[5], item.item_10_5?.ToString() ?? "");
                ReplaceCellText(cells[6], item.item_10_6?.ToString() ?? "");
                ReplaceCellText(cells[7], item.item_10_7?.ToString() ?? "");
                ReplaceCellText(cells[8], item.item_10_8?.ToString() ?? "");
                ReplaceCellText(cells[9], item.item_10_9?.ToString() ?? "");
                ReplaceCellText(cells[10], item.item_10_10?.ToString() ?? "");
                ReplaceCellText(cells[11], item.item_10_11?.ToString() ?? "");

                table.AppendChild(row);
            }

            templateRow.Remove();
        }

        private void FillBlock7d(Body body,List<Tbl_Block_7d> list)
        {
            if (list == null || list.Count == 0)
                return;

            //var table = body.Elements<Table>()
            //    .First(t => t.InnerText.ToLower()
            //        .Contains("[7d_Q7.12] mode of operation"));
            var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[7d_Q7.12] mode of operation"));
            var table = body.Elements<Table>().ElementAtOrDefault(22);

            var rows = table.Elements<TableRow>().ToList();

            // Last row assumed as template (blank)
            TableRow templateRow = rows.Last();

            foreach (var item in list)
            {
                var row = new TableRow();

                // (1) S. no. of activity
                row.Append(CreateTextCell(item.display_serial_number?.ToString() ?? ""));

                // (2) Description of activity
                row.Append(CreateTextCell(item.item_2 ?? ""));

                // (3) Mode of operation
                row.Append(CreateTextCell(item.item_3?.ToString() ?? ""));

                // (4) % shareholding
                row.Append(CreateTextCell(item.item_4?.ToString() ?? ""));

                table.AppendChild(row);
            }

            // Remove template row
            templateRow.Remove();
        }
        private void FillBlock7dRemarks( Body body,string remarksFromBlock1)
        {
            var remarksTable = body.Elements<Table>()
                .First(t => t.InnerText.ToLower()
                    .Contains("[7d]remarks"));

            var row = remarksTable.Elements<TableRow>().First();
            var cells = row.Elements<TableCell>().ToList();

            // second column = remarks value
            ReplaceCellText(cells[1], remarksFromBlock1);
        }






        string GetDocumentsPath()
        {
#if WINDOWS
    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif ANDROID
            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
#else
            return FileSystem.AppDataDirectory;
#endif
        }
        private TableCell CreateTextCell(string text)
        {
            return new TableCell(
                new Paragraph(
                    new Run(
                        new Text(text ?? "")
                    )
                )
            );
        }

    }
}
