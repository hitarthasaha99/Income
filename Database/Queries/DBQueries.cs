using Blazored.Toast.Services;
using Income.Common;
using Income.Database.Models.SCH0_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Queries
{
    public class DBQueries : Database
    {
        ToastService toastService = new();

        public async Task<int?> SaveSCH0Block2_1(List<Tbl_Sch_0_0_Block_2_1> tbl_Sch_0_0_Block_2_1)
        {
            try
            {
                if (tbl_Sch_0_0_Block_2_1 != null && tbl_Sch_0_0_Block_2_1.Count > 0)
                {
                    foreach (var item in tbl_Sch_0_0_Block_2_1)
                    {
                        var check_existence = await _database.Table<Tbl_Sch_0_0_Block_2_1>().Where(x => x.id == item.id).ToListAsync();
                        if (check_existence != null && check_existence.Count > 0)
                        {
                            await _database.UpdateAsync(item);
                        }
                        else
                        {
                            await _database.InsertAsync(item);
                        }
                    }
                    return 1;
                }
                else
                {
                    toastService.ShowError($"Getting Error While storing data!");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 4.1: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Tbl_Sch_0_0_Block_2_1>?> FetchSCH0Block2_1Data()
        {
            try
            {
                List<Tbl_Sch_0_0_Block_2_1> data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_2_1>("SELECT * FROM Tbl_Sch_0_0_Block_2_1 WHERE fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                return data_set;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Getting Error fetching data! {ex}");
                return null;
            }
        }

        public async Task<int?> SaveSCH0Block2_2(List<Tbl_Sch_0_0_Block_2_2> tbl_Sch_0_0_Block_2_2)
        {
            try
            {
                if (tbl_Sch_0_0_Block_2_2 != null && tbl_Sch_0_0_Block_2_2.Count > 0)
                {
                    foreach (var item in tbl_Sch_0_0_Block_2_2)
                    {
                        var check_existence = await _database.Table<Tbl_Sch_0_0_Block_2_2>().Where(x => x.id == item.id).ToListAsync();
                        if (check_existence != null && check_existence.Count > 0)
                        {
                            await _database.UpdateAsync(item);
                        }
                        else
                        {
                            await _database.InsertAsync(item);
                        }
                    }
                    return 1;
                }
                else
                {
                    toastService.ShowError($"Getting Error While storing data!");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 4.2: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Tbl_Sch_0_0_Block_2_2>?> FetchSCH0Block2_2Data()
        {
            try
            {
                List<Tbl_Sch_0_0_Block_2_2> data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_2_2>("SELECT * FROM Tbl_Sch_0_0_Block_2_2 WHERE fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                return data_set;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Getting Error fetching data! {ex}");
                return null;
            }
        }

        public async Task<int?> SaveBlock4_2A(Tbl_Sch_0_0_Block_4 tbl_Sch_0_0_Block_4_2A)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Sch_0_0_Block_4>().Where(x => x.id == tbl_Sch_0_0_Block_4_2A.id).ToListAsync();
                if (check_existence != null && check_existence.Count > 0)
                {
                    status = await _database.UpdateAsync(tbl_Sch_0_0_Block_4_2A);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_Sch_0_0_Block_4_2A);
                }
                return status;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 4.2A: {ex.Message}");
                return null;
            }
        }

        public async Task<Tbl_Sch_0_0_Block_4?> GetBlock4_2A()
        {
            try
            {
                var response = await _database.QueryAsync<Tbl_Sch_0_0_Block_4>("SELECT * FROM Tbl_Sch_0_0_Block_4_2A WHERE fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                return response != null && response.Count > 0 ? response.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While Getting SCH 0 Block 4.2A: {ex.Message}");
                return null;
            }
        }

        public async Task<int?> SaveSCH0Block4_3(List<Tbl_Sch_0_0_Block_5> tbl_Sch_0_0_Block_4_3)
        {
            try
            {
                if (tbl_Sch_0_0_Block_4_3 != null && tbl_Sch_0_0_Block_4_3.Count > 0)
                {
                    foreach (var item in tbl_Sch_0_0_Block_4_3)
                    {
                        var check_existence = await _database.Table<Tbl_Sch_0_0_Block_5>().Where(x => x.id == item.id).ToListAsync();
                        if (check_existence != null && check_existence.Count > 0)
                        {
                            await _database.UpdateAsync(item);
                        }
                        else
                        {
                            await _database.InsertAsync(item);
                        }
                    }
                    return 1;
                }
                else
                {
                    toastService.ShowError($"Getting Error While storing data!");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 4.3: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Tbl_Sch_0_0_Block_5>?> FetchSCH0Block4_3Data()
        {
            try
            {
                List<Tbl_Sch_0_0_Block_5> data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_5>("SELECT * FROM Tbl_Sch_0_0_Block_4_3 WHERE fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                return data_set;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Getting Error fetching data! {ex}");
                return null;
            }
        }

        public async Task<int?> SaveSCH0Block5(List<Tbl_Sch_0_0_Block_7> tbl_Sch_0_0_Block_5)
        {
            try
            {
                if (tbl_Sch_0_0_Block_5 != null && tbl_Sch_0_0_Block_5.Count > 0)
                {
                    foreach (var item in tbl_Sch_0_0_Block_5)
                    {
                        var check_existence = await _database.Table<Tbl_Sch_0_0_Block_7>().Where(x => x.id == item.id).ToListAsync();
                        if (check_existence != null && check_existence.Count > 0)
                        {
                            await _database.UpdateAsync(item);
                        }
                        else
                        {
                            await _database.InsertAsync(item);
                        }
                    }
                    return 1;
                }
                else
                {
                    toastService.ShowError($"Getting Error While storing data!");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 5: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Tbl_Sch_0_0_Block_7>?> FetchSCH0Block5Data()
        {
            try
            {
                List<Tbl_Sch_0_0_Block_7> data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_7>("SELECT * FROM Tbl_Sch_0_0_Block_5 WHERE fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                return data_set;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Getting Error fetching data! {ex}");
                return null;
            }
        }

        //public async Task<Tbl_Sch_0_0_Block_6?> FetchBlock6Data()
        //{
        //    try
        //    {
        //        var block_0_data = await FetchBlock1();
        //        Tbl_Sch_0_0_Block_6? data = new();
        //        data.id = Guid.NewGuid();
        //        data.schedule_name = "NHTS25";
        //        data.sss = "all(9)";
        //        var block_5_reponse = await FetchSCH0Block5Data();
        //        data.population = (block_5_reponse?.Where(x => x.is_household == true).Sum(x => x.Block_5A_5));
        //        int Data = GetEducationFilteredSubstitution(block_5_reponse);
        //        data.listed_hhd = block_5_reponse.Count(x => x.is_household == true);
        //        data.selected_hhd = block_5_reponse.Where(l => l.is_initially_selected_travel == true).Count();
        //        data.originally_selected_hhd = block_5_reponse.Where(x => x.is_initially_selected_travel == true && x.substitution_count_travel == 0 && x.status_travel != "CASUALTY").Count();
        //        data.number_of_hhd_surveyed_substituted = Data;
        //        data.total = (data?.originally_selected_hhd ?? 0) + (data?.number_of_hhd_surveyed_substituted ?? 0);
        //        data.cauality = (data?.selected_hhd ?? 0) - (data?.total ?? 0);
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public static int GetEducationFilteredSubstitution(List<Tbl_Sch_0_0_Block_7> block_5_reponse)
        {
            var counter = 0;
            var initialSelectedList = block_5_reponse.Where(entry => entry.is_initially_selected_travel == true && entry.substitution_count_travel != 0).ToList();

            foreach (var item in initialSelectedList)
            {
                var filteredData = block_5_reponse.Where(entry => entry.is_selected_travel == true && entry.original_household_id_travel == item.Block_5A_3).FirstOrDefault(); // && entry.substituted_for_id_cms != item.Block_5A_3
                if (filteredData != null && filteredData.status_travel == "SUBSTITUTE")
                {
                    counter++;
                }
                //}
            }
            return counter;
        }
        public async Task<int?> SaveBlock1(Tbl_Sch_0_0_Block_0_1 tbl_Sch_0_0_Block_0_1)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Sch_0_0_Block_0_1>().Where(x => x.id == tbl_Sch_0_0_Block_0_1.id).ToListAsync();
                if (check_existence != null && check_existence.Count > 0)
                {
                    status = await _database.UpdateAsync(tbl_Sch_0_0_Block_0_1);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_Sch_0_0_Block_0_1);
                }
                return status;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 1: {ex.Message}");
                return null;
            }
        }

        public async Task<Tbl_Sch_0_0_Block_0_1?> FetchBlock1()
        {
            try
            {
                var response = await _database.QueryAsync<Tbl_Sch_0_0_Block_0_1>("SELECT * FROM Tbl_Sch_0_0_Block_0_1 WHERE fsu_id = ?", SessionStorage.SelectedFSUId);
                if (response != null && response.Count > 0)
                {
                    return response.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 1: {ex.Message}");
                return null;
            }
        }

        public async Task<Tbl_Sch_0_0_FieldOperation?> FetchBlock2()
        {
            try
            {
                var response = await _database.QueryAsync<Tbl_Sch_0_0_FieldOperation>("SELECT * FROM Tbl_Sch_0_0_Block_2 WHERE fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                if (response != null && response.Count > 0)
                {
                    return response.FirstOrDefault();
                }
                else
                {
                    return new Tbl_Sch_0_0_FieldOperation();
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 1: {ex.Message}");
                return new Tbl_Sch_0_0_FieldOperation();
            }
        }

        public async Task<Tbl_Sch_0_0_Block_7> GetBlock5Data(int hhd_id = 0)
        {
            try
            {
                List<Tbl_Sch_0_0_Block_7>? data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_7>("SELECT * FROM Tbl_Sch_0_0_Block_5 WHERE fsu_id = ? AND tenant_id = ? AND Block_5A_3 = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id, SessionStorage.selected_hhd_id);
                return data_set != null && data_set.Count > 0 ? data_set.FirstOrDefault() : new();
            }
            catch (Exception ex)
            {
                return new Tbl_Sch_0_0_Block_7();
            }
        }

        public async Task<int?> SaveFileData(List<Tbl_Sch_0_0_Block_3> files)
        {
            try
            {
                int status = new();
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        file.block_name = file.is_sub_unit == true ? CommonEnum.sch_0_0_block_3.ToString() : CommonEnum.sch_0_0_block_3_1.ToString();
                        await _database.QueryAsync<Tbl_Sch_0_0_Block_3>("DELETE FROM Tbl_File WHERE block_name = ? AND is_deleted = false AND fsu_id = ? AND tenant_id = ?", file.block_name, SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                        file.id = Guid.NewGuid();
                        file.is_deleted = false;
                        status = await _database.InsertAsync(file);
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Tbl_Sch_0_0_Block_3?> GetFileData(string block_name)
        {
            try
            {
                var check_existence = await _database.QueryAsync<Tbl_Sch_0_0_Block_3>("SELECT * FROM Tbl_File WHERE block_name = ? AND is_deleted = false AND fsu_id = ? AND tenant_id = ?", block_name, SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                if (!check_existence.Any())
                {
                    return null;
                }
                else
                {
                    return check_existence.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<Tbl_Sch_0_0_Block_7> FetchCasualtyOrOriginalHouseHold(int FSUID, int ID)
        {
            return _database.Table<Tbl_Sch_0_0_Block_7>().Where(x => x.fsu_id == FSUID && x.Block_5A_3 == ID).FirstOrDefaultAsync();
        }

        //find a household with a higher and smaller serial number
        public async Task<Tbl_Sch_0_0_Block_7?> FetchHouseholdWithHigherSerialNumber(int FSUID, int casualtyHouseholdId)
        {
            // Step 1: Get the casualty household
            var casualtyHousehold = await _database.Table<Tbl_Sch_0_0_Block_7>()
                .Where(h => h.fsu_id == FSUID && h.Block_5A_3 == casualtyHouseholdId)
                .FirstOrDefaultAsync();

            if (casualtyHousehold == null)
                return null;

            // Step 2: Get all eligible households
            var filteredData = await _database.Table<Tbl_Sch_0_0_Block_7>()
                .Where(h => h.fsu_id == FSUID
                    && (h.is_selected_travel == null || h.is_selected_travel == false)
                    && (h.is_casualty_travel == null || h.is_casualty_travel == false)
                    && h.is_household == true)
                .ToListAsync();

            // Step 3: Try to find the next higher serial number
            var nextHousehold = filteredData
                .Where(h => h.Block_5A_3 > casualtyHousehold.Block_5A_3)
                .OrderBy(h => h.Block_5A_3)
                .FirstOrDefault();

            // Step 4: If not found, try to find the previous lower serial number
            if (nextHousehold == null)
            {
                nextHousehold = filteredData
                    .Where(h => h.Block_5A_3 < casualtyHousehold.Block_5A_3)
                    .OrderByDescending(h => h.Block_5A_3)
                    .FirstOrDefault();
            }

            return nextHousehold;
        }

        public async Task<int?> SaveUpdateSCH0Block5(Tbl_Sch_0_0_Block_7 tbl_Sch_0_0_Block_5)
        {
            try
            {
                if (tbl_Sch_0_0_Block_5 != null)
                {

                    var check_existence = await _database.Table<Tbl_Sch_0_0_Block_7>().Where(x => x.id == tbl_Sch_0_0_Block_5.id).ToListAsync();
                    if (check_existence != null)
                    {
                        await _database.UpdateAsync(tbl_Sch_0_0_Block_5);
                    }
                    else
                    {
                        await _database.InsertAsync(tbl_Sch_0_0_Block_5);
                    }

                    return 1;
                }
                else
                {
                    toastService.ShowError($"Getting Error While storing data!");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 5: {ex.Message}");
                return null;
            }
        }

        // Added By testing 16/6/25
        public Task<Tbl_Sch_0_0_Block_2_2> GetSelectedSubUnit(string fsuID)
        {
            var fsuId = Convert.ToInt32(fsuID);
            return _database.FindAsync<Tbl_Sch_0_0_Block_2_2>(x => x.fsu_id == fsuId && x.IsChecked == true);
        }

        // Added By testing 16/6/25
        public Task<List<Tbl_Sch_0_0_Block_7>> Get_SCH0_0_Block_5A_HouseHoldBy_FSUP(int fsu_id)
        {
            return _database.Table<Tbl_Sch_0_0_Block_7>()
                            .Where(x => x.fsu_id == fsu_id && x.is_household)
                            .ToListAsync();
        }

        //Mark casualty
        public async Task<int> MarkCasualty(int casualtyHouseholdId)
        {
            // Fetch the casualty household
            var casualtyHousehold = await _database.Table<Tbl_Sch_0_0_Block_7>()
                .Where(h => h.fsu_id == SessionStorage.SelectedFSUId && h.Block_5A_3 == casualtyHouseholdId)
                .FirstOrDefaultAsync();

            // If the household is found, mark it as a casualty
            if (casualtyHousehold != null)
            {
                casualtyHousehold.is_casualty_travel = true;
                casualtyHousehold.status_travel = "CASUALTY";
                // Update the household and return the number of affected rows
                int result = await _database.UpdateAsync(casualtyHousehold);
                return result;
            }

            // If the household is not found, return 0 to indicate no rows were updated
            return 0;
        }
        public Task<int> Update_SCH0_0_Block_5A(Tbl_Sch_0_0_Block_7 data)
        {
            return _database.UpdateAsync(data);
        }


        public async Task<List<Tbl_Sch_0_0_Block_7>> GetSelectedBlock5List(int Fsu_id)
        {
            try
            {
                List<Tbl_Sch_0_0_Block_7>? data_set = await _database.Table<Tbl_Sch_0_0_Block_7>().Where(x => x.fsu_id == Fsu_id && x.is_selected_travel == true).ToListAsync();
                return data_set;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Mark Orginal
        public async Task<int> MarkOriginalToCasualty(int casualtyHouseholdId)
        {
            try
            {
                // Fetch the casualty household
                var casualtyHousehold = await _database.Table<Tbl_Sch_0_0_Block_7>()
                    .Where(h => h.fsu_id == SessionStorage.SelectedFSUId && h.Block_5A_3 == casualtyHouseholdId)
                    .FirstOrDefaultAsync();

                // If the household is found, mark it as a casualty
                if (casualtyHousehold != null && casualtyHousehold.is_casualty_travel == true && casualtyHousehold.status_travel == "CASUALTY")
                {
                    casualtyHousehold.is_casualty_travel = false;
                    casualtyHousehold.status_travel = string.Empty;
                    // Update the household and return the number of affected rows
                    int result = await _database.UpdateAsync(casualtyHousehold);
                    return result;
                }

                // If the household is not found, return 0 to indicate no rows were updated
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> Update_SCH0_0_Block_5AHHD_Status(int status, int hhd_id, int fsu_id)
        {
            var query = "UPDATE Tbl_Sch_0_0_Block_5 SET hhdStatus = ? WHERE Block_5A_3 = ? AND fsu_id = ?";
            return await _database.ExecuteAsync(query, status, hhd_id, fsu_id);
        }
        public async Task<int> Update_SCH0_0_Block_5Download_Status(int status, int hhd_id, int fsu_id)
        {
            var query = "UPDATE Tbl_Sch_0_0_Block_5 SET needDownload = ? WHERE Block_5A_3 = ? AND fsu_id = ?";
            return await _database.ExecuteAsync(query, status, hhd_id, fsu_id);
        }

    }
}
