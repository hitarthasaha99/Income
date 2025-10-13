using BlazorBootstrap;
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
        Blazored.Toast.Services.ToastService ToastService = new Blazored.Toast.Services.ToastService();
        public List<Tbl_Sch_0_0_Block_2_2> tbl_Sch_0_0_Block_2_2 { get; set; } = new();
        public double total_population_percentage { get; set; } = new();
        public List<Tbl_Sch_0_0_Block_2_1> tbl_Sch_0_0_Block_2_1 { get; set; } = new();
        public Tbl_Fsu_List Tbl_Fsu_List { get; set; } = new();
        DBQueries SCH_0_0_Queries = new();
        public Modal SelectionModal = default;
        public List<Tbl_Sch_0_0_Block_2_2> SelectionList = new();
        public Guid current_selected_row { get; set; } = Guid.Empty;
        public Block_2_2_VM()
        {

        }

        public async Task Init()
        {
            tbl_Sch_0_0_Block_2_1 = await SCH_0_0_Queries.FetchSCH0Block2_1Data();
            tbl_Sch_0_0_Block_2_2 = await SCH_0_0_Queries.FetchSCH0Block2_2Data();
            if (tbl_Sch_0_0_Block_2_2 == null || tbl_Sch_0_0_Block_2_2.Count == 0)
            {
                for (var i = 0; i < Tbl_Fsu_List.su_n; i++)
                {
                    tbl_Sch_0_0_Block_2_2.Add(new Tbl_Sch_0_0_Block_2_2
                    {
                        id = Guid.NewGuid(),
                        is_deleted = false,
                        is_selected = i + 1 == Tbl_Fsu_List.su_d,
                        serial_number = i + 1,
                        SerialNumber = tbl_Sch_0_0_Block_2_2[i].SerialNumber
                    });
                }
            }
            tbl_Sch_0_0_Block_2_2 = tbl_Sch_0_0_Block_2_2.OrderBy(k => k.SerialNumber).ToList();
            total_population_percentage = CalculateTotalPopulationPercentage(tbl_Sch_0_0_Block_2_2);
            OnPropertyChanged(nameof(total_population_percentage));
            OnPropertyChanged(nameof(tbl_Sch_0_0_Block_2_2));
            return;
        }

        public async void HandleModal(string action = "", Guid? id = null)
        {
            if (action == "OPEN")
            {
                current_selected_row = id ?? Guid.Empty;
                SelectionList = new();
                foreach (var item in tbl_Sch_0_0_Block_2_1)
                {
                    if (item.is_deleted == false)
                    {
                        SelectionList.Add(new Tbl_Sch_0_0_Block_2_2
                        {
                            id = Guid.NewGuid(),
                            serial_number = item.serial_no,
                            serial_no_of_hamlets_in_su = item.name_of_hamlet,
                            population_percentage = item.percentage_of_population,
                        });
                    }
                }
                OnPropertyChanged(nameof(SelectionList));
                await SelectionModal.ShowAsync();
            }
            else
            {
                current_selected_row = Guid.Empty;
                await SelectionModal.HideAsync();
            }
            return;
        }

        public void SubmitSelection()
        {
            var selected_data = SelectionList.Where(k => k.is_selected == true).ToList();
            if (selected_data.Count == 0)
            {
                ToastService.ShowError("Please Select Atleast One Row!");
                return;
            }
            if (current_selected_row == Guid.Empty)
            {
                ToastService.ShowError("Row Not Found!");
                return;
            }
            int index = tbl_Sch_0_0_Block_2_2.FindIndex(k => k.id == current_selected_row);
            if (index == -1)
            {
                ToastService.ShowError("Row Not Found!");
                return;
            }
            string serial_no = "";
            foreach (var item in selected_data)
            {
                selected_serial_number.Add(item.serial_number);
                serial_no += item.serial_number + ",";
            }
            tbl_Sch_0_0_Block_2_2[index].serial_no_of_hamlets_in_su += serial_no;
            tbl_Sch_0_0_Block_2_2[index].Percentage = tbl_Sch_0_0_Block_2_2[index].Percentage != null ? tbl_Sch_0_0_Block_2_2[index].Percentage + CalculateTotalPopulationPercentage(selected_data) : CalculateTotalPopulationPercentage(selected_data);
            total_population_percentage = CalculateTotalPopulationPercentage(tbl_Sch_0_0_Block_2_2);
            OnPropertyChanged(nameof(total_population_percentage));
            OnPropertyChanged(nameof(tbl_Sch_0_0_Block_2_2));
            OnPropertyChanged(nameof(selected_serial_number));
            HandleModal();
            return;
        }

        public double CalculateTotalPopulationPercentage(List<Tbl_Sch_0_0_Block_2_2> data_set)
        {
            double total_population_percentage = data_set.Sum(static x => x.Percentage);
            return total_population_percentage;
        }
    }
}

