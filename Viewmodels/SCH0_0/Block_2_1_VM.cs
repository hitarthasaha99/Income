using BlazorBootstrap;
using Blazored.Toast.Services;
using BootstrapBlazor.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Income.Common;
using Income.Database.Models.Common;
using Income.Database.Models.SCH0_0;
using Income.Database.Queries;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Income.Viewmodels.SCH0_0
{
    public partial class Block_2_1_VM : BaseVM
    {
        public event Action NotifyUiUpdate;
        private IToastService _toastService;

        DBQueries SCH_0_0_Queries = new();
        public List<Tbl_Sch_0_0_Block_2_1> tbl_Sch_0_0_block_2_1 { get; set; } = new();
        [ObservableProperty]
        private double total_population_percentage = 0;
        DBQueries dB = new();
        CommonQueries cQ = new();
        int D = 0;
        public Block_2_1_VM(IToastService toastService)
        {
            // block_4_1 = new Tbl_Sch_0_0_Block_4_1();
        }
        
        public void AddRow()
        {
            var newRow = new Tbl_Sch_0_0_Block_2_1
            {
                id = Guid.NewGuid(),
                serial_no = tbl_Sch_0_0_block_2_1.Count(x => x.is_deleted == false) + 1,
                hamlet_name = string.Empty,
                percentage = null,
                is_deleted = false
            };
            tbl_Sch_0_0_block_2_1.Add(newRow);
            OnPropertyChanged(nameof(tbl_Sch_0_0_block_2_1));
        }

        public void RemoveRow(Tbl_Sch_0_0_Block_2_1 row)
        {
            try
            {
                if (row == null)
                {
                    _toastService.ShowError("Row Not Found!");
                    return;
                }
                int index = tbl_Sch_0_0_block_2_1.FindIndex(k => k.id == row.id);
                if (index == -1)
                {
                    _toastService.ShowError("Row Not Found!");
                    return;
                }
                if (row.serial_no <= D)
                {
                    _toastService.ShowError("Cannot delete pre-listed entries");
                    return;
                }
                tbl_Sch_0_0_block_2_1[index].is_deleted = true;
                ResetSerialNumbers();
                OnPropertyChanged(nameof(tbl_Sch_0_0_block_2_1));
                CalculateTotalPopulationPercentage();
                return;
            }
            catch (Exception ex)
            {
                _toastService.ShowError("Error While Deleting Row!");
                return;
            }
        }

        private void ResetSerialNumbers()
        {
            int serial = 1;
            foreach (var item in tbl_Sch_0_0_block_2_1)
            {
                if (item.is_deleted != true)
                {
                    item.serial_no = serial;
                    serial++;
                }
            }
        }

        public void HandleChange(ChangeEventArgs e, Guid id, string field_name)
        {
            try
            {
                int index = tbl_Sch_0_0_block_2_1.FindIndex(k => k.id == id);
                if (index == -1)
                {
                    return;
                }
                if (field_name == "name_of_hamlet")
                {
                    tbl_Sch_0_0_block_2_1[index].hamlet_name = e.Value.ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        double value = Convert.ToDouble(e.Value);
                        value = Math.Round(value, 1, MidpointRounding.AwayFromZero); // restrict to 1 decimal
                        tbl_Sch_0_0_block_2_1[index].percentage = value;
                    }
                    else
                    {
                        tbl_Sch_0_0_block_2_1[index].percentage = 0;
                    }
                    CalculateTotalPopulationPercentage();   
                }
                OnPropertyChanged(nameof(tbl_Sch_0_0_block_2_1));
            }
            catch(Exception ex)
            {
                return;
            }
        }

        public void CalculateTotalPopulationPercentage()
        {
            try
            {
                double perc = tbl_Sch_0_0_block_2_1.Where(x => x.is_deleted != true).Sum(static x => x.percentage.GetValueOrDefault());
                Total_population_percentage = perc;
                NotifyUiUpdate?.Invoke();
            }
            catch(Exception ex)
            {
            }
        }

        public async Task Init(IToastService toastService)
        {
            _toastService = toastService;
            tbl_Sch_0_0_block_2_1 = await SCH_0_0_Queries.FetchSCH0Block2_1Data();
            tbl_Sch_0_0_block_2_1 = tbl_Sch_0_0_block_2_1.OrderBy(k => k.serial_no).ToList();
            var fsu_response = await cQ.FetchFsuByFsuId(SessionStorage.SelectedFSUId);
            if (fsu_response != null)
            {
                D = fsu_response.totalsu.GetValueOrDefault();
            }    
            if (tbl_Sch_0_0_block_2_1 == null || tbl_Sch_0_0_block_2_1.Count == 0)
            {
                if (fsu_response != null)
                {
                    for (int i =1; i <= fsu_response.totalsu; i++)
                    {
                        AddRow();
                    }
                }
            }
            CalculateTotalPopulationPercentage();
            OnPropertyChanged(nameof(tbl_Sch_0_0_block_2_1));
        }

        const double EPSILON = 0.000001;


        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            try
            {
                var non_deleted = tbl_Sch_0_0_block_2_1.Where(x => x.is_deleted != true).ToList();
                double total = non_deleted.Sum(row => row.percentage.GetValueOrDefault());
                if (total != 100)
                    result.Errors.Add("Total percentage must be equal to 100");

                if (non_deleted.Any(row =>
                            row.percentage == null ||
                            Math.Abs(row.percentage.Value) < EPSILON))
                {
                    result.Errors.Add("Hamlet percentage cannot be zero.");
                }

                if (non_deleted.Any(row => string.IsNullOrWhiteSpace(row.hamlet_name)))
                    result.Errors.Add("Hamlet name cannot be empty.");

                if (non_deleted
                    .GroupBy(x => x.hamlet_name.ToLower())
                    .Any(g => g.Count() > 1))
                    result.Errors.Add("Hamlet names must be unique.");

                result.IsValid = result.Errors.Count == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add("An unexpected error occurred during validation.");
                result.IsValid = false;
            }

            return result;
        }

        public bool ArePercentagesWithinRange(IEnumerable<Tbl_Sch_0_0_Block_2_1> items, double allowedDifference = 5.0)
        {
            var validPercentages = items
                .Where(x => x.percentage.HasValue)
                .Select(x => x.percentage.Value)
                .ToList();

            if (validPercentages.Count <= 1)
                return true; // Only one or none — trivially within range

            double min = validPercentages.Min();
            double max = validPercentages.Max();

            return (max - min) <= allowedDifference;
        }

    }
}
