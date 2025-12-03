using Blazored.Toast.Services;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Income.Common;
using Income.Common.HIS2026;
using Income.Database.Models.Common;
using Income.Database.Models.HIS_2026;
using Income.Database.Models.SCH0_0;
using Income.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Income.Database.Queries
{
    public class DBQueries : Database
    {
        ToastService toastService = new();

        public async Task<int> SaveReceivedDataAsync(Submission_Model data)
        {
            try
            {
                if (data != null)
                {
                    var sch00_block_0_1 = data.IncomeSch00block0;
                    if (sch00_block_0_1 != null)
                    {
                        await SaveBlock1(sch00_block_0_1);
                    }

                    var sch00_block_2_1 = data.IncomeSch00block2_1;
                    if (sch00_block_2_1 != null && sch00_block_2_1.Count > 0)
                    {
                        await SaveSCH0Block2_1(sch00_block_2_1);
                    }

                    var sch00_block_2_2 = data.IncomeSch00block2_2;
                    if (sch00_block_2_2 != null && sch00_block_2_2.Count > 0)
                    {
                        await SaveSCH0Block2_2(sch00_block_2_2);
                    }

                    var sch00_block_3 = data.IncomeSch00block3;
                    if (sch00_block_3 != null && sch00_block_3.Count > 0)
                    {
                        await SaveFileData(sch00_block_3);
                    }

                    var sch00_block_4 = data.IncomeSch00block_4;
                    if (sch00_block_4 != null)
                    {
                        await SaveBlock4(sch00_block_4);
                    }

                    var sch00_block_5 = data.IncomeSch00block_5;
                    if (sch00_block_5 != null && sch00_block_5.Count > 0)
                    {
                        await SaveSCH0Block5(sch00_block_5);
                    }

                    var sch00_block_7 = data.IncomeSch00block_7;
                    if (sch00_block_7 != null && sch00_block_7.Count > 0)
                    {
                        foreach (var item in sch00_block_7)
                        {
                            await SaveUpdateSCH0Block7(item);
                        }
                    }

                    var sch00_field_op = data.IncomeSch00FieldOp;
                    if (sch00_field_op != null)
                    {
                        await SaveBlock2(sch00_field_op);
                    }

                    var sch00_warnings = data.IncomeSch00WarningList;
                    if (sch00_warnings != null)
                    {
                        foreach (var item in sch00_warnings)
                        {
                            var existing_warning = await _database.Table<Tbl_Warning>().Where(x => x.id == item.id).ToListAsync();
                            if (existing_warning != null && existing_warning.Count > 0)
                            {
                                await _database.UpdateAsync(item);
                            }
                            else
                            {
                                await _database.InsertAsync(item);
                            }
                        }
                    }

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> HardDeleteDataForFSUID(int fsuID)
        {
            try
            {
                await _database.ExecuteAsync($"DELETE FROM Tbl_Sch_0_0_Block_0_1 WHERE fsu_id = {fsuID} AND tenant_id = {SessionStorage.tenant_id}");
                await _database.ExecuteAsync($"DELETE FROM Tbl_Sch_0_0_Block_2_1 WHERE fsu_id = {fsuID} AND tenant_id = {SessionStorage.tenant_id}");
                await _database.ExecuteAsync($"DELETE FROM Tbl_Sch_0_0_Block_2_2 WHERE fsu_id = {fsuID} AND tenant_id = {SessionStorage.tenant_id}");
                await _database.ExecuteAsync($"DELETE FROM Tbl_Sch_0_0_Block_3 WHERE fsu_id = {fsuID} AND tenant_id = {SessionStorage.tenant_id}");
                await _database.ExecuteAsync($"DELETE FROM Tbl_Sch_0_0_Block_4 WHERE fsu_id = {fsuID} AND tenant_id = {SessionStorage.tenant_id}");
                await _database.ExecuteAsync($"DELETE FROM Tbl_Sch_0_0_Block_5 WHERE fsu_id = {fsuID} AND tenant_id = {SessionStorage.tenant_id}");
                await _database.ExecuteAsync($"DELETE FROM Tbl_Sch_0_0_Block_7 WHERE fsu_id = {fsuID} AND tenant_id = {SessionStorage.tenant_id}");
                await _database.ExecuteAsync($"DELETE FROM Tbl_Sch_0_0_FieldOperation WHERE fsu_id = {fsuID} AND tenant_id = {SessionStorage.tenant_id}");
                await _database.ExecuteAsync($"DELETE FROM Tbl_Warning WHERE fsu_id = {fsuID} AND tenant_id = {SessionStorage.tenant_id}");

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DoUpdateListing(int fsuID, int option)
        {
            try
            {
                if(SessionStorage.FSU_Submitted)
                {
                    //Should we soft-delete or just delete
                    //1 - nukes all entries in all tables related to the FSU
                    // Perform delete operations for all relevant tables
                    if (option == 1)
                    {
                        await _database.Table<Tbl_Sch_0_0_Block_0_1>().DeleteAsync(x => x.fsu_id == fsuID);
                    }

                    if (option == 1 || option == 2)
                    {
                        await _database.Table<Tbl_Sch_0_0_Block_2_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_2_2>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_3>().DeleteAsync(x => x.fsu_id == fsuID && x.is_sub_unit == true);

                    }

                    if (option == 1 || option == 2 || option == 3)
                    {
                        await _database.Table<Tbl_Block_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_3>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_4>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_4_Q5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_6>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7a_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c_NIC>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c_Q10>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7d>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_8>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_8_Q6>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_9a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_9b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_A>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_B>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_10>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_11a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_11b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_FieldOperation>().DeleteAsync(x => x.fsu_id == fsuID);


                        await _database.Table<Tbl_Sch_0_0_FieldOperation>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_3>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_7>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Warning>().DeleteAsync(x => x.fsu_id == fsuID);
                        var fsuRecord = await _database.Table<Tbl_Fsu_List>().Where(fsu => fsu.fsu_id == fsuID).FirstOrDefaultAsync();
                        if (fsuRecord != null)
                        {
                            // Update the IsReSelectionEnabled property
                            fsuRecord.is_selection_done = false;
                            // Update the record in the database
                            await _database.UpdateAsync(fsuRecord);
                        }
                    }
                    if (option == 4)
                    {
                        await _database.Table<Tbl_Block_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_3>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_4>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_4_Q5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_6>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7a_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c_NIC>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c_Q10>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7d>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_8>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_8_Q6>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_9a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_9b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_A>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_B>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_10>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_11a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_11b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_FieldOperation>().DeleteAsync(x => x.fsu_id == fsuID);
                        var fsuRecord = await _database.Table<Tbl_Fsu_List>().Where(fsu => fsu.fsu_id == fsuID).FirstOrDefaultAsync();
                        if (fsuRecord != null)
                        {
                            // Update the IsReSelectionEnabled property
                            fsuRecord.is_selection_done = false;
                            // Update the record in the database
                            await _database.UpdateAsync(fsuRecord);
                        }
                    }


                    // If all operations succeed, return 1
                    return 1;
                }
                else
                {
                    //1 - nukes all entries in all tables related to the FSU
                    // Perform delete operations for all relevant tables
                    if (option == 1)
                    {
                        await _database.Table<Tbl_Sch_0_0_Block_0_1>().DeleteAsync(x => x.fsu_id == fsuID);
                    }

                    if (option == 1 || option == 2)
                    {
                        await _database.Table<Tbl_Sch_0_0_Block_2_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_2_2>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_3>().DeleteAsync(x => x.fsu_id == fsuID && x.is_sub_unit == true);

                    }

                    if (option == 1 || option == 2 || option == 3)
                    {
                        await _database.Table<Tbl_Block_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_3>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_4>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_4_Q5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_6>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7a_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c_NIC>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c_Q10>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7d>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_8>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_8_Q6>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_9a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_9b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_A>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_B>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_10>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_11a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_11b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_FieldOperation>().DeleteAsync(x => x.fsu_id == fsuID);


                        await _database.Table<Tbl_Sch_0_0_FieldOperation>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_3>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_7>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Sch_0_0_Block_5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Warning>().DeleteAsync(x => x.fsu_id == fsuID);
                        var fsuRecord = await _database.Table<Tbl_Fsu_List>().Where(fsu => fsu.fsu_id == fsuID).FirstOrDefaultAsync();
                        if (fsuRecord != null)
                        {
                            // Update the IsReSelectionEnabled property
                            fsuRecord.is_selection_done = false;
                            // Update the record in the database
                            await _database.UpdateAsync(fsuRecord);
                        }
                    }
                    if (option == 4)
                    {
                        await _database.Table<Tbl_Block_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_3>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_4>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_4_Q5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_5>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_6>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7a_1>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c_NIC>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7c_Q10>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_7d>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_8>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_8_Q6>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_9a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_9b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_A>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_B>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_10>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_11a>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_11b>().DeleteAsync(x => x.fsu_id == fsuID);
                        await _database.Table<Tbl_Block_FieldOperation>().DeleteAsync(x => x.fsu_id == fsuID);
                        var fsuRecord = await _database.Table<Tbl_Fsu_List>().Where(fsu => fsu.fsu_id == fsuID).FirstOrDefaultAsync();
                        if (fsuRecord != null)
                        {
                            // Update the IsReSelectionEnabled property
                            fsuRecord.is_selection_done = false;
                            // Update the record in the database
                            await _database.UpdateAsync(fsuRecord);
                        }
                    }


                    // If all operations succeed, return 1
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

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

        public async Task<List<Tbl_Sch_0_0_Block_2_2>?> FetchSCH0Block2_2Data(int fsu_id = 0)
        {
            try
            {
                fsu_id = fsu_id == 0 ? SessionStorage.SelectedFSUId : fsu_id;
                List<Tbl_Sch_0_0_Block_2_2> data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_2_2>("SELECT * FROM Tbl_Sch_0_0_Block_2_2 WHERE fsu_id = ?", fsu_id);
                return data_set;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Getting Error fetching data! {ex}");
                return null;
            }
        }

        public async Task<int?> SaveBlock4(Tbl_Sch_0_0_Block_4 tbl_Sch_0_0_Block_4)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Sch_0_0_Block_4>().Where(x => x.id == tbl_Sch_0_0_Block_4.id).ToListAsync();
                if (check_existence != null && check_existence.Count > 0)
                {
                    status = await _database.UpdateAsync(tbl_Sch_0_0_Block_4);
                }
                else
                {
                    tbl_Sch_0_0_Block_4.id = Guid.NewGuid();
                    status = await _database.InsertAsync(tbl_Sch_0_0_Block_4);
                }
                return status;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 4: {ex.Message}");
                return null;
            }
        }

        public async Task<Tbl_Sch_0_0_Block_4?> GetBlock4()
        {
            try
            {
                var response = await _database.QueryAsync<Tbl_Sch_0_0_Block_4>("SELECT * FROM Tbl_Sch_0_0_Block_4 WHERE fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
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

        public async Task<int?> SaveSCH0Block5(List<Tbl_Sch_0_0_Block_5> tbl_Sch_0_0_Block_5)
        {
            try
            {
                if (tbl_Sch_0_0_Block_5 != null && tbl_Sch_0_0_Block_5.Count > 0)
                {
                    foreach (var item in tbl_Sch_0_0_Block_5)
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
                toastService.ShowError($"Error While saving SCH 0 Block 5: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Tbl_Sch_0_0_Block_5>?> FetchSCH0Block5Data()
        {
            try
            {
                List<Tbl_Sch_0_0_Block_5> data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_5>("SELECT * FROM Tbl_Sch_0_0_Block_5 WHERE fsu_id = ?", SessionStorage.SelectedFSUId);
                return data_set;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Getting Error fetching data! {ex}");
                return null;
            }
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
                    tbl_Sch_0_0_Block_0_1.id = Guid.NewGuid();
                    tbl_Sch_0_0_Block_0_1.survey_timestamp = DateTime.Now;
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
                var response = await _database.QueryAsync<Tbl_Sch_0_0_FieldOperation>("SELECT * FROM Tbl_Sch_0_0_FieldOperation WHERE fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
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

        public async Task<int?> SaveBlock2(Tbl_Sch_0_0_FieldOperation tbl_Sch_0_0_FieldOperation)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Sch_0_0_FieldOperation>().Where(x => x.id == tbl_Sch_0_0_FieldOperation.id).ToListAsync();
                if (check_existence != null && check_existence.Count > 0)
                {
                    status = await _database.UpdateAsync(tbl_Sch_0_0_FieldOperation);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_Sch_0_0_FieldOperation);
                }
                return status;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 2: {ex.Message}");
                return null;
            }
        }


        public async Task<Tbl_Sch_0_0_Block_7> GetBlock7DataByHHD(int hhd_id = 0)
        {
            try
            {
                List<Tbl_Sch_0_0_Block_7>? data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_7>("SELECT * FROM Tbl_Sch_0_0_Block_7 WHERE fsu_id = ? AND tenant_id = ? AND Block_5A_3 = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id, SessionStorage.selected_hhd_id);
                return data_set != null && data_set.Count > 0 ? data_set.FirstOrDefault() : new();
            }
            catch (Exception ex)
            {
                return new Tbl_Sch_0_0_Block_7();
            }
        }

        public async Task<List<Tbl_Sch_0_0_Block_7>> GetBlock7Data(int fsuID = 0)
        {
            try
            {
                List<Tbl_Sch_0_0_Block_7>? data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_7>("SELECT * FROM Tbl_Sch_0_0_Block_7 WHERE fsu_id = ? AND tenant_id = ? AND (is_deleted IS NULL OR is_deleted = 0)", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                return data_set != null && data_set.Count > 0 ? data_set : new();
            }
            catch (Exception ex)
            {
                return new List<Tbl_Sch_0_0_Block_7>();
            }
        }

        public async Task<List<Tbl_Sch_0_0_Block_7>> GetBlock7DataWithDeleted()
        {
            try
            {
                List<Tbl_Sch_0_0_Block_7>? data_set = await _database.QueryAsync<Tbl_Sch_0_0_Block_7>("SELECT * FROM Tbl_Sch_0_0_Block_7 WHERE fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                return data_set != null && data_set.Count > 0 ? data_set : new();
            }
            catch (Exception ex)
            {
                return new List<Tbl_Sch_0_0_Block_7>();
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
                        await _database.QueryAsync<Tbl_Sch_0_0_Block_3>("DELETE FROM Tbl_Sch_0_0_Block_3 WHERE block_name = ? AND is_deleted = false AND fsu_id = ? AND tenant_id = ?", file.block_name, SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
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

        public async Task<Tbl_Sch_0_0_Block_3?> GetFileData(int is_su)
        {
            try
            {
                var check_existence = await _database.QueryAsync<Tbl_Sch_0_0_Block_3>("SELECT * FROM Tbl_Sch_0_0_Block_3 WHERE is_sub_unit = ? AND is_deleted = false AND fsu_id = ? AND tenant_id = ?", is_su, SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
                if (check_existence.Count == 0)
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
            return _database.Table<Tbl_Sch_0_0_Block_7>().Where(x => x.fsu_id == FSUID && x.Block_7_3 == ID).FirstOrDefaultAsync();
        }

        //find a household with a higher and smaller serial number
        public async Task<Tbl_Sch_0_0_Block_7?> FetchHouseholdWithHigherSerialNumber(int FSUID, int casualtyHouseholdId)
        {
            // Step 1: Get the casualty household
            var casualtyHousehold = await _database.Table<Tbl_Sch_0_0_Block_7>()
                .Where(h => h.fsu_id == FSUID && h.Block_7_3 == casualtyHouseholdId)
                .FirstOrDefaultAsync();

            if (casualtyHousehold == null)
                return null;

            // Step 2: Get all eligible households
            var filteredData = await _database.Table<Tbl_Sch_0_0_Block_7>()
                .Where(h => h.fsu_id == FSUID
                    && (h.isSelected == null || h.isSelected == false)
                    && (h.isCasualty == null || h.isCasualty == false)
                    && h.is_household == 1)
                .ToListAsync();

            // Step 3: Try to find the next higher serial number
            var nextHousehold = filteredData
                .Where(h => h.Block_7_3 > casualtyHousehold.Block_7_3)
                .OrderBy(h => h.Block_7_3)
                .FirstOrDefault();

            // Step 4: If not found, try to find the previous lower serial number
            if (nextHousehold == null)
            {
                nextHousehold = filteredData
                    .Where(h => h.Block_7_3 < casualtyHousehold.Block_7_3)
                    .OrderByDescending(h => h.Block_7_3)
                    .FirstOrDefault();
            }

            return nextHousehold;
        }

        public async Task<int?> SaveUpdateSCH0Block7(Tbl_Sch_0_0_Block_7 tbl_Sch_0_0_Block_5)
        {
            try
            {
                if (tbl_Sch_0_0_Block_5 != null)
                {

                    var check_existence = await _database.Table<Tbl_Sch_0_0_Block_7>().Where(x => x.id == tbl_Sch_0_0_Block_5.id).ToListAsync();
                    if (check_existence != null && check_existence.Count > 0)
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

        public async Task<int> DeleteHHD(Guid id)
        {
            try
            {
                var exists = await _database.Table<Tbl_Sch_0_0_Block_7>().Where(x => x.id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        exists.is_deleted = true;
                        await _database.UpdateAsync(exists);
                    }
                    else
                    {
                        int deleted = await _database.DeleteAsync(exists);
                    }
                    await ReserializeHHD();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting household: " + ex.Message);
                return 0;
            }
        }

        public async Task ReserializeHHD()
        {
            try
            {
                var households = await GetBlock7Data(SessionStorage.SelectedFSUId);
                if (households != null && households.Count > 0)
                {
                    //1. Re-serialize Block_7_1 for all records after the deleted entry
                    // Order by current serial
                    var allOrdered = households.OrderBy(h => h.Block_7_1).ToList();

                    int nextSerial = 1;
                    foreach (var h in allOrdered)
                    {
                        h.Block_7_1 = nextSerial++;
                    }

                    int hhdSrl = 1;

                    foreach(var hhd in allOrdered)
                    {
                        if(hhd.is_household == 2)
                        {
                            hhd.Block_7_3 = hhdSrl++;
                        }
                        else
                        {
                            hhd.Block_7_3 = 0;
                        }
                    }

                    foreach (var hhd in households)
                    {
                        await _database.UpdateAsync(hhd);
                    }
                }
            }
            catch
            {

            }
        }

        public async Task<int> DeleteAllHHDS()
        {
            try
            {
                var exists = await _database.Table<Tbl_Sch_0_0_Block_7>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId).ToListAsync();
                if (exists != null && exists.Count > 0)
                {
                    foreach (var item in exists)
                    {
                        int deleted = await _database.DeleteAsync(item);
                    }

                }
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting household: " + ex.Message);
                return 0;
            }
        }

        public async Task<int> SaveSSSToDatabase(List<Tbl_Sch_0_0_Block_7> households)
        {
            if (households == null || households.Count == 0)
                return 0;

            try
            {

                // Begin transaction
                await _database.RunInTransactionAsync(tran =>
                {
                    foreach (var item in households)
                    {
                        // Update each household in the database
                        tran.Execute("UPDATE Tbl_Sch_0_0_Block_7 SET Stratum = ?, SSS = ?, a = ?, b = ? WHERE id = ?",
             item.Stratum, item.SSS, item.a, item.b, item.id);
                    }
                });

                Console.WriteLine("All SSS values updated successfully!");
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating SSS values: " + ex.Message);
                return 0;
            }
        }

        public async Task<int> ResetSelection()
        {
            try
            {
                var response = await GetBlock7Data(SessionStorage.SelectedFSUId);
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        item.isSelected = false;
                        item.isInitialySelected = false;
                        item.isCasualty = false;
                        item.isSubstitute = false;
                        item.SubstitutedForID = null;
                        item.OriginalHouseholdID = null;
                        item.SubstitutionCount = 0;
                        item.status = "";
                        item.SSS = 0;
                        item.Stratum = 0;
                        item.a = 0;
                        item.b = 0;
                    }
                    var update = await _database.UpdateAllAsync(response);
                    SessionStorage.selection_done = false;
                    return update;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error resetting selection: " + ex.Message);
                return 0;
            }
        }


        public async Task<int?> SaveBulkBlock7(List<Tbl_Sch_0_0_Block_7> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {

                    var insert = await _database.InsertAllAsync(list);

                    return insert;
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
                            .Where(x => x.fsu_id == fsu_id && x.is_household == 2 && (x.is_deleted == null || x.is_deleted == false))
                            .ToListAsync();
        }

        public async Task<int> GetCurrentHHDStatus(int fsuID, int hhd_id)
        {
            try
            {
                var hhd = await _database.Table<Tbl_Sch_0_0_Block_7>()
                    .Where(x => x.fsu_id == fsuID && x.Block_7_3 == hhd_id)
                    .FirstOrDefaultAsync();
                if (hhd != null)
                {
                    return hhd.hhdStatus ?? 0;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {

            }
        }

        public async Task<Tbl_Sch_0_0_Block_7?> GetCurrentHHD(int fsuID, int hhd_id)
        {
            try
            {
                var hhd = await _database.Table<Tbl_Sch_0_0_Block_7>()
                    .Where(x => x.fsu_id == fsuID && x.Block_7_3 == hhd_id)
                    .FirstOrDefaultAsync();
                if (hhd != null)
                {
                    return hhd;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        //Mark casualty
        public async Task<int> MarkCasualty(int casualtyHouseholdId)
        {
            // Fetch the casualty household
            var casualtyHousehold = await _database.Table<Tbl_Sch_0_0_Block_7>()
                .Where(h => h.fsu_id == SessionStorage.SelectedFSUId && h.Block_7_3 == casualtyHouseholdId)
                .FirstOrDefaultAsync();

            // If the household is found, mark it as a casualty
            if (casualtyHousehold != null)
            {
                casualtyHousehold.isCasualty = true;
                casualtyHousehold.status = "CASUALTY";
                // Update the household and return the number of affected rows
                int result = await _database.UpdateAsync(casualtyHousehold);
                return result;
            }

            // If the household is not found, return 0 to indicate no rows were updated
            return 0;
        }
        public Task<int> Update_SCH0_0_Block_7(Tbl_Sch_0_0_Block_7 data)
        {
            return _database.UpdateAsync(data);
        }

        public async Task AssignSSSHouseholdIds()
        {
            // Group by SSS
            try
            {
                var items = await GetBlock7Data(SessionStorage.SelectedFSUId);
                if (items != null && items.Count > 0)
                {
                    var groups = items
                        .Where(x => x.isInitialySelected == true)
                        .GroupBy(x => x.SSS);

                    foreach (var group in groups)
                    {
                        int serial = 1;

                        // Order by Block_7_3 and assign serial
                        foreach (var item in group.OrderBy(x => x.Block_7_3))
                        {
                            item.SSS_household_id = serial++;
                            await _database.UpdateAsync(item);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {

            }
        }



        public async Task<List<Tbl_Sch_0_0_Block_7>> GetSelectedBlock7List(int Fsu_id)
        {
            try
            {
                List<Tbl_Sch_0_0_Block_7>? data_set = await _database.Table<Tbl_Sch_0_0_Block_7>().Where(x => x.fsu_id == Fsu_id && x.isSelected == true).ToListAsync();
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
                    .Where(h => h.fsu_id == SessionStorage.SelectedFSUId && h.Block_7_3 == casualtyHouseholdId)
                    .FirstOrDefaultAsync();

                // If the household is found, mark it as a casualty
                if (casualtyHousehold != null && casualtyHousehold.isCasualty == true && casualtyHousehold.status == "CASUALTY")
                {
                    casualtyHousehold.isCasualty = false;
                    casualtyHousehold.status = string.Empty;
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

        public async Task<int> UpdateAllRowsForColumnAsync(int fsu_id)
        {
            // Retrieve rows from the table where fsu_id matches the provided value
            var rows = await _database.Table<Tbl_Sch_0_0_Block_7>()
                                      .Where(x => x.fsu_id == fsu_id) // Filter by fsu_id
                                      .ToListAsync();

            // Iterate over each row and update the specific column
            foreach (var row in rows)
            {
                row.isSelected = false;  // Update the 'isSelected' property
                row.isCasualty = false;
                row.isSubstitute = false;
                row.SubstitutedForID = null;
                row.isInitialySelected = false;
                row.OriginalHouseholdID = null;
                row.SubstitutionCount = 0;
                row.status = "";
                row.hhdStatus = 0;
                row.needDownload = 0;
                row.SelectedFromSSS = null;
                row.SelectedPostedSSS = null;
            }
            // Use UpdateAllAsync to update all records in the table
            return await _database.UpdateAllAsync(rows);
        }

        public async Task<List<Tbl_Comments>> GetItemsAsync(string block)
        {
            try
            {
                var response = await _database.QueryAsync<Tbl_Comments>("SELECT * FROM Tbl_Comments WHERE fsu_id = ? AND hhd_id = ? AND tenant_id = ? AND survey_id = ? AND block = ? AND (parent_comment_id IS NULL OR parent_comment_id == ?)", SessionStorage.SelectedFSUId, SessionStorage.selected_hhd_id, SessionStorage.tenant_id, SessionStorage.surveyId, block, Guid.Empty.ToString());
                if (response != null && response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Tbl_Comments> GetItemsAsyncById(Tbl_Comments data)
        {
            try
            {
                var response = await _database.QueryAsync<Tbl_Comments>("SELECT * FROM Tbl_Comments WHERE fsu_id = ? AND hhd_id = ? AND tenant_id = ? AND survey_id = ? AND block = ? AND id = ?", SessionStorage.SelectedFSUId, SessionStorage.selected_hhd_id, SessionStorage.tenant_id, SessionStorage.surveyId, data.block, data.id);
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
                return null;
            }
        }

        public async Task<int> SaveBlockTbl_Comments(Tbl_Comments Comments)
        {
            try
            {
                int status = 0;
                var check_existance = await _database.Table<Tbl_Comments>().Where(x => x.id == Comments.id).ToListAsync();
                if (check_existance != null && check_existance.Count > 0)
                {
                    status = await _database.UpdateAsync(Comments);
                }
                else
                {
                    status = await _database.InsertAsync(Comments);
                }
                return status;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<int> DeleteComments(Tbl_Comments Comments)
        {
            try
            {
                var data = await _database.DeleteAsync(Comments);
                return data;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //HIS BLOCK 1
        public async Task<int?> Save_SCH_HIS_Block1(Tbl_Block_1 tbl_block_1)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_1>().Where(x => x.id == tbl_block_1.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_1);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_1);
                    var hhd = await GetCurrentHHD(SessionStorage.SelectedFSUId, tbl_block_1.hhd_id.GetValueOrDefault());
                    if (hhd != null)
                    {
                        hhd.hhdStatus = hhd.status == "SUBSTITUTED" ? null : 11;
                        await Update_SCH0_0_Block_7(hhd);
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving SCH 0 Block 1: {ex.Message}");
                return null;
            }
        }

        public async Task<Tbl_Block_1?> Fetch_SCH_HIS_Block1(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While saving Block 1: {ex.Message}");
                return null;
            }
        }

        public async Task<int> DeleteHISBlock1(Guid id)
        {
            try
            {
                var exists = await _database.Table<Tbl_Block_1>().Where(x => x.id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        exists.is_deleted = true;
                        await _database.UpdateAsync(exists);
                    }
                    else
                    {
                        int deleted = await _database.DeleteAsync(exists);
                        return deleted;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting household: " + ex.Message);
                return 0;
            }
        }

        //HIS Block 3
        public async Task<List<Tbl_Block_3>> Fetch_SCH_HIS_Block3(int hhd_id, int tenant_id = 1)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_3>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && x.tenant_id == tenant_id && (x.is_deleted == null || x.is_deleted == false)).ToListAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return new List<Tbl_Block_3>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving Block 3: {ex.Message}");
                return new List<Tbl_Block_3>();
            }
        }

        public async Task<int?> Save_SCH_HIS_Block3(Tbl_Block_3 tbl_block_3)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_3>().Where(x => x.id == tbl_block_3.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_3);
                    await UpdateOrDeleteDependentBlocks_HIS_Block_3(tbl_block_3);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_3);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 3: {ex.Message}");
                return null;
            }
        }

        public async Task<int> DeleteHISBlock3(Guid id)
        {
            try
            {
                var exists = await _database.Table<Tbl_Block_3>().Where(x => x.id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        exists.is_deleted = true;
                        await _database.UpdateAsync(exists);
                    }
                    else
                    {
                        int deleted = await _database.DeleteAsync(exists);
                    }
                    var json = System.Text.Json.JsonSerializer.Serialize(exists);

                    //await _logger.LogInfo($"Deleted member - \n{json}");
                    await ReserializeMemberList(exists);
                    await UpdateOrDeleteDependentBlocks_HIS_Block_3(exists);
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting household member: " + ex.Message);
                return 0;
            }
        }

        public async Task UpdateOrDeleteDependentBlocks_HIS_Block_3(Tbl_Block_3 block_3)
        {
            try
            {
                //HIS_Block_4
                //HIS_Block_5 - if entry having 04 in items 8 - 11 gets deleted, delete the entry from block-5 too
                if(block_3.item_8 != 4 && block_3.item_9 != 4 && block_3.item_10 != 4 && block_3.item_11 != 4)
                {
                    var existingEntryBlock5 = await _database.Table<Tbl_Block_5>().Where(x => x.fk_block_3 == block_3.id).FirstOrDefaultAsync();
                    if (existingEntryBlock5 != null)
                    {
                        await Delete_HIS_Block_5(existingEntryBlock5.id);
                    }
                }
                //HIS_Block_6 - if entry having 05 in items 8 - 9 gets deleted
                if (block_3.item_8 != 5 && block_3.item_9 != 5)
                {
                    var existingEntryBlock6 = await _database.Table<Tbl_Block_6>().Where(x => x.fk_block_3 == block_3.id).FirstOrDefaultAsync();
                    if (existingEntryBlock6 != null)
                    {
                        await Delete_HIS_Block_6(existingEntryBlock6.id);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async Task ReserializeMemberList(Tbl_Block_3 deleted)
        {
            try
            {
                var members = await Fetch_SCH_HIS_Block3(SessionStorage.selected_hhd_id);
                if (members != null && members.Count > 0)
                {
                    //1. Re-serialize Block_7_1 for all records after the deleted entry
                    // Order by current serial
                    var allOrdered = members.OrderBy(h => h.serial_no).ToList();

                    // Identify deleted serial position
                    int deletedSerial1 = deleted.serial_no ?? 0;

                    // Start from the next serial
                    int nextSerial = deletedSerial1;

                    foreach (var h in allOrdered.Where(h => h.serial_no > deletedSerial1))
                    {
                        nextSerial++;
                        h.serial_no = nextSerial;
                    }

                    foreach (var member in members)
                    {
                        await _database.UpdateAsync(member);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        //HIS Block 4

        public async Task<List<Tbl_Block_4_Q5>> Fetch_SCH_HIS_Block4_NICList()
        {
            try
            {
                var response = await _database.Table<Tbl_Block_4_Q5>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == SessionStorage.selected_hhd_id && (x.is_deleted == null || x.is_deleted == false)).ToListAsync();
                if (response != null && response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<Tbl_Block_4_Q5>();
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 4 NICLIST: {ex.Message}");
                return new List<Tbl_Block_4_Q5>();
            }
        }

        public async Task<Tbl_Block_4?> Fetch_SCH_HIS_Block4(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_4>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 4: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Save_SCH_HIS_Block4(Tbl_Block_4 tbl_block_4, bool has_item_4_changed = false)
        {
            try
            {
                int status;
                var check_existence = await _database.Table<Tbl_Block_4>().Where(x => x.id == tbl_block_4.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_4);
                    await SyncBlock7D(check_existence);

                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_4);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 4: {ex.Message}");
                return 0;
            }
        }

        public async Task SyncBlock7D(Tbl_Block_4 tbl_Block_4)
        {
            try
            {
                var Block7DList = await Fetch_SCH_HIS_Block7D() ?? new List<Tbl_Block_7d>();
                var toAdd = new List<Tbl_Block_7d>();
                int current_sl = Block7DList.Count;
                // Map Block 4 → Block 7D creation metadata
                var map = new[]
                {
                    new { Flag = tbl_Block_4.item_4_3, Code = 4003, Item1 = 2, Item2 = "animal husbandry" },
                    new { Flag = tbl_Block_4.item_4_4, Code = 4004, Item1 = 3, Item2 = "fisheries" },
                    new { Flag = tbl_Block_4.item_4_5, Code = 4005, Item1 = 4, Item2 = "agroforestry activity" },
                    new { Flag = tbl_Block_4.item_4_6, Code = 4006, Item1 = 5, Item2 = "others (bee keeping, sericulture, lac culture, ancillary etc.)" }
                };

                foreach (var row in map)
                {
                    var exists = Block7DList.FirstOrDefault(d => d.code == row.Code);

                    // ❌ CASE 1: Corresponding Block_4 flag is false → delete existing entry
                    if (!row.Flag.GetValueOrDefault())
                    {
                        if (exists != null)
                            await Delete_SCH_HIS_Block7D_List(exists);

                        continue; // move to next row
                    }

                    // ✔️ CASE 2: Flag is true → create entry if missing
                    if (exists == null)
                    {
                        var newEntry = new Tbl_Block_7d
                        {
                            id = Guid.NewGuid(),
                            hhd_id = tbl_Block_4.hhd_id,
                            serial_number = current_sl++,
                            code = row.Code,
                            item_1 = row.Item1,
                            item_2 = row.Item2,
                        };

                        toAdd.Add(newEntry);
                    }
                }

                // Save new entries
                foreach (var item in toAdd)
                {
                    await Save_SCH_HIS_Block7D_List(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While syncing Block 7D: {ex.Message}");
            }
        }

        public async Task<int?> Save_SCH_HIS_Block4_NICList(Tbl_Block_4_Q5 obj)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_4_Q5>().Where(x => x.id == obj.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(obj);
                }
                else
                {
                    status = await _database.InsertAsync(obj);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 4: {ex.Message}");
                return null;
            }
        }

        public async Task<int> DeleteBlock4_NICList(Guid id)
        {
            try
            {
                var exists = await _database.Table<Tbl_Block_4_Q5>().Where(x => x.id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        exists.is_deleted = true;
                        await _database.UpdateAsync(exists);
                    }
                    else
                    {
                        int deleted = await _database.DeleteAsync(exists);
                    }
                    await ReserializeNICList();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting item " + ex.Message);
                return 0;
            }
        }

        private async Task ReserializeNICList()
        {
            try
            {
                var items = await Fetch_SCH_HIS_Block4_NICList();
                if (items != null && items.Count > 0)
                {
                    int s = 1;
                    foreach (var item in items)
                    {
                        item.SerialNumber = s;
                        s++;
                        await _database.UpdateAsync(item);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task ReserializeBlock7DList()
        {
            try
            {
                var items = await Fetch_SCH_HIS_Block7D();
                if (items != null && items.Count > 0)
                {
                    int s = 1;
                    foreach (var item in items)
                    {
                        item.serial_number = s;
                        s++;
                        await _database.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task ReserializeHISBlock5()
        {
            try
            {
                var items = await Fetch_SCH_HIS_Block5(SessionStorage.selected_hhd_id);
                if (items != null && items.Count > 0)
                {
                    int s = 1;
                    foreach (var item in items)
                    {
                        item.serial_number = s;
                        s++;
                        await _database.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task ReserializeHISBlock6()
        {
            try
            {
                var items = await Fetch_SCH_HIS_Block6(SessionStorage.selected_hhd_id);
                if (items != null && items.Count > 0)
                {
                    int s = 1;
                    foreach (var item in items)
                    {
                        item.serial_number = s;
                        s++;
                        await _database.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        //HIS Block 5
        public async Task<List<Tbl_Block_5>> Fetch_SCH_HIS_Block5(int hhd_id, int tenant_id = 1)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_5>()
                    .Where(x =>
                        x.fsu_id == SessionStorage.SelectedFSUId &&
                        x.hhd_id == hhd_id &&
                        x.tenant_id == tenant_id &&
                        (x.is_deleted == null || x.is_deleted == false)
                    )
                    .ToListAsync();

                if (response != null)
                {
                    return response;
                }
                else
                {
                    return new List<Tbl_Block_5>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While fetching Block 5: {ex.Message}");
                return new List<Tbl_Block_5>();
            }
        }


        public async Task<int?> Save_SCH_HIS_Block5(Tbl_Block_5 tbl_block_5)
        {
            try
            {
                int status = new();

                var check_existence = await _database
                    .Table<Tbl_Block_5>()
                    .Where(x => x.id == tbl_block_5.id)
                    .FirstOrDefaultAsync();

                if (check_existence != null)
                {
                    // update existing row
                    tbl_block_5.isUpdated = true;
                    status = await _database.UpdateAsync(tbl_block_5);
                }
                else
                {
                    // insert new row
                    tbl_block_5.isUpdated = false;
                    status = await _database.InsertAsync(tbl_block_5);
                }

                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 5: {ex.Message}");
                return null;
            }
        }


        //Block_6
        public async Task<List<Tbl_Block_6>> Fetch_SCH_HIS_Block6(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_6>()
                    .Where(x => x.fsu_id == SessionStorage.SelectedFSUId
                                && x.hhd_id == hhd_id
                                && (x.is_deleted == null || x.is_deleted == false))
                    .ToListAsync();

                if (response != null)
                {
                    return response;
                }
                else
                {
                    return new List<Tbl_Block_6>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While fetching Block 6: {ex.Message}");
                return new List<Tbl_Block_6>();
            }
        }

        public async Task<int> Delete_HIS_Block_5(Guid id)
        {
            try
            {
                var exists = await _database.Table<Tbl_Block_5>().Where(x => x.id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        exists.is_deleted = true;
                        await _database.UpdateAsync(exists);
                    }
                    else
                    {
                        int deleted = await _database.DeleteAsync(exists);
                    }
                    string json = JsonSerializer.Serialize(exists);
                    //await _logger.LogInfo($"Deleted Block 5 entry - \n{json}");
                    await ReserializeHISBlock5();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting item " + ex.Message);
                return 0;
            }
        }

        public async Task<int?> Save_SCH_HIS_Block6(Tbl_Block_6 tbl_block_6)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_6>()
                    .Where(x => x.id == tbl_block_6.id)
                    .FirstOrDefaultAsync();

                if (check_existence != null)
                {
                    tbl_block_6.isUpdated = true;
                    status = await _database.UpdateAsync(tbl_block_6);
                }
                else
                {
                    tbl_block_6.isUpdated = false;
                    status = await _database.InsertAsync(tbl_block_6);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 6: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Delete_HIS_Block_6(Guid id)
        {
            try
            {
                var exists = await _database.Table<Tbl_Block_6>().Where(x => x.id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        exists.is_deleted = true;
                        await _database.UpdateAsync(exists);
                    }
                    else
                    {
                        int deleted = await _database.DeleteAsync(exists);
                    }
                    string json = JsonSerializer.Serialize(exists);
                    //await _logger.LogInfo($"Deleted Block 5 entry - \n{json}");
                    await ReserializeHISBlock6();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting item " + ex.Message);
                return 0;
            }
        }

        public async Task<Tbl_Block_7a?> Fetch_SCH_HIS_Block7A(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_7a>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 7A: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Save_SCH_HIS_Block7A(Tbl_Block_7a tbl_block_7a)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_7a>().Where(x => x.id == tbl_block_7a.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_7a);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_7a);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 7A: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<Tbl_Block_7a_1>> Fetch_SCH_HIS_Block7A_CodeList()
        {
            try
            {
                var response = await _database.Table<Tbl_Block_7a_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == SessionStorage.selected_hhd_id && (x.is_deleted == null || x.is_deleted == false)).ToListAsync();
                if (response != null && response.Count > 0)
                {
                    return response.OrderBy(x => x.serial_number).ToList();
                }
                else
                {
                    return new List<Tbl_Block_7a_1>();
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 7 code list: {ex.Message}");
                return new List<Tbl_Block_7a_1>();
            }
        }

        public async Task<int?> Save_SCH_HIS_Block7A_CodeList(Tbl_Block_7a_1 obj)
        {
            try
            {
                int status;
                var check_existence = await _database.Table<Tbl_Block_7a_1>().Where(x => x.id == obj.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(obj);
                    //Sync with Block 7D
                    var block_7d = await Fetch_SCH_HIS_Block7D();
                    if (block_7d != null && block_7d.Count > 0)
                    {
                        var matching_7d = block_7d.FirstOrDefault(x => x.block_7a_id == obj.id);
                        if (matching_7d != null)
                        {
                            matching_7d.code = obj.code;
                            matching_7d.item_2 = Block_7_1_Constants.CropCodes.FirstOrDefault(x => x.id == obj.code)?.title ?? "";
                            await Save_SCH_HIS_Block7D_List(matching_7d);
                        }
                    }
                }
                else
                {
                    status = await _database.InsertAsync(obj);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 7a code list: {ex.Message}");
                return null;
            }
        }

        public async Task<int> DeleteBlock7A_CodeList(Guid id)
        {
            try
            {
                var exists = await _database.Table<Tbl_Block_7a_1>().Where(x => x.id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        exists.is_deleted = true;
                        await _database.UpdateAsync(exists);
                    }
                    else
                    {
                        int deleted = await _database.DeleteAsync(exists);
                    }
                    var block_7d = await Fetch_SCH_HIS_Block7D();
                    if (block_7d != null)
                    {
                        var item = block_7d.FirstOrDefault(x => x.block_7c_id == id);
                        if (item != null)
                        {
                            _ = await Delete_SCH_HIS_Block7D_List(item);
                        }
                    }
                    await ReserializeCodeList();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting item " + ex.Message);
                return 0;
            }
        }

        private async Task ReserializeCodeList()
        {
            try
            {
                var items = await Fetch_SCH_HIS_Block7A_CodeList();
                if (items != null && items.Count > 0)
                {
                    int s = 1;
                    foreach (var item in items)
                    {
                        item.serial_number = s;
                        s++;
                        await _database.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<Tbl_Block_7b?> Fetch_SCH_HIS_Block7B(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_7b>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 7B: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Save_SCH_HIS_Block7B(Tbl_Block_7b tbl_block_7b)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_7b>().Where(x => x.id == tbl_block_7b.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_7b);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_7b);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 7B: {ex.Message}");
                return 0;
            }
        }

        public async Task<Tbl_Block_7c?> Fetch_SCH_HIS_Block7C(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_7c>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 7C: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Save_SCH_HIS_Block7C(Tbl_Block_7c tbl_block_7c)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_7c>().Where(x => x.id == tbl_block_7c.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_7c);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_7c);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 7C: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<Tbl_Block_7c_NIC>> Fetch_SCH_HIS_Block7_NICList()
        {
            try
            {
                var response = await _database.Table<Tbl_Block_7c_NIC>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == SessionStorage.selected_hhd_id && (x.is_deleted == null || x.is_deleted == false)).ToListAsync();
                if (response != null && response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<Tbl_Block_7c_NIC>();
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 7 NICLIST: {ex.Message}");
                return new List<Tbl_Block_7c_NIC>();
            }
        }

        public async Task<int?> Save_SCH_HIS_Block7_NICList(Tbl_Block_7c_NIC obj)
        {
            try
            {
                int status;
                var check_existence = await _database.Table<Tbl_Block_7c_NIC>().Where(x => x.id == obj.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(obj);
                    //Sync with Block 7D
                    var block_7d = await Fetch_SCH_HIS_Block7D();
                    if (block_7d != null && block_7d.Count > 0)
                    {
                        var matching_7d = block_7d.FirstOrDefault(x => x.block_7a_id == obj.id);
                        if (matching_7d != null)
                        {
                            matching_7d.code = obj.NicCode;
                            matching_7d.item_2 = Block_4_Constants.NIC_CODES.FirstOrDefault(x => x.id == obj.NicCode)?.title ?? "";
                            await Save_SCH_HIS_Block7D_List(matching_7d);
                        }
                    }
                }
                else
                {
                    status = await _database.InsertAsync(obj);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 7: {ex.Message}");
                return null;
            }
        }

        public async Task<int> DeleteBlock7_NICList(Guid id)
        {
            try
            {
                var exists = await _database.Table<Tbl_Block_7c_NIC>().Where(x => x.id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        exists.is_deleted = true;
                        await _database.UpdateAsync(exists);
                    }
                    else
                    {
                        int deleted = await _database.DeleteAsync(exists);
                    }
                    var block_7d = await Fetch_SCH_HIS_Block7D();
                    if(block_7d != null)
                    {
                        var item = block_7d.FirstOrDefault(x => x.block_7c_id == id);
                        if (item != null)
                        {
                            _ = await Delete_SCH_HIS_Block7D_List(item);
                        }
                    }
                    await ReserializeBlock7NICList();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting item " + ex.Message);
                return 0;
            }
        }

        private async Task ReserializeBlock7NICList()
        {
            try
            {
                var items = await Fetch_SCH_HIS_Block7_NICList();
                if (items != null && items.Count > 0)
                {
                    int s = 1;
                    foreach (var item in items)
                    {
                        item.SerialNumber = s;
                        s++;
                        await _database.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<Tbl_Block_7c_Q10>> Fetch_SCH_HIS_Block7_Q10_List()
        {
            try
            {
                var response = await _database.Table<Tbl_Block_7c_Q10>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == SessionStorage.selected_hhd_id && (x.is_deleted == null || x.is_deleted == false)).ToListAsync();
                if (response != null && response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<Tbl_Block_7c_Q10>();
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 7 NICLIST: {ex.Message}");
                return new List<Tbl_Block_7c_Q10>();
            }
        }

        public async Task<int?> Save_SCH_HIS_Block7_Q10_List(Tbl_Block_7c_Q10 obj)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_7c_Q10>().Where(x => x.id == obj.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(obj);
                }
                else
                {
                    status = await _database.InsertAsync(obj);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 7: {ex.Message}");
                return null;
            }
        }

        public async Task<int> DeleteBlock7_Q10_List(Guid id)
        {
            try
            {
                var exists = await _database.Table<Tbl_Block_7c_Q10>().Where(x => x.id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        exists.is_deleted = true;
                        await _database.UpdateAsync(exists);
                    }
                    else
                    {
                        int deleted = await _database.DeleteAsync(exists);
                    }
                    await ReserializeBlock7_Q10_List();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting item " + ex.Message);
                return 0;
            }
        }

        private async Task ReserializeBlock7_Q10_List()
        {
            try
            {
                var items = await Fetch_SCH_HIS_Block7_Q10_List();
                if (items != null && items.Count > 0)
                {
                    int s = 1;
                    foreach (var item in items)
                    {
                        item.serial_number = s;
                        s++;
                        await _database.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<Tbl_Block_7d>> Fetch_SCH_HIS_Block7D()
        {
            try
            {
                var response = await _database.Table<Tbl_Block_7d>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == SessionStorage.selected_hhd_id && (x.is_deleted == null || x.is_deleted == false)).ToListAsync();
                if (response != null && response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<Tbl_Block_7d>();
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error while fetching Block 7D list: {ex.Message}");
                return new List<Tbl_Block_7d>();
            }
        }

        public async Task<int?> Save_SCH_HIS_Block7D_List(Tbl_Block_7d obj)
        {
            try
            {
                int status;
                var check_existence = await _database.Table<Tbl_Block_7d>().Where(x => x.id == obj.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(obj);
                }
                else
                {
                    status = await _database.InsertAsync(obj);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 7D: {ex.Message}");
                return null;
            }
        }

        public async Task<int?> Delete_SCH_HIS_Block7D_List(Tbl_Block_7d obj)
        {
            try
            {
                int status;
                var check_existence = await _database.Table<Tbl_Block_7d>().Where(x => x.id == obj.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        obj.is_deleted = true;
                        status = await _database.UpdateAsync(obj);
                    }
                    else
                    {
                        status = await _database.DeleteAsync(obj);
                    }

                    await ReserializeBlock7DList();
                    return status;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While deleting SCH HIS Block 7D: {ex.Message}");
                return null;
            }
        }

        public async Task<Tbl_Block_8?> Fetch_SCH_HIS_Block8(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_8>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 8: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Save_SCH_HIS_Block8(Tbl_Block_8 tbl_block_8)
        {
            try
            {
                int status;
                var check_existence = await _database.Table<Tbl_Block_8>().Where(x => x.id == tbl_block_8.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_8);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_8);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 8: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<Tbl_Block_8_Q6>> Fetch_SCH_HIS_Block8_6(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_8_Q6>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).ToListAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return new List<Tbl_Block_8_Q6>();
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 8_Q6: {ex.Message}");
                return new List<Tbl_Block_8_Q6>();
            }
        }

        public async Task<int> Save_SCH_HIS_Block8_Q6(Tbl_Block_8_Q6 tbl_block_8)
        {
            try
            {
                int status;
                var check_existence = await _database.Table<Tbl_Block_8_Q6>().Where(x => x.id == tbl_block_8.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_8);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_8);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 8: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> Delete_SCH_HIS_Block8_Q6(Tbl_Block_8_Q6 obj)
        {
            try
            {
                int status;
                var check_existence = await _database.Table<Tbl_Block_8_Q6>().Where(x => x.id == obj.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    if (SessionStorage.FSU_Submitted)
                    {
                        obj.is_deleted = true;
                        status = await _database.UpdateAsync(obj);
                    }
                    else
                    {
                        status = await _database.DeleteAsync(obj);
                    }

                    await ReserializeBlock8List();
                    return status;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While deleting SCH HIS Block 7D: {ex.Message}");
                return 0;
            }
        }

        public async Task ReserializeBlock8List()
        {
            try
            {
                var items = await Fetch_SCH_HIS_Block8_6(SessionStorage.selected_hhd_id);
                if (items != null && items.Count > 0)
                {
                    int s = 1;
                    foreach (var item in items)
                    {
                        item.serial_number = s;
                        s++;
                        await _database.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<Tbl_Block_9a?> Fetch_SCH_HIS_Block9a(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_9a>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 9a: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Save_SCH_HIS_Block9a(Tbl_Block_9a tbl_block_9a)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_9a>().Where(x => x.id == tbl_block_9a.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_9a);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_9a);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 9a: {ex.Message}");
                return 0;
            }
        }

        public async Task<Tbl_Block_9b?> Fetch_SCH_HIS_Block9B(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_9b>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 9B: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Save_SCH_HIS_Block9B(Tbl_Block_9b tbl_block_9b)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_9b>().Where(x => x.id == tbl_block_9b.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_9b);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_9b);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 9B: {ex.Message}");
                return 0;
            }
        }

        public async Task<Tbl_Block_10?> Fetch_SCH_HIS_Block10()
        {
            try
            {
                var response = await _database.Table<Tbl_Block_10>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == SessionStorage.selected_hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 10: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Save_SCH_HIS_Block10(Tbl_Block_10 tbl_block_10)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_10>().Where(x => x.id == tbl_block_10.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_10);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_10);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 10: {ex.Message}");
                return 0;
            }
        }

        public async Task<Tbl_Block_11a?> Fetch_SCH_HIS_Block11(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_11a>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).FirstOrDefaultAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 11a: {ex.Message}");
                return null;
            }
        }

        public async Task<int> Save_SCH_HIS_Block11a(Tbl_Block_11a tbl_block_11a)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_11a>().Where(x => x.id == tbl_block_11a.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    status = await _database.UpdateAsync(tbl_block_11a);
                }
                else
                {
                    status = await _database.InsertAsync(tbl_block_11a);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 11a: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<Tbl_Block_11b>> Fetch_SCH_HIS_Block11b(int hhd_id)
        {
            try
            {
                var response = await _database.Table<Tbl_Block_11b>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.hhd_id == hhd_id && (x.is_deleted == null || x.is_deleted == false)).ToListAsync();
                if (response != null)
                {
                    return response;
                }
                else
                {
                    return new();
                }
            }
            catch (Exception ex)
            {
                toastService.ShowError($"Error While fetching Block 11b: {ex.Message}");
                return new();
            }
        }

        public async Task<int> Save_SCH_HIS_Block11b(Tbl_Block_11b tbl_block_11b)
        {
            try
            {
                int status = new();
                var check_existence = await _database.Table<Tbl_Block_11b>().Where(x => x.id == tbl_block_11b.id).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    tbl_block_11b.is_updated = true;
                    status = await _database.UpdateAsync(tbl_block_11b);
                }
                else
                {
                    tbl_block_11b.is_updated = false;
                    status = await _database.InsertAsync(tbl_block_11b);
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error While saving SCH HIS Block 11b: {ex.Message}");
                return 0;
            }
        }

        public enum DeleteFilter
        {
            ExcludeDeleted,   // default: SAME behaviour as now
            IncludeAll,       // return everything
            OnlyDeleted       // return only deleted rows
        }


        public async Task<T?> FetchSingleForFsuAndHhdAsync<T>(
    DeleteFilter filter = DeleteFilter.ExcludeDeleted)
    where T : Tbl_Base, IHISModel, new()
        {
            var query = _database.Table<T>()
                .Where(x => x.fsu_id == SessionStorage.SelectedFSUId)
                .Where(x => x.hhd_id == SessionStorage.selected_hhd_id);

            // apply delete filter
            query = filter switch
            {
                DeleteFilter.ExcludeDeleted => query.Where(x => x.is_deleted == null || x.is_deleted == false),
                DeleteFilter.OnlyDeleted => query.Where(x => x.is_deleted == true),
                DeleteFilter.IncludeAll => query, // no filter
                _ => query
            };

            return await query.FirstOrDefaultAsync();
        }


        public async Task<List<T>> FetchListAsync<T>(
    DeleteFilter filter = DeleteFilter.ExcludeDeleted)
    where T : Tbl_Base, IHISModel, new()
        {
            try
            {
                var query = _database.Table<T>()
                    .Where(x => x.fsu_id == SessionStorage.SelectedFSUId)
                    .Where(x => x.hhd_id == SessionStorage.selected_hhd_id);

                // apply delete filter
                query = filter switch
                {
                    DeleteFilter.ExcludeDeleted => query.Where(x => x.is_deleted == null || x.is_deleted == false),
                    DeleteFilter.OnlyDeleted => query.Where(x => x.is_deleted == true),
                    DeleteFilter.IncludeAll => query,
                    _ => query
                };

                return await query.ToListAsync();
            }
            catch
            {
                return new List<T>();
            }
        }




        public async Task<int> SaveAsync<T>(T entity) where T : Tbl_Base, new()
        {
            if (entity == null)
                return 0;

            // Check if exists in database
            var existing = await _database.Table<T>()
                .FirstOrDefaultAsync(x => x.id == entity.id);

            entity.survey_timestamp = DateTime.Now;

            if (existing == null)
            {
                // INSERT
                entity.id = Guid.NewGuid();
                entity.survey_coordinates = SessionStorage.location;
                await _database.InsertAsync(entity);
                return 1;
            }
            else
            {
                // UPDATE
                await _database.UpdateAsync(entity);
                return 1;
            }
        }

        public async Task DeleteEntryAsync<T>(Guid id) where T : Tbl_Base, new()
        {
            // Fetch entry by ID
            var entry = await _database.Table<T>().FirstOrDefaultAsync(x => x.id == id);

            if (entry == null)
                return;

            // If FSU is submitted → soft delete
            if (SessionStorage.FSU_Submitted)
            {
                entry.is_deleted = true;
                await _database.UpdateAsync(entry);
            }
            else
            {
                // Hard delete
                await _database.DeleteAsync(entry);
            }
        }


        //Warning and Comment related queries
        public async Task<int> UpsertWarningAsync(List<Tbl_Warning> warnings)
        {
            int result = 0;

            if (warnings == null || !warnings.Any())
                return result;

            foreach (var warning in warnings)
            {
                if (warning == null)
                    continue;

                if (warning.id == Guid.Empty)
                    warning.id = Guid.NewGuid(); // Ensure a valid ID

                var existing = await _database.Table<Tbl_Warning>()
                    .Where(w => w.id == warning.id)
                    .FirstOrDefaultAsync();

                if (existing != null)
                {
                    await _database.UpdateAsync(warning);
                    result++;
                }
                else
                {
                    await _database.InsertAsync(warning);
                    result++;
                }
            }

            return result;
        }



        

        public async Task<List<Tbl_Warning>> GetWarningList(int hhd_id = 0, string schedule = "HIS")
        {
            //if (SessionStorage.user_role == CommonConstants.USER_CODE_JSO)
            //{
            //    return await _database.Table<Tbl_Warning>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && (x.parent_comment_id == Guid.Empty || x.parent_comment_id == null) && (x.is_deleted == false || x.is_deleted == null) && x.warning_status == 1 && x.hhd_id == hhd_id).ToListAsync();
            //}
            //else
            //{
            //    return await _database.Table<Tbl_Warning>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && (x.parent_comment_id == Guid.Empty || x.parent_comment_id == null) && (x.is_deleted == false || x.is_deleted == null) && x.warning_status == 2 && x.hhd_id == hhd_id).ToListAsync();
            //}
            if (schedule == "0")
            {
                return await _database.Table<Tbl_Warning>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && (x.is_deleted == false || x.is_deleted == null)).ToListAsync();
            }
            else
            {
                return await _database.Table<Tbl_Warning>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && (x.is_deleted == false || x.is_deleted == null) && x.hhd_id == hhd_id).ToListAsync();
            }

        }

        public async Task<List<Tbl_Warning>> GetWarningTableDataForSerial(int fsuId, int hddId, string block, int serial)
        {
            return await _database.Table<Tbl_Warning>().Where(x => x.fsu_id == fsuId && x.hhd_id == hddId && (x.parent_comment_id == Guid.Empty || x.parent_comment_id == null) && x.block == block && x.serial_number == serial && (x.is_deleted == false || x.is_deleted == null)).ToListAsync();
        }

        public async Task<List<Tbl_Warning>> GetWarningTableDataForBlock(int fsuId, int hddId, string schedule, string block)
        {
            return await _database.Table<Tbl_Warning>().Where(x => x.fsu_id == fsuId && x.hhd_id == hddId && x.schedule == schedule && (x.parent_comment_id == Guid.Empty || x.parent_comment_id == null) && x.block == block && (x.is_deleted == false || x.is_deleted == null)).ToListAsync();
        }

        public async Task<List<Tbl_Warning>> GetChildCommentsAsync(Guid parentId)
        {
            return await _database.Table<Tbl_Warning>()
                .Where(x => x.parent_comment_id == parentId && (x.is_deleted == null || x.is_deleted == false))
                .ToListAsync();
        }


        public Task<int> DeleteWarningTableDataForSerial(int fsuId, int hddId, string block, int serial, Guid id)
        {
            var data = _database.QueryAsync<Tbl_Warning>($"SELECT * FROM Tbl_Warning WHERE fsu_id == {fsuId} AND hhd_id == {hddId}  AND warning_status == 1");
            if (data.Result.Count != 0)
            {
                var result = _database.ExecuteAsync($"DELETE FROM Tbl_Warning WHERE fsu_id == {fsuId} AND hhd_id == {hddId}");
                if (result?.Result > 0)
                {//Delete child
                    _database.ExecuteAsync($"DELETE FROM Tbl_Warning WHERE fsu_id == {fsuId} AND hhd_id == {hddId} AND parent_id == {id}");
                }
                return result;
            }
            else
            {
                var result = _database.ExecuteAsync($"UPDATE Tbl_Warning SET is_deleted = true WHERE fsu_id == {fsuId} AND hhd_id == {hddId} AND id == {id}");
                if (result?.Result > 0)
                {//Delete child
                    _database.ExecuteAsync($"UPDATE Tbl_Warning SET is_deleted = true WHERE fsu_id == {fsuId} AND hhd_id == {hddId} AND parent_id == {id}");
                }
                return result;
            }

        }
    }
}
