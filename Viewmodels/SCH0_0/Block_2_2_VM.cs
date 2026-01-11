using BlazorBootstrap;
using Blazored.Toast.Services;
using BootstrapBlazor.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.Spreadsheet;
using Income.Common;
using Income.Database.Models.Common;
using Income.Database.Models.SCH0_0;
using Income.Database.Queries;
using System.ComponentModel;

namespace Income.Viewmodels.SCH0_0
{
    public partial class Block_2_2_VM : BaseVM
    {
        public event Action NotifyUiUpdate;
        public List<Tbl_Sch_0_0_Block_2_2>? tbl_Sch_0_0_Block_2_2 { get; set; } = new();
        [ObservableProperty]
        private double total_population_percentage = 0.0;
        [ObservableProperty]
        private bool _allowSelection = false;
        public List<Tbl_Sch_0_0_Block_2_1>? tbl_Sch_0_0_Block_2_1 { get; set; } = new();
        private Tbl_Fsu_List Tbl_Fsu_List = new();
        DBQueries SCH_0_0_Queries = new();
        CommonQueries cQ = new();
        public Block_2_2_VM()
        {
        }

        public async Task Init()
        {
            tbl_Sch_0_0_Block_2_1 = await SCH_0_0_Queries.FetchSCH0Block2_1Data();
            if (tbl_Sch_0_0_Block_2_1 != null && tbl_Sch_0_0_Block_2_1.Count > 0)
            {
                tbl_Sch_0_0_Block_2_1 = tbl_Sch_0_0_Block_2_1.Where(x => x.is_deleted == null || x.is_deleted == false).ToList();
                tbl_Sch_0_0_Block_2_1 = tbl_Sch_0_0_Block_2_1.OrderBy(k => k.serial_no).ToList();
            }
            tbl_Sch_0_0_Block_2_2 = await SCH_0_0_Queries.FetchSCH0Block2_2Data();
            if (tbl_Sch_0_0_Block_2_2 != null && tbl_Sch_0_0_Block_2_2.Count > 0)
            {
                tbl_Sch_0_0_Block_2_2 = tbl_Sch_0_0_Block_2_2.Where(x => x.is_deleted == null || x.is_deleted == false).ToList();
            }
            var fsu_response = Tbl_Fsu_List = await cQ.FetchFsuByFsuId(SessionStorage.SelectedFSUId);

            if (tbl_Sch_0_0_Block_2_2 == null || tbl_Sch_0_0_Block_2_2.Count == 0)
            {
                if (fsu_response != null)
                {
                    if (fsu_response.totalsu > 0)
                    {
                        for (int i = 1; i <= fsu_response.totalsu; i++)
                        {
                            tbl_Sch_0_0_Block_2_2.Add(new Tbl_Sch_0_0_Block_2_2
                            {
                                id = Guid.NewGuid(),
                                is_deleted = false,
                                is_selected = false,
                                is_enabled = true,
                                Percentage = 0,
                                serial_number = i,
                            });
                        }
                    }
                }
            }
            AllowSelection = !tbl_Sch_0_0_Block_2_2?.Any(x => x.IsChecked == true) ?? true;
            tbl_Sch_0_0_Block_2_2 = tbl_Sch_0_0_Block_2_2?.OrderBy(k => k.serial_number).ToList();
            CalculateTotalPercentage();
            OnPropertyChanged(nameof(tbl_Sch_0_0_Block_2_2));
        }

        
    


        public void CalculateTotalPopulationPercentage(Tbl_Sch_0_0_Block_2_2 data_set)
        {
            try
            {
                string[] result = data_set.serial_no_of_hamlets_in_su.Split(',');
                double total = 0;
                foreach (string s in result)
                {
                    double r = 0;
                    if (double.TryParse(s, out r))
                    {
                        var data = tbl_Sch_0_0_Block_2_1.Where(x => x.serial_no == r).SingleOrDefault();
                        if (data != null)
                        {
                            total += data.percentage.GetValueOrDefault();
                        }
                    }
                }
                total = Math.Round(total, 2);
                data_set.Percentage = total;
                tbl_Sch_0_0_Block_2_2.FirstOrDefault(x => x.id == data_set.id).Percentage = total;
                CalculateTotalPercentage();
                OnPropertyChanged(nameof(tbl_Sch_0_0_Block_2_2));
            }
            catch (Exception ex)
            {

            }
        }

        public void CalculateTotalPopulationPercentage_Urban()
        {
            try
            {

                CalculateTotalPercentage();
                OnPropertyChanged(nameof(tbl_Sch_0_0_Block_2_2));
            }
            catch (Exception ex)
            {

            }
        }

        public void CalculateTotalPercentage()
        {
            double total = 0;
            foreach (var item in tbl_Sch_0_0_Block_2_2)
            {
                if (!item.is_deleted.GetValueOrDefault())
                {
                    total += item.Percentage.GetValueOrDefault();
                }
            }
            total = Math.Round(total, 2);
            Total_population_percentage = total;
        }

        public async void DoSelection()
        {
            try
            {
                List<Tbl_Sch_0_0_Block_2_2> list = [];
                var percentages = tbl_Sch_0_0_Block_2_2
                          .Select(row => row.Percentage.GetValueOrDefault()) // Extract Percentage values.
                          .ToList();
                double minValue = percentages.Min();
                double maxValue = percentages.Max();
                if (Math.Abs(maxValue - minValue) > 5)
                {
                    //bool r = await AppShell.Current.DisplayAlert("All the sub-units need to have more or less equal population. Do you want to continue?", "", "Yes", "No");

                    //if (!r)
                    //{
                    //    return;
                    //}
                }
                if (Tbl_Fsu_List != null)
                {
                    if (Tbl_Fsu_List.totalsu > 0 && Tbl_Fsu_List.totalsu >= Tbl_Fsu_List.selsu)
                    {
                        int index = 1;
                        foreach (var row in tbl_Sch_0_0_Block_2_2)
                        {
                            row.IsChecked = false;
                            row.is_enabled = false;// Reset IsChecked for all rows
                            row.SamplingSerialNumberOfTheHgSb = string.Empty; // Reset SamplingSerialNumberOfTheHgSb
                            row.SampleHgSbNumber = string.Empty; // Reset SampleHgSbNumber
                            if (index == Tbl_Fsu_List.selsu)
                            {
                                row.IsChecked = true;
                                row.SampleHgSbNumber = "1";
                            }
                            row.fsu_id = SessionStorage.SelectedFSUId;

                            Tbl_Sch_0_0_Block_2_2 n1 = new();
                            n1.IsChecked = row.IsChecked;
                            n1.is_enabled = row.is_enabled;
                            n1.SamplingSerialNumberOfTheHgSb = row.SamplingSerialNumberOfTheHgSb;
                            n1.SampleHgSbNumber = row.SampleHgSbNumber;
                            n1.fsu_id = row.fsu_id;
                            n1.serial_number = row.serial_number;
                            n1.HamletName = row.HamletName;
                            n1.Percentage = row.Percentage;
                            n1.tenant_id = row.tenant_id;
                            n1.serial_no_of_hamlets_in_su = row.serial_no_of_hamlets_in_su;
                            if (string.IsNullOrEmpty(row.id.ToString()))
                            {
                                // Save the data object to the database
                                n1.id = Guid.NewGuid();
                            }
                            else
                            {
                                n1.id = row.id;
                            }
                            list.Add(n1);
                            index++;
                        }
                        if (list.Count > 0)
                        {
                            await SCH_0_0_Queries.SaveSCH0Block2_2(list);
                        }
                        tbl_Sch_0_0_Block_2_2 = list;
                        AllowSelection = false;
                        SessionStorage.hamlet_selection_done = true;
                        OnPropertyChanged(nameof(tbl_Sch_0_0_Block_2_2));
                        NotifyUiUpdate?.Invoke();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        void ResetSerialPopulationsList()
        {
            if (tbl_Sch_0_0_Block_2_1 != null)
            {
                foreach (var row in tbl_Sch_0_0_Block_2_1)
                {
                    row.is_selected = false;
                }
            }
        }

        public bool PopulationVariationExists()
        {
            try
            {
                var percentages = tbl_Sch_0_0_Block_2_2
                          .Select(row => row.Percentage.GetValueOrDefault()) // Extract Percentage values.
                          .ToList();
                double minValue = percentages.Min();
                double maxValue = percentages.Max();
                return Math.Abs(maxValue - minValue) > 5;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            ResetSerialPopulationsList();
            try
            {
                double total = tbl_Sch_0_0_Block_2_2.Sum(row => row.Percentage.GetValueOrDefault());
                total = Math.Round(total, 2);
                if (total != 100)
                    result.Errors.Add("Total percentage must be equal to 100.");

                if (tbl_Sch_0_0_Block_2_2.Any(row =>
                            row.Percentage == null ||
                            Math.Abs(row.Percentage.Value) < EPSILON))
                    result.Errors.Add("Hamlet percentage cannot be zero.");

                if(tbl_Sch_0_0_Block_2_2.Any(row => row.serial_no_of_hamlets_in_su?.Split(',').Length == 0))
                    result.Errors.Add("At least one hamlet must be specified for each SU.");

                if (Tbl_Fsu_List.totalsu == 0)
                {
                    result.Errors.Add("Total SU cannot be zero.");
                }
                else if (Tbl_Fsu_List.selsu > Tbl_Fsu_List.totalsu)
                {
                    result.Errors.Add("Selected SU cannot be greater than Total SU.");
                }

                foreach (var item in tbl_Sch_0_0_Block_2_2)
                {
                    string[] res = item.serial_no_of_hamlets_in_su?.Split(',') ?? [];
                    if (res.Length == 0)
                    {
                        result.Errors.Add($"No hamlet found in serial number of SU {item.serial_number}");
                    }

                    foreach (string s in res)
                    {
                        double r = 0;
                        if (double.TryParse(s, out r))
                        {
                            var data = tbl_Sch_0_0_Block_2_1?.Where(x => x.serial_no == r);
                            if (data == null || data.Count() == 0)
                            {
                                result.Errors.Add($"Invalid hamlet serial number {s} found in serial number of SU {item.serial_number}");
                            }
                            else if (data.Count() > 1)
                            {
                                result.Errors.Add($"Same hamlet serial number {s} found in serial number of SU {item.serial_number}");
                            }
                            else if (data.First().is_selected)
                            {
                                result.Errors.Add($"Same hamlet serial number {s} found multiple SU");
                            }
                            else
                            {
                                data.First().is_selected = true;
                            }
                        }
                        else
                        {
                            result.Errors.Add($"Invalid hamlet serial number {s} found in serial number of SU {item.serial_number}");
                        }
                    }
                }

                result.IsValid = result.Errors.Count == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add("An unexpected error occurred during validation.");
                result.IsValid = false;
            }

            return result;
        }

        const double EPSILON = 0.000001;

        public ValidationResult ValidateUrban()
        {
            var result = new ValidationResult();
            try
            {
                double total = tbl_Sch_0_0_Block_2_2.Sum(row => row.Percentage.GetValueOrDefault());
                if (total != 100)
                    result.Errors.Add("Total percentage must be equal to 100.");

                if (tbl_Sch_0_0_Block_2_2.Any(row =>
                            row.Percentage == null ||
                            Math.Abs(row.Percentage.Value) < EPSILON)) //row =>
                    
                    result.Errors.Add("Hamlet percentage cannot be zero.");

                if (Tbl_Fsu_List.totalsu == 0)
                {
                    result.Errors.Add("Total SU cannot be zero.");
                }
                else if (Tbl_Fsu_List.selsu > Tbl_Fsu_List.totalsu)
                {
                    result.Errors.Add("Selected SU cannot be greater than Total SU.");
                }

                result.IsValid = result.Errors.Count == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add("An unexpected error occurred during validation.");
                result.IsValid = false;
            }

            return result;
        }
    }
}

