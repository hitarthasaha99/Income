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
        public List<int?> selected_serial_number = new();
        Blazored.Toast.Services.IToastService ToastService;
        public List<Tbl_Sch_0_0_Block_2_2>? tbl_Sch_0_0_Block_2_2 { get; set; } = new();
        [ObservableProperty]
        private double total_population_percentage = 0.0;
        public List<Tbl_Sch_0_0_Block_2_1>? tbl_Sch_0_0_Block_2_1 { get; set; } = new();
        private List<SerialPopulation> _serialPopulationsList = new();
        DBQueries SCH_0_0_Queries = new();
        CommonQueries cQ = new();
        //public Modal SelectionModal = default;
        public List<Tbl_Sch_0_0_Block_2_2> SelectionList = new();
        public Guid current_selected_row { get; set; } = Guid.Empty;
        public Block_2_2_VM(IToastService toastService)
        {
            ToastService = toastService;
        }

        public async Task Init()
        {
            tbl_Sch_0_0_Block_2_1 = await SCH_0_0_Queries.FetchSCH0Block2_1Data();
            tbl_Sch_0_0_Block_2_2 = await SCH_0_0_Queries.FetchSCH0Block2_2Data();
            if (tbl_Sch_0_0_Block_2_2 == null || tbl_Sch_0_0_Block_2_2.Count == 0)
            {
                var fsu_response = await cQ.FetchFsuByFsuId(SessionStorage.SelectedFSUId);
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
            tbl_Sch_0_0_Block_2_2 = tbl_Sch_0_0_Block_2_2.OrderBy(k => k.serial_number).ToList();
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
                data_set.Percentage = total;
                tbl_Sch_0_0_Block_2_2.FirstOrDefault(x => x.id == data_set.id).Percentage = total;
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
                    total += item.Percentage;
                }
            }
            Total_population_percentage = total;
        }

        public void DoSelection()
        {
            var result = Validate();
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ToastService.ShowError(error);
                }
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

        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            ResetSerialPopulationsList();
            try
            {
                double total = tbl_Sch_0_0_Block_2_2.Sum(row => row.Percentage);
                if (total != 100)
                    result.Errors.Add("Total percentage must be equal to 100.");

                if (tbl_Sch_0_0_Block_2_2.Any(row => row.Percentage == 0))
                    result.Errors.Add("Hamlet percentage cannot be zero.");

                if(tbl_Sch_0_0_Block_2_2.Any(row => row.serial_no_of_hamlets_in_su?.Split(',').Length == 0))
                    result.Errors.Add("At least one hamlet must be specified for each SU.");

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
    }
}

