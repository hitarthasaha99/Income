//using BootstrapBlazor.Components;
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;
//using Microsoft.Maui.Controls;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Income.Database.Models.SCH0_0;
//using Income.Database.Models.HIS_2026;
//using Income.Database.Queries;

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
        int hhdId = 0;
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
                //using var ms = new MemoryStream(File.ReadAllBytes(templatePath));
                using (var doc = WordprocessingDocument.Open(ms, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;

                    FillBlock0_1(body, block_0_1);
                    //FillBlock1(body, sch104record, SCH00record);
                    //FillBlock2(body, sch104record);
                    //FillListOfMember(body, sch104record);
                    //FillBlock3(body, sch104record);
                    //FillBlock4(body, sch104record);
                    //FillBlock41(body, sch104record);
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

        public async Task<byte[]> FillDocument26(Stream templatePath00)
        {
            try
            {
                using var ms = new MemoryStream();
                await templatePath00.CopyToAsync(ms);
                ms.Position = 0;
                var commonQueries = new CommonQueries();
                var dbQueries = new DBQueries();
                block_0_1 = await dbQueries.FetchBlock1();
                //using var ms = new MemoryStream(File.ReadAllBytes(templatePath));
                using (var doc = WordprocessingDocument.Open(ms, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;

                    //FillBlock0_1(body, block_0_1);
                    //FillBlock1(body, sch104record, SCH00record);
                    //FillBlock2(body, sch104record);
                    //FillListOfMember(body, sch104record);
                    //FillBlock3(body, sch104record);
                    //FillBlock4(body, sch104record);
                    //FillBlock41(body, sch104record);
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

        private void FillBlock0_1(Body body, Tbl_Sch_0_0_Block_0_1 block01)
        {
            var table = body.Elements<Table>().FirstOrDefault(t => t.InnerText.ToLower().Contains("[0]"));
            if (table != null)
            {
                var cells = table.Descendants<TableCell>().ToList();
                ReplaceCellText(cells[6], block01?.Block_0_1 ?? "");
                ReplaceCellText(cells[9], block01?.Block_0_2 ?? "");
                ReplaceCellText(cells[12], block01?.Block_0_3 ?? "");
                ReplaceCellText(cells[15], block01?.Block_0_4 ?? "");
                ReplaceCellText(cells[18], block01?.Block_0_5 ?? "");
                ReplaceCellText(cells[21], block01?.Block_0_6 ?? "");              
            }
        }

        //private void FillBlock1(Body body, Sch104Record sch104record, Sch000Record SCH00record)
        //{
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("[1] identification of sample household"));
        //    var table = body.Elements<Table>().ElementAtOrDefault(1);
        //    var Household = SCH00record.Households.FirstOrDefault(p => p.IsSelected && p.HHDSerial == hhdId);
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        ReplaceCellText(cells[9], sch104record?.FsuNumber ?? "");
        //        ReplaceCellText(cells[12], sch104record?.SRO ?? "");   // need data
        //        ReplaceCellText(cells[15], sch104record?.SchNumber ?? "");
        //        ReplaceCellText(cells[18], SCH00record?.TotalSu.ToInt() > 1 ? "1" : "");
        //        ReplaceCellText(cells[21], sch104record?.Sector ?? "");
        //        ReplaceCellText(cells[24], sch104record?.SelSubDiv ?? "");
        //        ReplaceCellText(cells[27], sch104record?.NssReg ?? "");
        //        ReplaceCellText(cells[30], sch104record?.SSS ?? "");
        //        ReplaceCellText(cells[33], sch104record?.DisCode ?? "");
        //        ReplaceCellText(cells[36], Household?.HouseSerial ?? "");
        //        ReplaceCellText(cells[39], sch104record?.Strm ?? "");
        //        ReplaceCellText(cells[42], sch104record?.InfSrl ?? "");
        //        ReplaceCellText(cells[45], sch104record?.Sstrm ?? "");
        //        ReplaceCellText(cells[48], sch104record?.RespCode ?? "");
        //        ReplaceCellText(cells[51], sch104record?.MmYear ?? "");
        //        ReplaceCellText(cells[54], sch104record?.SurvCode ?? "");
        //        ReplaceCellText(cells[57], sch104record?.Year ?? "");
        //        ReplaceCellText(cells[60], sch104record?.ReasonCode ?? "");
        //        ReplaceCellText(cells[63], sch104record?.Month ?? "");
        //        ReplaceCellText(cells[66], sch104record?.Visit ?? "");
        //        ReplaceCellText(cells[69], sch104record?.Panel ?? "");

        //    }

        //}

        //private void FillBlock2(Body body, Sch104Record sch104record)
        //{
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(2);
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        ReplaceCellText(cells[11], sch104record?.Fi1 ?? "");
        //        ReplaceCellText(cells[23], sch104record?.Fi2 ?? "");
        //        ReplaceCellText(cells[39], sch104record?.DOS ?? "");
        //        ReplaceCellText(cells[59], sch104record?.Time ?? "0");
        //        string informantDisplay = GetInformantDisplay(sch104record);
        //        ReplaceCellText(cells[63], informantDisplay);

        //        ReplaceCellText(cells[67], sch104record?.RespCode ?? "0");

        //    }

        //    var table7 = body.Elements<Table>().ElementAtOrDefault(3);
        //    var table8 = body.Elements<Table>().ElementAtOrDefault(4);
        //    if (table7 != null)
        //    {
        //        var cells = table7.Descendants<TableCell>().ToList();
        //        ReplaceCellText(cells[1], sch104record?.Remark1 ?? "");
        //    }

        //    if (table8 != null)
        //    {
        //        var cells = table8.Descendants<TableCell>().ToList();
        //        ReplaceCellText(cells[1], sch104record?.Remark2 ?? "");
        //    }

        //}

        //private string GetInformantDisplay(Sch104Record sch104record)
        //{
        //    if (sch104record == null || string.IsNullOrEmpty(sch104record.InfSrl))
        //        return "";

        //    if (sch104record.InfSrl.Trim() == "99")
        //        return "99 - Not a Household Member";

        //    if (!int.TryParse(sch104record.InfSrl?.Trim(), out int infSrlInt))
        //        return sch104record.InfSrl;

        //    var member = sch104record.HouseholdMemberData?
        //        .FirstOrDefault(x => x.Srl == infSrlInt);

        //    if (member != null && !string.IsNullOrEmpty(member.PName))
        //    {
        //        return $"{member.Srl} - {member.PName}";
        //    }

        //    return sch104record.InfSrl;
        //}

        //private void FillListOfMember(Body body, Sch104Record sch104record)
        //{
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(5);
        //    List<MemberDetails> members = new List<MemberDetails>();
        //    members = sch104record.HouseholdMemberData ?? new List<MemberDetails>();
        //    int HouseholdCount = members.Count;
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        ReplaceCellText(cells[2], sch104record?.HhdSize ?? "");
        //        ReplaceCellText(cells[4], HouseholdCount > 0 ? HouseholdCount.ToString() : "");

        //        if (members != null && members.Count > 0)
        //        {
        //            int startIndex = 7;
        //            int existingCellCount = cells.Count;
        //            for (var i = 0; i < members.Count; i++)
        //            {
        //                int baseIndex = startIndex + i * 2;
        //                if (baseIndex + 1 < existingCellCount)
        //                {
        //                    ReplaceCellText(cells[baseIndex], members[i]?.Srl.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 1], members[i]?.PName.ToString() ?? "");
        //                }
        //                else
        //                {
        //                    var row = new TableRow();
        //                    row.Append(
        //                        CreateTextCell(members[i]?.Srl.ToString() ?? ""),
        //                        CreateTextCell(members[i]?.PName.ToString() ?? "")
        //                       );
        //                    table.AppendChild(row);
        //                }
        //            }
        //        }
        //    }
        //}

        //private void FillBlock4(Body body, Sch104Record record)
        //{
        //    List<MemberDetails> Memberdata = new();
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(6);
        //    Memberdata = record?.HouseholdMemberData ?? new();
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        //var array = record.Households;
        //        if (Memberdata != null && Memberdata.Count > 0)
        //        {
        //            int startIndex = 62;
        //            int existingCellCount = cells.Count;
        //            for (var i = 0; i < Memberdata.Count; i++)
        //            {
        //                int baseIndex = startIndex + i * 18;
        //                if (baseIndex + 17 < existingCellCount)
        //                {
        //                    ReplaceCellText(cells[baseIndex], Memberdata[i]?.Srl.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.PName.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 2], Memberdata[i].StillMember.ToString());
        //                    ReplaceCellText(cells[baseIndex + 3], Memberdata[i]?.Rela.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 4], Memberdata[i]?.Gender ?? "");
        //                    ReplaceCellText(cells[baseIndex + 5], Memberdata[i].Age.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 6], Memberdata[i]?.Marital.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 7], Memberdata[i]?.Edu.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 8], Memberdata[i]?.TEdu.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 9], Memberdata[i]?.Grade.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 10], Memberdata[i]?.YrsBeforeI.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 11], Memberdata[i]?.YrsAfterGrade.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 12], Memberdata[i]?.IfLastYr.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 13], Memberdata[i]?.Months.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 14], Memberdata[i]?.CurrAtt.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 15], Memberdata[i]?.Secondary.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 16], Memberdata[i]?.AnyVocational.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 17], Memberdata[i]?.AnyTraining.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 18], Memberdata[i]?.TrainingCompleted.ToString() ?? "");
        //                }
        //                else
        //                {
        //                    var row = new TableRow();
        //                    row.Append(
        //                        CreateTextCell(Memberdata[i]?.Srl.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.PName.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.StillMember ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Rela?.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Gender ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Age.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Marital.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Edu.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.TEdu.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Grade.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.YrsBeforeI.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.YrsAfterGrade.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.IfLastYr.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Months.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.CurrAtt.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Secondary.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.AnyVocational.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.AnyTraining.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.TrainingCompleted.ToString() ?? "")
        //                    );
        //                    table.AppendChild(row);
        //                }
        //            }

        //        }

        //    }

        //}

        //private void FillBlock41(Body body, Sch104Record record)
        //{
        //    List<MemberDetails> Memberdata = new();
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(7);
        //    Memberdata = record?.HouseholdMemberData ?? new();
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        //var array = record.Households;
        //        if (Memberdata != null && Memberdata.Count > 0)
        //        {
        //            int startIndex = 15;
        //            int existingCellCount = cells.Count;
        //            var filteredMembers = Memberdata.FindAll(x => (x.Age.ToInt() >= 12 && x.Age.ToInt() <= 59 && x.AnyVocational.ToInt() == 1 && x.TrainingCompleted.ToInt() != 1));
        //            for (var i = 0; i < filteredMembers.Count; i++)
        //            {
        //                int baseIndex = startIndex + i * 7;
        //                if (baseIndex + 6 < existingCellCount)
        //                {
        //                    ReplaceCellText(cells[baseIndex], Memberdata[i]?.Srl.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.Age.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 2], Memberdata[i]?.Field.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 3], Memberdata[i]?.Duration.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 4], Memberdata[i]?.TypeTraining.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 5], Memberdata[i]?.SourceFund.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 6], Memberdata[i]?.TrainingEntity.ToString() ?? "");
        //                }
        //                else
        //                {
        //                    var row = new TableRow();

        //                    row.Append(
        //                        CreateTextCell(Memberdata[i]?.Srl.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Age?.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Field.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Duration.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.TypeTraining.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.SourceFund.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.TrainingEntity.ToString() ?? "")
        //                    );
        //                    table.AppendChild(row);
        //                }
        //            }

        //        }

        //    }

        //}

        //private void FillBlock5A(Body body, Sch104Record record)
        //{
        //    List<MemberDetails> Memberdata = new();
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(8);
        //    Memberdata = record?.HouseholdMemberData ?? new();
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        //var array = record.Households;
        //        if (Memberdata != null && Memberdata.Count > 0)
        //        {
        //            int startIndex = 83;
        //            int existingCellCount = cells.Count;
        //            for (var i = 0; i < Memberdata.Count; i++)
        //            {
        //                int baseIndex = startIndex + i * 16;
        //                if (baseIndex + 16 < existingCellCount)
        //                {
        //                    ReplaceCellText(cells[baseIndex], Memberdata[i]?.Srl.ToString() ?? "");
        //                    //ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.PName.ToString() ?? "");                            
        //                    ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.Age.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 2], Memberdata[i]?.Pas ?? "");
        //                    ReplaceCellText(cells[baseIndex + 3], Memberdata[i]?.NICDescPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 4], Memberdata[i]?.NICPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 5], Memberdata[i]?.NCOPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 6], Memberdata[i]?.HasSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 7], Memberdata[i]?.LocPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 8], Memberdata[i]?.EntypPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 9], Memberdata[i]?.WrkrPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 10], Memberdata[i]?.JobPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 11], Memberdata[i]?.LeavePas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 12], Memberdata[i]?.SSecPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 13], Memberdata[i]?.DestProdPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 14], Memberdata[i]?.Block5_1col_15.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 15], Memberdata[i]?.Block5_1col_16.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 16], Memberdata[i]?.Block5_1col_17.ToString() ?? "");
        //                }
        //                else
        //                {
        //                    var row = new TableRow();
        //                    row.Append(
        //                        CreateTextCell(Memberdata[i]?.Srl.ToString() ?? ""),
        //                        //CreateTextCell(Memberdata[i]?.PName.ToString() ?? ""),                              
        //                        CreateTextCell(Memberdata[i]?.Age?.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Pas ?? ""),
        //                        CreateTextCell(Memberdata[i]?.NICDescPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.NICPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.NCOPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.HasSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.LocPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.EntypPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.WrkrPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.JobPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.LeavePas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.SSecPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.DestProdPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_1col_15.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_1col_16.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_1col_17.ToString() ?? "")
        //                    );
        //                    table.AppendChild(row);
        //                }
        //            }

        //        }

        //    }
        //}

        //private void FillBlock5B(Body body, Sch104Record record)
        //{
        //    List<MemberDetails> Memberdata = new();
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(9);
        //    Memberdata = record?.HouseholdMemberData ?? new();
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        //var array = record.Households;
        //        if (Memberdata != null && Memberdata.Count > 0)
        //        {
        //            int startIndex = 56;
        //            int existingCellCount = cells.Count;
        //            var filteredMembers = Memberdata.Where(x => x.HasSas.ToInt() == 1).ToList();
        //            for (var i = 0; i < filteredMembers.Count; i++)
        //            {
        //                int baseIndex = startIndex + i * 16;
        //                if (baseIndex + 15 < existingCellCount)
        //                {
        //                    ReplaceCellText(cells[baseIndex], Memberdata[i]?.Srl.ToString() ?? "");
        //                    //ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.PName.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.Age.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 2], Memberdata[i]?.Sas ?? "");
        //                    ReplaceCellText(cells[baseIndex + 3], Memberdata[i]?.NICDescSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 4], Memberdata[i]?.NICSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 5], Memberdata[i]?.NCOSas.ToString() ?? "");
        //                    //ReplaceCellText(cells[baseIndex + 7], Memberdata[i]?.HasSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 6], Memberdata[i]?.LocSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 7], Memberdata[i]?.EntypSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 8], Memberdata[i]?.WrkrSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 9], Memberdata[i]?.JobSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 10], Memberdata[i]?.LeaveSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 11], Memberdata[i]?.SSecSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 12], Memberdata[i]?.DestProdSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 13], Memberdata[i]?.Block5_2col_14.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 14], Memberdata[i]?.Block5_2col_15.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 15], Memberdata[i]?.Block5_2col_16.ToString() ?? "");
        //                }
        //                else
        //                {
        //                    var row = new TableRow();
        //                    row.Append(
        //                        CreateTextCell(Memberdata[i]?.Srl.ToString() ?? ""),
        //                        //CreateTextCell(Memberdata[i]?.PName.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Age?.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Sas ?? ""),
        //                        CreateTextCell(Memberdata[i]?.NICDescSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.NICSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.NCOSas.ToString() ?? ""),
        //                        //CreateTextCell(Memberdata[i]?.HasSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.LocSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.EntypSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.WrkrSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.JobSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.LeaveSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.SSecSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.DestProdSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_2col_14.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_2col_15.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_2col_16.ToString() ?? "")
        //                    );
        //                    table.AppendChild(row);
        //                }
        //            }

        //        }

        //    }

        //}

        //private void FillBlock5C(Body body, Sch104Record record)
        //{
        //    List<MemberDetails> Memberdata = new();
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(10);
        //    Memberdata = record?.HouseholdMemberData ?? new();
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        //var array = record.Households;
        //        if (Memberdata != null && Memberdata.Count > 0)
        //        {
        //            int startIndex = 34;
        //            int existingCellCount = cells.Count;
        //            var filteredMembers = Memberdata.Where(x => x.Age.ToInt() >= 5).ToList();
        //            for (var i = 0; i < filteredMembers.Count; i++)
        //            {
        //                int baseIndex = startIndex + i * 12;
        //                if (baseIndex + 11 < existingCellCount)
        //                {
        //                    ReplaceCellText(cells[baseIndex], Memberdata[i]?.Srl.ToString() ?? "");
        //                    //ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.PName.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.Age.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 2], Memberdata[i]?.Pas ?? "");
        //                    ReplaceCellText(cells[baseIndex + 3], Memberdata[i]?.Sas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 4], Memberdata[i]?.EverWorked1151.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 5], Memberdata[i]?.DurPas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 6], Memberdata[i]?.DurSas.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 7], Memberdata[i]?.ToSearchWork.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 8], Memberdata[i]?.SpellUnemp.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 9], Memberdata[i]?.EverWorked8197.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 10], Memberdata[i]?.ReasonNotWorking.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 11], Memberdata[i]?.MainReason.ToString() ?? "");
        //                }
        //                else
        //                {
        //                    var row = new TableRow();
        //                    row.Append(
        //                        CreateTextCell(Memberdata[i]?.Srl.ToString() ?? ""),
        //                        //CreateTextCell(Memberdata[i]?.PName.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Age?.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Pas ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Sas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.EverWorked1151.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.DurPas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.DurSas.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.ToSearchWork.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.SpellUnemp.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.EverWorked8197.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.ReasonNotWorking.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.MainReason.ToString() ?? "")
        //                    );
        //                    table.AppendChild(row);
        //                }
        //            }

        //        }

        //    }

        //}

        //private void FillBlock5D(Body body, Sch104Record record)
        //{
        //    List<MemberDetails> Memberdata = new();
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(11);
        //    Memberdata = record?.HouseholdMemberData ?? new();
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        //var array = record.Households;
        //        if (Memberdata != null && Memberdata.Count > 0)
        //        {
        //            int startIndex = 34;
        //            int existingCellCount = cells.Count;
        //            var filteredMembers = Memberdata.Where(x => x.Age.ToInt() >= 5 && ((x.Block5_1col_15 == "1" || x.Block5_1col_16 == "1") || (x.Block5_2col_14 == "1" || x.Block5_2col_15 == "1"))).ToList();
        //            for (var i = 0; i < filteredMembers.Count; i++)
        //            {
        //                int baseIndex = startIndex + i * 11;
        //                if (baseIndex + 10 < existingCellCount)
        //                {
        //                    ReplaceCellText(cells[baseIndex], Memberdata[i]?.Srl.ToString() ?? "");
        //                    //ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.PName.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 1], Memberdata[i]?.Age.ToString() ?? "");
        //                    if ((Memberdata[i].Block5_1col_15 == "1" || Memberdata[i].Block5_1col_16 == "1") && !string.IsNullOrEmpty(Memberdata[i].Pas))
        //                    {
        //                        ReplaceCellText(cells[baseIndex + 2], Memberdata[i]?.Pas ?? "");
        //                    }
        //                    if ((Memberdata[i].Block5_2col_14 == "1" || Memberdata[i].Block5_2col_15 == "1") && (Memberdata[i].Block5_1col_15 != "1" && Memberdata[i].Block5_1col_16 != "1") && !string.IsNullOrEmpty(Memberdata[i].Sas))
        //                    {
        //                        ReplaceCellText(cells[baseIndex + 3], Memberdata[i]?.Sas.ToString() ?? "");
        //                    }
        //                    if ((Memberdata[i].Block5_1col_15 == "1" || Memberdata[i].Block5_1col_16 == "1") && !string.IsNullOrEmpty(Memberdata[i].Pas))
        //                    {
        //                        ReplaceCellText(cells[baseIndex + 4], Memberdata[i]?.NICPas.ToString() ?? "");
        //                    }
        //                    if ((Memberdata[i].Block5_2col_14 == "1" || Memberdata[i].Block5_2col_15 == "1") && (Memberdata[i].Block5_1col_15 != "1" && Memberdata[i].Block5_1col_16 != "1") && string.IsNullOrEmpty(Memberdata[i].NICPas) && !string.IsNullOrEmpty(Memberdata[i].Sas))
        //                    {
        //                        ReplaceCellText(cells[baseIndex + 5], Memberdata[i]?.NICSas.ToString() ?? "");
        //                    }
        //                    ReplaceCellText(cells[baseIndex + 6], Memberdata[i]?.Block5_4col_5.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 7], Memberdata[i]?.Block5_4col_6.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 8], Memberdata[i]?.Block5_4col_7.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 9], Memberdata[i]?.Block5_4col_8.ToString() ?? "");
        //                    ReplaceCellText(cells[baseIndex + 10], Memberdata[i]?.Block5_4col_9.ToString() ?? "");
        //                }
        //                else
        //                {
        //                    var row = new TableRow();
        //                    TableCell pasCell;
        //                    TableCell sasCell;
        //                    TableCell nicPasCell;
        //                    TableCell nicSasCell;
        //                    if ((Memberdata[i].Block5_1col_15 == "1" || Memberdata[i].Block5_1col_16 == "1") && !string.IsNullOrEmpty(Memberdata[i].Pas))
        //                    {
        //                        pasCell = CreateTextCell(Memberdata[i]?.Pas ?? "");
        //                    }
        //                    else
        //                    {
        //                        pasCell = CreateTextCell("");
        //                    }

        //                    if ((Memberdata[i].Block5_2col_14 == "1" || Memberdata[i].Block5_2col_15 == "1") && (Memberdata[i].Block5_1col_15 != "1" && Memberdata[i].Block5_1col_16 != "1") && !string.IsNullOrEmpty(Memberdata[i].Sas))
        //                    {
        //                        sasCell = CreateTextCell(Memberdata[i]?.Sas ?? "");
        //                    }
        //                    else
        //                    {
        //                        sasCell = CreateTextCell("");
        //                    }

        //                    if ((Memberdata[i].Block5_1col_15 == "1" || Memberdata[i].Block5_1col_16 == "1") && !string.IsNullOrEmpty(Memberdata[i].Pas))
        //                    {
        //                        nicPasCell = CreateTextCell(Memberdata[i]?.NICPas ?? "");
        //                    }
        //                    else
        //                    {
        //                        nicPasCell = CreateTextCell("");
        //                    }

        //                    if ((Memberdata[i].Block5_2col_14 == "1" || Memberdata[i].Block5_2col_15 == "1") && (Memberdata[i].Block5_1col_15 != "1" && Memberdata[i].Block5_1col_16 != "1") && string.IsNullOrEmpty(Memberdata[i].NICPas) && !string.IsNullOrEmpty(Memberdata[i].Sas))
        //                    {
        //                        nicSasCell = CreateTextCell(Memberdata[i]?.NICSas ?? "");
        //                    }
        //                    else
        //                    {
        //                        nicSasCell = CreateTextCell("");
        //                    }
        //                    row.Append(
        //                        CreateTextCell(Memberdata[i]?.Srl.ToString() ?? ""),
        //                        //CreateTextCell(Memberdata[i]?.PName.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Age?.ToString() ?? ""),
        //                        pasCell,
        //                        sasCell,
        //                        nicPasCell,
        //                        nicSasCell,
        //                        CreateTextCell(Memberdata[i]?.Block5_4col_5.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_4col_6.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_4col_7.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_4col_8.ToString() ?? ""),
        //                        CreateTextCell(Memberdata[i]?.Block5_4col_9.ToString() ?? "")
        //                    );
        //                    table.AppendChild(row);
        //                }
        //            }

        //        }

        //    }

        //}

        //private void FillBlock3(Body body, Sch104Record sch104record)
        //{
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(12);
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        ReplaceCellText(cells[3], sch104record?.HhdSizeFrame ?? "");
        //        ReplaceCellText(cells[6], sch104record?.HhdType ?? "");   // need data
        //        ReplaceCellText(cells[9], sch104record?.Religion ?? "");
        //        ReplaceCellText(cells[12], sch104record?.SocGrp ?? "");
        //        ReplaceCellText(cells[18], sch104record?.Purchase ?? "");
        //        ReplaceCellText(cells[21], sch104record?.HomeGrown ?? "");
        //        ReplaceCellText(cells[24], sch104record?.KindWage ?? "");
        //        ReplaceCellText(cells[27], sch104record?.ClothFootwear ?? "");
        //        ReplaceCellText(cells[30], sch104record?.DurableGoods ?? "");
        //        ReplaceCellText(cells[33], sch104record?.MCE ?? "");
        //        ReplaceCellText(cells[36], sch104record?.Lposs ?? "");
        //        ReplaceCellText(cells[39], sch104record?.Llout ?? "");
        //        //ReplaceCellText(cells[39], sch104record?.ReasonCode ?? "");              

        //    }

        //}

        //private void FillIdentification1(Body body, Sch104Record sch104record)
        //{
        //    var TableList = body.Elements<Table>().Where(t => t.InnerText.ToLower().Contains("state/u.t."));
        //    var table = body.Elements<Table>().ElementAtOrDefault(13);
        //    if (table != null)
        //    {
        //        var cells = table.Descendants<TableCell>().ToList();
        //        ReplaceCellText(cells[7], sch104record?.Mobile ?? "");
        //        ReplaceCellText(cells[11], sch104record?.Mobile2 ?? "");   // need data
        //        ReplaceCellText(cells[15], sch104record?.Landline ?? "");
        //    }
        //}

        //private void FillBlock6(Body body, Sch104Record sch104record)
        //{
        //    var members = sch104record?.HouseholdMemberData ?? new List<MemberDetails>();
        //    if (!members.Any())
        //        return;

        //    var templateTable = body.Elements<Table>().ElementAtOrDefault(14);
        //    if (templateTable == null)
        //        return;

        //    templateTable.Remove();

        //    var filteredMembers = sch104record?.Visit.ToInt() == 1
        //        ? members.Where(x => x.Age.ToInt() > 4).ToList()
        //        : members.Where(x => new int[] { 1, 2, 3 }.Contains(x.StillMember.ToInt()) && x.Age.ToInt() > 4).ToList();

        //    foreach (var member in filteredMembers)
        //    {



        //        Table block6Table = (Table)templateTable.CloneNode(true);
        //        var cells = block6Table.Descendants<TableCell>().ToList();

        //        if (!string.IsNullOrEmpty(sch104record?.DateOfWeekend) && DateTime.TryParseExact(sch104record.DateOfWeekend, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        //        {
        //            ReplaceCellText(cells[2], parsedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
        //        }
        //        else
        //        {
        //            ReplaceCellText(cells[2], sch104record?.DateOfWeekend ?? "");
        //        }
        //        ReplaceCellText(cells[11], member.Srl.ToString());
        //        ReplaceCellText(cells[14], member.Age ?? "");
        //        // < !--Day7-- >
        //        ReplaceCellText(cells[37], member.Das17 ?? "");
        //        ReplaceCellText(cells[38], member.Ind17 ?? "");
        //        ReplaceCellText(cells[39], member.Hrs17 ?? "");
        //        ReplaceCellText(cells[40], member.Hr7 ?? "");
        //        ReplaceCellText(cells[41], member.AHr7 ?? "");
        //        ReplaceCellText(cells[42], member.Ern17 ?? "");
        //        ReplaceCellText(cells[46], member.Das27 ?? "");
        //        ReplaceCellText(cells[47], member.Ind27 ?? "");
        //        ReplaceCellText(cells[48], member.Hrs27 ?? "");
        //        ReplaceCellText(cells[51], member.Ern27 ?? "");

        //        // < !--Day6-- >
        //        ReplaceCellText(cells[55], member.Das16 ?? "");
        //        ReplaceCellText(cells[56], member.Ind16 ?? "");
        //        ReplaceCellText(cells[57], member.Hrs16 ?? "");
        //        ReplaceCellText(cells[58], member.Hr6 ?? "");
        //        ReplaceCellText(cells[59], member.AHr6 ?? "");
        //        ReplaceCellText(cells[60], member.Ern16 ?? "");
        //        ReplaceCellText(cells[64], member.Das26 ?? "");
        //        ReplaceCellText(cells[65], member.Ind26 ?? "");
        //        ReplaceCellText(cells[66], member.Hrs26 ?? "");
        //        ReplaceCellText(cells[69], member.Ern26 ?? "");

        //        // < !--Day5-- >
        //        ReplaceCellText(cells[73], member.Das15 ?? "");
        //        ReplaceCellText(cells[74], member.Ind15 ?? "");
        //        ReplaceCellText(cells[75], member.Hrs15 ?? "");
        //        ReplaceCellText(cells[76], member.Hr5 ?? "");
        //        ReplaceCellText(cells[77], member.AHr5 ?? "");
        //        ReplaceCellText(cells[78], member.Ern15 ?? "");
        //        ReplaceCellText(cells[82], member.Das25 ?? "");
        //        ReplaceCellText(cells[83], member.Ind25 ?? "");
        //        ReplaceCellText(cells[84], member.Hrs25 ?? "");
        //        ReplaceCellText(cells[87], member.Ern25 ?? "");

        //        // < !--Day4-- >
        //        ReplaceCellText(cells[91], member.Das14 ?? "");
        //        ReplaceCellText(cells[92], member.Ind14 ?? "");
        //        ReplaceCellText(cells[93], member.Hrs14 ?? "");
        //        ReplaceCellText(cells[94], member.Hr4 ?? "");
        //        ReplaceCellText(cells[95], member.AHr4 ?? "");
        //        ReplaceCellText(cells[96], member.Ern14 ?? "");
        //        ReplaceCellText(cells[100], member.Das24 ?? "");
        //        ReplaceCellText(cells[101], member.Ind24 ?? "");
        //        ReplaceCellText(cells[102], member.Hrs24 ?? "");
        //        ReplaceCellText(cells[105], member.Ern24 ?? "");

        //        // < !--Day3-- >
        //        ReplaceCellText(cells[109], member.Das13 ?? "");
        //        ReplaceCellText(cells[110], member.Ind13 ?? "");
        //        ReplaceCellText(cells[111], member.Hrs13 ?? "");
        //        ReplaceCellText(cells[112], member.Hr3 ?? "");
        //        ReplaceCellText(cells[113], member.AHr3 ?? "");
        //        ReplaceCellText(cells[114], member.Ern13 ?? "");
        //        ReplaceCellText(cells[118], member.Das23 ?? "");
        //        ReplaceCellText(cells[119], member.Ind23 ?? "");
        //        ReplaceCellText(cells[120], member.Hrs23 ?? "");
        //        ReplaceCellText(cells[123], member.Ern23 ?? "");

        //        //// < !--Day2-- >
        //        ReplaceCellText(cells[127], member.Das12 ?? "");
        //        ReplaceCellText(cells[128], member.Ind12 ?? "");
        //        ReplaceCellText(cells[129], member.Hrs12 ?? "");
        //        ReplaceCellText(cells[130], member.Hr2 ?? "");
        //        ReplaceCellText(cells[131], member.AHr2 ?? "");
        //        ReplaceCellText(cells[132], member.Ern12 ?? "");
        //        ReplaceCellText(cells[136], member.Das22 ?? "");
        //        ReplaceCellText(cells[137], member.Ind22 ?? "");
        //        ReplaceCellText(cells[138], member.Hrs22 ?? "");
        //        ReplaceCellText(cells[141], member.Ern22 ?? "");

        //        //// < !--Day1-- >
        //        ReplaceCellText(cells[145], member.Das11 ?? "");
        //        ReplaceCellText(cells[146], member.Ind11 ?? "");
        //        ReplaceCellText(cells[147], member.Hrs11 ?? "");
        //        ReplaceCellText(cells[148], member.Hr1 ?? "");
        //        ReplaceCellText(cells[149], member.AHr1 ?? "");
        //        ReplaceCellText(cells[150], member.Ern11 ?? "");
        //        ReplaceCellText(cells[154], member.Das21 ?? "");
        //        ReplaceCellText(cells[155], member.Ind21 ?? "");
        //        ReplaceCellText(cells[156], member.Hrs21 ?? "");
        //        ReplaceCellText(cells[159], member.Ern21 ?? "");

        //        ReplaceCellText(cells[162], member.TotalHours ?? "");
        //        ReplaceCellText(cells[163], member.TotalAvgHours ?? "");
        //        ReplaceCellText(cells[168], member.CWS ?? "");
        //        ReplaceCellText(cells[172], member.NICCWS ?? "");
        //        ReplaceCellText(cells[176], member.NCOCWS ?? "");
        //        ReplaceCellText(cells[180], member.NICDescCWS ?? "");
        //        ReplaceCellText(cells[183], member.EarningsRegular ?? "");
        //        ReplaceCellText(cells[186], member.EarningsSelf ?? "");

        //        body.AppendChild(block6Table);
        //    }
        //}

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
