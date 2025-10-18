using BlazorBootstrap;
using CommunityToolkit.Mvvm.ComponentModel;
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
        Blazored.Toast.Services.ToastService ToastService = new Blazored.Toast.Services.ToastService();
        public List<Tbl_Sch_0_0_Block_2_2>? tbl_Sch_0_0_Block_2_2 { get; set; } = new();
        [ObservableProperty]
        private double total_population_percentage = 0.0;
        public List<Tbl_Sch_0_0_Block_2_1>? tbl_Sch_0_0_Block_2_1 { get; set; } = new();
        private List<SerialPopulation> serialPopulationsList = new();
        DBQueries SCH_0_0_Queries = new();
        CommonQueries cQ = new();
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
                var fsu_response = await cQ.FetchFsuByFsuId(SessionStorage.SelectedFSUId);
                if (fsu_response != null)
                {
                    if (fsu_response.totalsu > 0)
                    {
                        //var block_2_1 = await SCH_0_0_Queries.FetchSCH0Block2_1Data();
                        //if (block_2_1 != null)
                        //{

                        //    foreach (var row in block_2_1)
                        //    {
                        //        SerialPopulation serialPopulation = new SerialPopulation();
                        //        serialPopulation.Id = row.serial_no.GetValueOrDefault();
                        //        serialPopulation.PopulationPercentage = row.percentage.GetValueOrDefault();
                        //        serialPopulationsList.Add(serialPopulation);
                        //    }
                        //    //_serialPopulationsList = serialPopulationsList;
                        //}
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
                                SerialPopulationsList = serialPopulationsList
                            });
                        }
                    }
                }
            }
            tbl_Sch_0_0_Block_2_2 = tbl_Sch_0_0_Block_2_2.OrderBy(k => k.serial_number).ToList();
            CalculateTotalPercentage();
            //Total_population_percentage = CalculateTotalPopulationPercentage(tbl_Sch_0_0_Block_2_2);
            OnPropertyChanged(nameof(tbl_Sch_0_0_Block_2_2));
        }

        //public void CalculatePopulation()
        //{
        //    try
        //    {

        //        string[] result = _hamlet_serial_nos.Split(',');
        //        double total = 0;
        //        foreach (string s in result)
        //        {
        //            double r = 0;
        //            if (double.TryParse(s, out r))
        //            {
        //                var data = SerialPopulationsList.Where(x => x.Id == r).SingleOrDefault();
        //                if (data != null)
        //                {
        //                    total += data.PopulationPercentage;
        //                }
        //            }
        //        }
        //        Percentage = total;
        //    }
        //    catch (Exception)
        //    {
        //        Percentage = 0;
        //    }
        //}
    


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
    }
}

