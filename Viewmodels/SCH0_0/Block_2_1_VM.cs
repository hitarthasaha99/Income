using BlazorBootstrap;
using BootstrapBlazor.Components;
using Income.Common;
using Income.Database.Models.SCH0_0;
using Income.Database.Queries;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Income.Viewmodels.SCH0_0
{
    public partial class Block_2_1_VM : BaseVM
    {
        public event Action NotifyUiUpdate;
        Blazored.Toast.Services.ToastService ToastService = new Blazored.Toast.Services.ToastService();
        DBQueries SCH_0_0_Queries = new();
        public List<Tbl_Sch_0_0_Block_2_1> tbl_Sch_0_0_block_2_1 { get; set; } = new();
        public double total_population_percentage { get; set; } = new();
        DBQueries dB = new();
        CommonQueries cQ = new();
        public Block_2_1_VM()
        {
            // block_4_1 = new Tbl_Sch_0_0_Block_4_1();
        }
        
        public void AddRow()
        {
            var newRow = new Tbl_Sch_0_0_Block_2_1
            {
                id = Guid.NewGuid(),
                serial_no = tbl_Sch_0_0_block_2_1.Count + 1,
                name_of_hamlet = string.Empty,
                percentage = 0,
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
                    ToastService.ShowError("Row Not Found!");
                    return;
                }
                int index = tbl_Sch_0_0_block_2_1.FindIndex(k => k.id == row.id);
                if (index == -1)
                {
                    ToastService.ShowError("Row Not Found!");
                    return;
                }
                tbl_Sch_0_0_block_2_1[index].is_deleted = true;
                OnPropertyChanged(nameof(tbl_Sch_0_0_block_2_1));
                CalculateTotalPopulationPercentage();
                return;
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Error While Deleting Row!");
                return;
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
                    tbl_Sch_0_0_block_2_1[index].name_of_hamlet = e.Value.ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        tbl_Sch_0_0_block_2_1[index].percentage = Convert.ToDouble(e.Value);
                    }
                    else
                    {
                        tbl_Sch_0_0_block_2_1[index].percentage = 0;
                    }
                    CalculateTotalPopulationPercentage();   
                }
                OnPropertyChanged(nameof(tbl_Sch_0_0_block_2_1));
                return;
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
                double perc = tbl_Sch_0_0_block_2_1.Where(x => x.is_deleted != true).Sum(static x => x.percentage);
                total_population_percentage = perc;
                OnPropertyChanged(nameof(total_population_percentage));
                return;
            }
            catch(Exception ex)
            {
                total_population_percentage = 0;
                return;
            }
        }

        public async Task Init()
        {
            tbl_Sch_0_0_block_2_1 = await SCH_0_0_Queries.FetchSCH0Block2_1Data();
            tbl_Sch_0_0_block_2_1 = tbl_Sch_0_0_block_2_1.OrderBy(k => k.serial_no).ToList();
            if (tbl_Sch_0_0_block_2_1 == null || tbl_Sch_0_0_block_2_1.Count == 0)
            {
                var fsu_response = await cQ.FetchFsuByFsuId(SessionStorage.SelectedFSUId);
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
            return;
        }
    }
}
