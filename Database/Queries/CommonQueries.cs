using Income.Common;
using Income.Database.Models.Common;
using Income.Database.Models.HIS_2026;
using Income.Database.Models.SCH0_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Queries
{
    public class CommonQueries : Database
    {
        public async Task<Tbl_User_Details>? GetUserDetailsAsync()
        {
            try
            {
                var userDetails = await _database.QueryAsync<Tbl_User_Details>("SELECT TU.* FROM Tbl_User_Details TU Where TU.is_Deleted == false OR TU.is_Deleted IS NULL");
                if (userDetails != null && userDetails.Count > 0)
                {
                    return userDetails[0]; // Return the first user details
                }
                else
                {
                    return null; // No user details found
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Tbl_User_Details>? InsertUser(Tbl_User_Details? user_details)
        {
            try
            {
                var userDetails = await _database.QueryAsync<Tbl_User_Details>($"SELECT TU.* FROM Tbl_User_Details TU Where  TU.id == '{user_details?.id}' AND (TU.is_Deleted == false OR TU.is_Deleted IS NULL)");
                if (userDetails != null && userDetails.Count > 0)
                {
                    user_details.id = userDetails[0].id;
                    var res = await _database.UpdateAsync(user_details);
                    if (res > 0)
                    {
                        return user_details; // Return the updated user details
                    }
                    else
                    {
                        return null; // Update failed
                    }
                }
                else
                {
                    var res = await _database.InsertAsync(user_details);
                    if (res > 0)
                    {
                        return user_details; // Return the inserted user details
                    }
                    else
                    {
                        return null; // Insert failed
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return null;
            }
        }

        public async Task<int> DeleteUserData()
        {
            try
            {
                // var result = await _database.DeleteAllAsync<Tbl_User_Details>();
                var result = await _database.ExecuteAsync("UPDATE Tbl_User_Details SET is_Deleted = true");
                if (result == null)
                    return 0;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error deleting user details: {ex.Message}");
                return 0;
            }
        }

        public async Task<Tbl_User_Details>? GetDeletedUserDetailsAsync()
        {
            try
            {
                var userDetails = await _database.QueryAsync<Tbl_User_Details>("SELECT TU.* FROM Tbl_User_Details TU Where TU.is_Deleted == true");
                if (userDetails != null && userDetails.Count > 0)
                {
                    return userDetails[0]; // Return the first user details
                }
                else
                {
                    return null; // No user details found
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Tbl_Lookup>?>? FetchLookupDataByType(string type_name)
        {
            try
            {
                var items = await _database.QueryAsync<Tbl_Lookup>("SELECT id, lookup_type, title FROM Tbl_Lookup WHERE lookup_type = ?", type_name);
                return items != null && items.Count > 0 ? items : null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error deleting user details: {ex.Message}");
                return null;
            }
        }

        public async Task InsertVisitedBlock(Tbl_Visited_Blocks block_data)
        {
            try
            {
                List<Tbl_Visited_Blocks> is_exist = [];
                if(SessionStorage.selected_hhd_id == 0)
                {
                    is_exist = await _database.QueryAsync<Tbl_Visited_Blocks>("SELECT id FROM Tbl_Visited_Blocks tvb WHERE tvb.block_code = ? AND tvb.fsu_id = ?", block_data.block_code, SessionStorage.SelectedFSUId);
                }
                else
                {
                    is_exist = await _database.QueryAsync<Tbl_Visited_Blocks>("SELECT id FROM Tbl_Visited_Blocks tvb WHERE tvb.block_code = ? AND tvb.fsu_id = ? AND tvb.hhd_id = ?", block_data.block_code, SessionStorage.SelectedFSUId, SessionStorage.selected_hhd_id);
                }
                if (is_exist == null || is_exist.Count == 0)
                {
                    block_data.id = Guid.NewGuid();
                    block_data.fsu_id = SessionStorage.SelectedFSUId;
                    block_data.hhd_id = SessionStorage.selected_hhd_id;
                    await _database.InsertAsync(block_data);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task<List<Tbl_Visited_Blocks>?>? GetVisitedBlocks()
        {
            try
            {
                List<Tbl_Visited_Blocks> list = [];
                Tbl_Visited_Blocks tbl_visited_block = new()
                {
                    block_uri = "/dashboard/fsulist",
                    block_title = "FSU List",
                    block_code = CommonEnum.fsu_list_page.ToString()
                };
                
                list.Add(tbl_visited_block);
                if (SessionStorage.selected_hhd_id != 0)
                {
                    Tbl_Visited_Blocks tbl_visited_block_0 = new()
                    {
                        block_uri = "/dashboard/his/ssu_page",
                        block_title = "SSU Page",
                        block_code = CommonEnum.ssu_list_page.ToString()
                    };
                    list.Add(tbl_visited_block_0);
                }
                var items = await _database.QueryAsync<Tbl_Visited_Blocks>("SELECT id, block_uri, block_title, block_code FROM Tbl_Visited_Blocks tvb WHERE tvb.fsu_id = ? and tvb.hhd_id = ?", SessionStorage.SelectedFSUId, SessionStorage.selected_hhd_id);
                if (items != null && items.Count > 0)
                {
                    list.AddRange(items);
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteSSUBlockAsync()
        {
            try
            {
                await _database.ExecuteAsync(
                    "DELETE FROM Tbl_Visited_Blocks WHERE block_title = ?",
                    "SSU List Page"
                );
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<Tbl_Fsu_List>>? FetchFsuList()
        {
            try
            {
                var items = await _database.QueryAsync<Tbl_Fsu_List>("SELECT * FROM Tbl_Fsu_List WHERE tenant_id = ? AND user_id = ?", SessionStorage.tenant_id, SessionStorage.__user_id);
                return items != null && items.Count > 0 ? items : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Tbl_Fsu_List?> FetchFsuByFsuId(int fsu_id)
        {
            try
            {
                var items = await _database.QueryAsync<Tbl_Fsu_List>("SELECT * FROM Tbl_Fsu_List WHERE fsu_id=?", fsu_id);
                if (items != null && items.Count > 0)
                {
                    return items.First();
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
        public async Task<List<Tbl_Fsu_List>>? FetchFsuListByFsuId(int fsu_id)
        {
            try
            {
                var items = await _database.QueryAsync<Tbl_Fsu_List>("SELECT * FROM Tbl_Fsu_List WHERE fsu_id=?", fsu_id);
                return items != null && items.Count > 0 ? items : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task UpdateFSU(Tbl_Fsu_List fsu)
        {
            try
            {
                if (fsu != null)
                {
                    var check_existence = await _database.Table<Tbl_Fsu_List>().Where(x => x.id == fsu.id).ToListAsync();
                    if (check_existence != null)
                    {
                        await _database.UpdateAsync(fsu);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<int?> SaveFsuList(List<Tbl_Fsu_List> tbl_Fsu_List)
        {
            try
            {
                if (tbl_Fsu_List != null && tbl_Fsu_List.Count > 0)
                {
                    foreach (var item in tbl_Fsu_List)
                    {
                        var check_existance = await _database.Table<Tbl_Fsu_List>().Where(x => x.id == item.id).ToListAsync();
                        if (check_existance != null && check_existance.Count > 0)
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
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task DeleteAllBlockData()
        {
            await _database.ExecuteAsync("BEGIN TRANSACTION");
            try
            {
                await _database.DeleteAllAsync<Tbl_User_Details>();
                await _database.DeleteAllAsync<Tbl_Sch_0_0_Block_0_1>();
                await _database.DeleteAllAsync<Tbl_Sch_0_0_FieldOperation>();
                await _database.DeleteAllAsync<Tbl_Sch_0_0_Block_3>();
                await _database.DeleteAllAsync<Tbl_Sch_0_0_Block_2_1>();
                await _database.DeleteAllAsync<Tbl_Sch_0_0_Block_2_2>();
                await _database.DeleteAllAsync<Tbl_Sch_0_0_Block_4>();
                await _database.DeleteAllAsync<Tbl_Sch_0_0_Block_5>();
                await _database.DeleteAllAsync<Tbl_Sch_0_0_Block_7>();               
                await _database.DeleteAllAsync<Tbl_Fsu_List>();
                await _database.DeleteAllAsync<Tbl_Visited_Blocks>();
                await _database.ExecuteAsync("COMMIT");
                return;
            }
            catch (Exception)
            {
                await _database.ExecuteAsync("ROLLBACK");
                return;
            }
        }

        public async Task<int> DeleteBlockDataForCasulaty()
        {
            await _database.ExecuteAsync("BEGIN TRANSACTION");
            try
            {
                int fsuId = SessionStorage.SelectedFSUId;
                int hhdId = SessionStorage.selected_hhd_id;
                bool submitted = SessionStorage.HHD_Submitted;

                // List of all block tables involved
                var tables = new Type[]
                {
            typeof(Tbl_Block_3),
            typeof(Tbl_Block_4),
            typeof(Tbl_Block_4_Q5),
            typeof(Tbl_Block_5),
            typeof(Tbl_Block_6),
            typeof(Tbl_Block_7a),
            typeof(Tbl_Block_7a_1),
            typeof(Tbl_Block_7b),
            typeof(Tbl_Block_7c),
            typeof(Tbl_Block_7c_NIC),
            typeof(Tbl_Block_7c_Q10),
            typeof(Tbl_Block_7d),
            typeof(Tbl_Block_8),
            typeof(Tbl_Block_8_Q6),
            typeof(Tbl_Block_9a),
            typeof(Tbl_Block_9b),
            typeof(Tbl_Block_10),
            typeof(Tbl_Block_11a),
            typeof(Tbl_Block_11b),
            typeof(Tbl_Block_A),
            typeof(Tbl_Block_B),
            typeof(Tbl_Block_FieldOperation),
            typeof(Tbl_Warning)
                };

                foreach (var table in tables)
                {
                    if (submitted)
                    {
                        // Soft delete → set is_deleted = 1
                        string query =
                            $"UPDATE {table.Name} SET is_deleted = 1 " +
                            $"WHERE fsu_id = ? AND hhd_id = ?";

                        await _database.ExecuteAsync(query, fsuId, hhdId);
                    }
                    else
                    {
                        // Hard delete → remove records
                        string query =
                            $"DELETE FROM {table.Name} " +
                            $"WHERE fsu_id = ? AND hhd_id = ?";

                        await _database.ExecuteAsync(query, fsuId, hhdId);
                    }
                }

                await _database.ExecuteAsync("COMMIT");
                return 1;
            }
            catch (Exception ex)
            {
                await _database.ExecuteAsync("ROLLBACK");
                return 0;
            }
        }


        public async Task<int?> UpdateFsuDownloadStatus(int Fsu_ID)
        {
            try
            {
                var check_existence = await _database.Table<Tbl_Fsu_List>().Where(x => x.fsu_id == Fsu_ID).FirstOrDefaultAsync();
                if (check_existence != null)
                {
                    check_existence.NeedDownload = false;
                    await _database.UpdateAsync(check_existence);
                    return 1;
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
    }
}
