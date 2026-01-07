

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

        public Query SCH_0_0_Quaries = new();
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
        List<Tbl_Block_5>? blockhis_5 = new();
        List<Tbl_Block_6>? blockhis_6 = new();
        Tbl_Block_7a? blockhis_7a = new();
        Tbl_Block_10? blockhis_10 = new();
    

        //int hhdId = 0;
        //public PrintHelper(ScheduleZeroDatabase _scheduleZeroDatabase, ScheduleTenOFourDatabase sch104Db)
        //{
        //    _ScheduleZeroDatabase = _scheduleZeroDatabase;
        //    _sch104Db = sch104Db;
        //}
        public async Task<int> GenerateReportAsync(int FsuId)
        {
            try
            {
                //hhdId = HhdId;
                string printDir;
                string downloadsDir;
                var fileName = $"{FsuId}.docx";

                //Stream templateStream;
                Stream sch00TemplateStream;
                Stream sch26TemplateStream;


#if WINDOWS
        // Access from Resources/Template folder in Windows
        var templatePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Template", "IncomeSch00Template.docx");
        var template2026Path = Path.Combine(AppContext.BaseDirectory, "Resources", "Template", "TemplateHis2026.docx");
        sch00TemplateStream = File.OpenRead(templatePath);
        sch26TemplateStream = File.OpenRead(template2026Path);
#else

                sch00TemplateStream = await FileSystem.OpenAppPackageFileAsync("Resources/Template/IncomeSch00Template.docx");
                sch26TemplateStream = await FileSystem.OpenAppPackageFileAsync("Resources/Template/TemplateHis2026.docx");
#endif

                var generatedDoc00 = await FillDocument00(sch00TemplateStream);
                var generatedDoc2026 = await FillDocument26(sch26TemplateStream);
                if (generatedDoc00 == null || generatedDoc00.Length == 0)
                    return 0;
                if (generatedDoc2026 == null || generatedDoc2026.Length == 0)
                    return 0;
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
                string targetPath2026 = Path.Combine(printDir, SCH2026fileName);
                await File.WriteAllBytesAsync(targetPath00, generatedDoc00);
                await File.WriteAllBytesAsync(targetPath2026, generatedDoc2026);
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
                var dbQueries = new DBQueries();
                block_0_1 = await dbQueries.FetchBlock1();
                block_2_1 = await dbQueries.FetchSCH0Block2_1Data();
                block_2_2 = await dbQueries.FetchSCH0Block2_2Data();
                block_4 = await dbQueries.GetBlock4();
                block_5 = await dbQueries.FetchSCH0Block5Data();
                //block_7 = await dbQueries.GetBlock7Data();
                block_7 = await dbQueries.Get_SCH0_0_Block_5A_HouseHoldBy_FSUP(SessionStorage.SelectedFSUId);
                block_11 = await dbQueries.FetchBlock2();


                using (var doc = WordprocessingDocument.Open(ms, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;

                    FillBlock0_1(body, block_0_1);
                    FillBlock2_1_And_2_2(body, block_2_1, block_2_2);
                    FillBlock4(body, block_4);
                    FillBlock5(body, block_5);
                    FillBlock7(body, block_7);
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
                SafeReplace(rows1, 3, (block01?.Block_1_2 ?? 0).ToString());
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
            var table10 = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower()
                    .Contains("[10] remarks by field supervisor"));

            if (table10 != null)
            {
                var rows10 = table10.Elements<TableRow>().ToList();
                SafeReplace(rows10, 1, block01?.SupervisorRemarks);
            }

        }//Block0_1...

        private void FillBlock2_1_And_2_2(Body body, List<Tbl_Sch_0_0_Block_2_1> block21, List<Tbl_Sch_0_0_Block_2_2> block22)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[2.1]"));

            if (table == null)
                return;

            var rows = table.Elements<TableRow>().ToList();
            TableRow templateRow = rows[3];
            for (int i = rows.Count - 4; i >= 3; i--)
                rows[i].Remove();

            int maxRows = Math.Max(block21?.Count ?? 0, block22?.Count ?? 0);

            for (int i = 0; i < maxRows; i++)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                // ---------- 2.1 ----------
                if (block21 != null && i < block21.Count)
                {
                    ReplaceCellText(cells[0], block21[i].serial_no?.ToString() ?? "");
                    ReplaceCellText(cells[1], block21[i].hamlet_name ?? "");
                    ReplaceCellText(cells[2], block21[i].percentage?.ToString() ?? "");
                }
                else
                {
                    ReplaceCellText(cells[0], "");
                    ReplaceCellText(cells[1], "");
                    ReplaceCellText(cells[2], "");
                }

                // ---------- 2.2 ----------
                if (block22 != null && i < block22.Count)
                {
                    ReplaceCellText(cells[3], block22[i].serial_number?.ToString() ?? "");
                    ReplaceCellText(cells[4], block22[i].serial_no_of_hamlets_in_su ?? "");
                    ReplaceCellText(cells[5], block22[i].Percentage?.ToString() ?? "");
                    ReplaceCellText(cells[6], block22[i].IsChecked ? "✔" : "");
                }
                else
                {
                    ReplaceCellText(cells[3], "");
                    ReplaceCellText(cells[4], "");
                    ReplaceCellText(cells[5], "");
                    ReplaceCellText(cells[6], "");
                }

                table.AppendChild(row);
            }
            // ---------- BLOCK [2] REMARKS (2.1 & 2.2) ----------
            var rowsAfter = table.Elements<TableRow>().ToList();
            var remarksRow = rowsAfter.Last();
            var remarkCells = remarksRow.Elements<TableCell>().ToList();

            // Left side → Block 2.1 remarks
            if (remarkCells.Count >= 1)
            {
                ReplaceCellText(remarkCells[0], block_0_1?.remarks_block_2_1 ?? "");
            }

            // Right side → Block 2.2 remarks
            if (remarkCells.Count >= 2)
            {
                ReplaceCellText(remarkCells.Last(), block_0_1?.remarks_block_2_2 ?? "");
            }
        }
        //   private void FillBlock2_1_And_2_2(
        //Body body,
        //List<Tbl_Sch_0_0_Block_2_1> block21,
        //List<Tbl_Sch_0_0_Block_2_2> block22)
        //   {
        //       var table = body.Elements<Table>()
        //           .FirstOrDefault(t => t.InnerText.Contains("[2.1]"));

        //       if (table == null)
        //           return;

        //       var rows = table.Elements<TableRow>().ToList();
        //       if (rows.Count < 2)
        //           return;

        //       // ✅ Remarks row = LAST row (stable in your template)
        //       var remarksRow = rows.Last();

        //       // ✅ Template row = first row AFTER column-numbering row
        //       TableRow templateRow = null;

        //       for (int i = 0; i < rows.Count; i++)
        //       {
        //           if (rows[i].InnerText.Contains("(1)") &&
        //               rows[i].InnerText.Contains("(2)") &&
        //               rows[i].InnerText.Contains("(3)"))
        //           {
        //               if (i + 1 < rows.Count)
        //                   templateRow = rows[i + 1];
        //               break;
        //           }
        //       }

        //       if (templateRow == null)
        //           return;

        //       // 🔥 Remove old data rows (between template & remarks)
        //       foreach (var r in rows
        //           .SkipWhile(r => r != templateRow)
        //           .Skip(1)
        //           .TakeWhile(r => r != remarksRow)
        //           .ToList())
        //       {
        //           r.Remove();
        //       }

        //       int maxRows = Math.Max(block21?.Count ?? 0, block22?.Count ?? 0);

        //       for (int i = 0; i < maxRows; i++)
        //       {
        //           var row = (TableRow)templateRow.CloneNode(true);
        //           var cells = row.Elements<TableCell>().ToList();

        //           void Put(int index, string value)
        //           {
        //               if (index < cells.Count)
        //                   ReplaceCellText(cells[index], value ?? "");
        //           }

        //           // ---- LEFT (2.1) ----
        //           Put(0, block21 != null && i < block21.Count ? block21[i].serial_no?.ToString() : "");
        //           Put(1, block21 != null && i < block21.Count ? block21[i].hamlet_name : "");
        //           Put(2, block21 != null && i < block21.Count ? block21[i].percentage?.ToString() : "");

        //           // ---- RIGHT (2.2) ----
        //           Put(3, block22 != null && i < block22.Count ? block22[i].serial_number?.ToString() : "");
        //           Put(4, block22 != null && i < block22.Count ? block22[i].serial_no_of_hamlets_in_su : "");
        //           Put(5, block22 != null && i < block22.Count ? block22[i].Percentage?.ToString() : "");
        //           //Put(6, block22 != null && i < block22.Count && block22[i].IsChecked ? "✔" : "");

        //           table.InsertBefore(row, remarksRow);
        //       }

        //       // ---- REMARKS ----
        //       var remarkCells = remarksRow.Elements<TableCell>().ToList();

        //       if (remarkCells.Count > 0)
        //           ReplaceCellText(remarkCells[0], block_0_1?.remarks_block_2_1 ?? "");

        //       if (remarkCells.Count > 1)
        //           ReplaceCellText(remarkCells.Last(), block_0_1?.remarks_block_2_2 ?? "");
        //   }


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

        //private void FillBlock5(Body body, List<Tbl_Sch_0_0_Block_5> list)
        //{
        //    var table = body.Elements<Table>()
        //        .FirstOrDefault(t => t.InnerText.ToLower().Contains("[5]"));

        //    if (table == null || list == null || !list.Any())
        //        return;

        //    var rows = table.Elements<TableRow>().ToList();

        //    // Row index 3 = first data row (based on your template)
        //    TableRow templateRow = rows[3];

        //    // Remove existing data rows (keep header, total, remarks)
        //    for (int i = rows.Count - 3; i >= 3; i--)
        //        rows[i].Remove();

        //    foreach (var item in list)
        //    {
        //        var newRow = (TableRow)templateRow.CloneNode(true);
        //        var cells = newRow.Elements<TableCell>().ToList();

        //        ReplaceCellText(cells[0], item.serial_number?.ToString() ?? "");
        //        ReplaceCellText(cells[1], item.Percentage?.ToString() ?? "");
        //        ReplaceCellText(cells[2], item.IsChecked == true ? "✔" : "");

        //        table.AppendChild(newRow);
        //    }
        //    // ---------- BLOCK [5] REMARKS ----------
        //    var rowsAfter = table.Elements<TableRow>().ToList();
        //    var remarksRow = rowsAfter.Last();

        //    ReplaceCellText(GetLastCell(remarksRow), block_0_1?.remarks_block_5 ?? "");

        //}

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
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.ToLower().Contains("[7]"));

            if (table == null || list == null)
                return;

            var rows = table.Elements<TableRow>().ToList();


            TableRow templateRow = rows[3];

            TableRow remarksRow = rows.Last();

            for (int i = rows.Count - 2; i >= 3; i--)
                rows[i].Remove();

            int serial = 1;

            foreach (var item in list)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], serial.ToString());
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

                table.InsertBefore(row, remarksRow);

                serial++;
            }

            ReplaceCellText(
                GetLastCell(remarksRow),
                block_0_1?.remarks_block_7 ?? ""
            );
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
            var table = body.Elements<Table>().ElementAtOrDefault(7);
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


        //      private void CalculateBlock8(List<Tbl_Sch_0_0_Block_7> list, int sector,
        //          out int population,
        //          out int sss_11_listed, out int sss_12_listed, out int sss_21_listed, out int sss_22_listed,
        //          out int sss_31_listed, out int sss_32_listed, out int sss_40_listed, out int sss_90_listed, out int total_listed,
        //          out int sss_11_selected, out int sss_12_selected, out int sss_21_selected, out int sss_22_selected,
        //          out int sss_31_selected, out int sss_32_selected, out int sss_40_selected, out int sss_90_selected, out int total_selected,
        //          out int sss_11_orig, out int sss_12_orig, out int sss_21_orig, out int sss_22_orig,
        //          out int sss_31_orig, out int sss_32_orig, out int sss_40_orig, out int sss_90_orig, out int total_orig,
        //          out int sss_11_sub, out int sss_12_sub, out int sss_21_sub, out int sss_22_sub,
        //          out int sss_31_sub, out int sss_32_sub, out int sss_40_sub, out int sss_90_sub, out int total_sub,
        //          out int sss_11_total, out int sss_12_total, out int sss_21_total, out int sss_22_total,
        //          out int sss_31_total, out int sss_32_total, out int sss_40_total, out int sss_90_total, out int total_total,
        //          out int sss_11_cas, out int sss_12_cas, out int sss_21_cas, out int sss_22_cas,
        //          out int sss_31_cas, out int sss_32_cas, out int sss_40_cas, out int sss_90_cas, out int total_cas)
        //      {
        //          population = list.Sum(x => x.Block_7_5 ?? 0);

        //          int Count(int sss) => list.Count(x => x.SSS == sss);
        //          int Selected(int sss) => list.Count(x => x.SSS == sss && x.isInitialySelected == true);
        //          int Original(int sss) => list.Count(x =>
        //              x.SSS == sss &&
        //              x.isInitialySelected == true &&
        //              x.SubstitutionCount == 0 &&
        //              x.status != "CASUALTY");

        //          int Substituted(int sss) => list.Count(x => x.SSS == sss && x.SubstitutionCount > 0);
        //          int Total(int sss) => Original(sss) + Substituted(sss);
        //          int Casualty(int sss) => Selected(sss) - Total(sss);

        //          int[] rural = { 11, 12, 21, 22, 31, 32, 40, 90 };
        //          int[] urban = { 11, 121, 122, 21, 221, 222, 40, 90 };
        //          var sssList = sector == 1 ? rural : urban;

        //          int[] listed = sssList.Select(Count).ToArray();
        //          int[] selected = sssList.Select(Selected).ToArray();
        //          int[] orig = sssList.Select(Original).ToArray();
        //          int[] sub = sssList.Select(Substituted).ToArray();
        //          int[] total = sssList.Select(Total).ToArray();
        //          int[] cas = sssList.Select(Casualty).ToArray();

        //          sss_11_listed = listed[0]; sss_12_listed = listed[1]; sss_21_listed = listed[2]; sss_22_listed = listed[3];
        //          sss_31_listed = listed[4]; sss_32_listed = listed[5]; sss_40_listed = listed[6]; sss_90_listed = listed[7];
        //          total_listed = listed.Sum();

        //          sss_11_selected = selected[0]; sss_12_selected = selected[1]; sss_21_selected = selected[2]; sss_22_selected = selected[3];
        //          sss_31_selected = selected[4]; sss_32_selected = selected[5]; sss_40_selected = selected[6]; sss_90_selected = selected[7];
        //          total_selected = selected.Sum();

        //          sss_11_orig = orig[0]; sss_12_orig = orig[1]; sss_21_orig = orig[2]; sss_22_orig = orig[3];
        //          sss_31_orig = orig[4]; sss_32_orig = orig[5]; sss_40_orig = orig[6]; sss_90_orig = orig[7];
        //          total_orig = orig.Sum();

        //          sss_11_sub = sub[0]; sss_12_sub = sub[1]; sss_21_sub = sub[2]; sss_22_sub = sub[3];
        //          sss_31_sub = sub[4]; sss_32_sub = sub[5]; sss_40_sub = sub[6]; sss_90_sub = sub[7];
        //          total_sub = sub.Sum();

        //          sss_11_total = total[0]; sss_12_total = total[1]; sss_21_total = total[2]; sss_22_total = total[3];
        //          sss_31_total = total[4]; sss_32_total = total[5]; sss_40_total = total[6]; sss_90_total = total[7];
        //          total_total = total.Sum();

        //          sss_11_cas = cas[0]; sss_12_cas = cas[1]; sss_21_cas = cas[2]; sss_22_cas = cas[3];
        //          sss_31_cas = cas[4]; sss_32_cas = cas[5]; sss_40_cas = cas[6]; sss_90_cas = cas[7];
        //          total_cas = cas.Sum();
        //      }

        //      private void FillBlock8(
        //    Body body,
        //    int sector,
        //    string scheduleName,
        //    int population,
        //    Dictionary<int, (int l, int s, int o, int sub, int t, int c)> data
        //)
        //      {
        //          // 
        //          var table = body.Elements<Table>()
        //              .FirstOrDefault(t => t.InnerText.Contains("[8]"));

        //          if (table == null)
        //              return;

        //          var rows = table.Elements<TableRow>().ToList();


        //          foreach (var row in rows)
        //          {
        //              var cells = row.Elements<TableCell>().ToList();
        //              if (cells.Count < 2) continue;

        //              // T
        //              if (cells[0].InnerText.Contains("HIS"))
        //              {
        //                  ReplaceCellText(cells[0], scheduleName);
        //                  ReplaceCellText(cells[1], population.ToString());
        //                  break;
        //              }
        //          }

        //          foreach (var row in rows)
        //          {
        //              var cells = row.Elements<TableCell>().ToList();
        //              if (cells.Count < 9)
        //                  continue;

        //              // Column-3 = SSS
        //              string sssText = cells[2].InnerText
        //                  .Replace("\r", "")
        //                  .Replace("\n", "")
        //                  .Trim();

        //              int sss;

        //              if (sssText.Equals("all (99)", StringComparison.OrdinalIgnoreCase))
        //              {
        //                  sss = 99;
        //              }
        //              else if (!int.TryParse(sssText, out sss))
        //              {
        //                  continue; // non-data row
        //              }

        //              if (!data.ContainsKey(sss))
        //              {
        //                  ReplaceCellText(cells[3], "0");
        //                  ReplaceCellText(cells[4], "0");
        //                  ReplaceCellText(cells[5], "0");
        //                  ReplaceCellText(cells[6], "0");
        //                  ReplaceCellText(cells[7], "0");
        //                  ReplaceCellText(cells[8], "0");
        //                  continue;
        //              }
        //              var v = data[sss];

        //              ReplaceCellText(cells[3], v.l.ToString());   // Listed
        //              ReplaceCellText(cells[4], v.s.ToString());   // Selected
        //              ReplaceCellText(cells[5], v.o.ToString());   // Originally selected
        //              ReplaceCellText(cells[6], v.sub.ToString()); // Substituted
        //              ReplaceCellText(cells[7], v.t.ToString());   // Total
        //              ReplaceCellText(cells[8], v.c.ToString());   // Casualty
        //          }
        //      }




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
            ReplaceCellText(GetLastCell(rows[6]), !string.IsNullOrWhiteSpace(block_0_1?.SupervisorRemarks) ? "1" : "2");

            // 5.1 (i) In Block 9 / 10
            ReplaceCellText(GetLastCell(rows[7]), !string.IsNullOrWhiteSpace(block_0_1?.EnumeratorRemarks) || !string.IsNullOrWhiteSpace(block_0_1?.SupervisorRemarks)
                    ? "1"
                    : "2"
            );

            // 5.2 (ii) Elsewhere in the schedule
            ReplaceCellText(
                GetLastCell(rows[8]), string.IsNullOrWhiteSpace(block_0_1?.EnumeratorRemarks) && string.IsNullOrWhiteSpace(block_0_1?.SupervisorRemarks)
                    ? "1"
                    : "2"
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
                var commonQueries = new CommonQueries();
                var dbQueries = new DBQueries();
                //int hhdId = SessionStorage.selected_hhd_id;
                blockhis_1 = await dbQueries.Fetch_SCH_HIS_Block1(7);
                blockhis_3 = await dbQueries.Fetch_SCH_HIS_Block3(7);
                blockhis_4 = await dbQueries.Fetch_SCH_HIS_Block4(7);
                blockhis_5 = await dbQueries.Fetch_SCH_HIS_Block5(7);
                blockhis_6 = await dbQueries.Fetch_SCH_HIS_Block6(7);
                blockhis_7a = await dbQueries.Fetch_SCH_HIS_Block7A(7);
                blockhis_10 = await dbQueries.Fetch_SCH_HIS_Block10();
                //using var ms = new MemoryStream(File.ReadAllBytes(templatePath));
                using (var doc = WordprocessingDocument.Open(ms, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;

                    FillBlockHis1(body, blockhis_1);
                    FillBlockHis3(body, blockhis_3);
                    FillBlockHis4(body, blockhis_4);
                    //FillBlockHis5(body, blockhis_5);
                    //FillBlockHis6(body, blockhis_6);
                    //FillBlockHis7A(body, blockhis_7a);
                    //FillBlockHis10(body, blockhis_10);
                    //FillBlock5A(body, sch104record);
                    //FillBlock5B(body, sch104record);
                    //FillBlock5C(body, sch104record);
                    //FillBlock5D(body, sch104record);
                    //FillIdentification1(body, sch104record);
                    //FillBlock6(body, sch104record);

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
        
        private void FillBlockHis3(Body body, List<Tbl_Block_3> members)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[3]"));

            if (table == null || members == null || members.Count == 0)
                return;

            var rows = table.Elements<TableRow>().ToList();

            // 👉 According to template: header rows = 0,1 ; data template row = 2
            int templateRowIndex = 4;
            var templateRow = rows[templateRowIndex];

            // 🔥 Remove old data rows (keep header + remarks)
            for (int i = rows.Count - 1; i > templateRowIndex; i--)
                rows[i].Remove();

            foreach (var m in members)
            {
                var row = (TableRow)templateRow.CloneNode(true);
                table.AppendChild(row);

                var cells = row.Elements<TableCell>().ToList();
                int col = 0;

                // sequential writer (MERGE SAFE)
                void Put(string? value)
                {
                    if (col < cells.Count)
                    {
                        ReplaceCellText(cells[col], value ?? "");
                        col++;
                    }
                }

                // ---- EXACT mapping as per template ----
                Put(m.serial_no?.ToString());   // (1) S. no.
                Put(m.item_2);                 // (2) Name of member
                Put(m.item_3?.ToString());     // (3) Relation to head
                Put(m.gender?.ToString());     // (4) Gender
                Put(m.age?.ToString());        // (5) Age
                Put(m.item_6?.ToString());     // (6) Marital status
                Put(m.item_7?.ToString());     // (7) Education level
                Put(m.item_8?.ToString());     // (8) 30 days - Primary
                Put(m.item_9?.ToString());     // (9) 30 days - Secondary
                Put(m.item_10?.ToString());    // (10) 365 days - Primary
                Put(m.item_11?.ToString());    // (11) 365 days - Secondary
                Put(m.item_12?.ToString());    // (12) Beneficiary
            }
        }





        private void FillBlockHis4(Body body, Tbl_Block_4 block4)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[4]"));

            if (table == null || block4 == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            SafeReplace(rows, 2, 2, block4.item_1?.ToString());
            SafeReplace(rows, 3, 2, block4.item_2?.ToString());
            SafeReplace(rows, 4, 2, block4.item_3?.ToString());
            SafeReplace(rows, 5, 2, block4.item_6?.ToString());
            SafeReplace(rows, 6, 2, block4.item_7?.ToString());
            SafeReplace(rows, 7, 2, block4.item_11?.ToString());
            SafeReplace(rows, 8, 2, block4.item_14?.ToString());
            SafeReplace(rows, 9, 2, block4.remarks);
        }
        private void FillBlockHis5(Body body, List<Tbl_Block_5> list)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[5]"));

            if (table == null || list == null)
                return;

            var rows = table.Elements<TableRow>().ToList();
            var template = rows[2];

            for (int i = rows.Count - 1; i > 2; i--)
                rows[i].Remove();

            foreach (var item in list)
            {
                var row = (TableRow)template.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.serial_number?.ToString());
                ReplaceCellText(cells[1], item.item_1?.ToString());
                ReplaceCellText(cells[2], item.item_2?.ToString());
                ReplaceCellText(cells[3], item.item_11);

                table.AppendChild(row);
            }
        }
        private void FillBlockHis6(Body body, List<Tbl_Block_6> list)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[6]"));

            if (table == null || list == null)
                return;

            var rows = table.Elements<TableRow>().ToList();
            var template = rows[2];

            for (int i = rows.Count - 1; i > 2; i--)
                rows[i].Remove();

            foreach (var item in list)
            {
                var row = (TableRow)template.CloneNode(true);
                var cells = row.Elements<TableCell>().ToList();

                ReplaceCellText(cells[0], item.serial_no?.ToString());
                ReplaceCellText(cells[1], item.item_1?.ToString());
                ReplaceCellText(cells[2], item.item_2?.ToString());
                ReplaceCellText(cells[3], item.remarks);

                table.AppendChild(row);
            }
        }

        private void FillBlockHis7A(Body body, Tbl_Block_7a block)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[7A]"));

            if (table == null || block == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            SafeReplace(rows, 2, 1, block.item_3?.ToString());
            SafeReplace(rows, 3, 1, block.item_4?.ToString());
            SafeReplace(rows, 4, 1, block.item_5?.ToString());
            SafeReplace(rows, 5, 1, block.remarks);
        }
        private void FillBlockHis10(Body body, Tbl_Block_10 block10)
        {
            var table = body.Elements<Table>()
                .FirstOrDefault(t => t.InnerText.Contains("[10]"));

            if (table == null || block10 == null)
                return;

            var rows = table.Elements<TableRow>().ToList();

            SafeReplace(rows, 2, 1, block10.item_1?.ToString());
            SafeReplace(rows, 3, 1, block10.item_2?.ToString());
            SafeReplace(rows, 4, 1, block10.item_3?.ToString());
            SafeReplace(rows, 5, 1, block10.item_23?.ToString());
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
