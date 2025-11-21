using Income.Common;
using Income.Database.Models.Common;
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
                var is_exist = await _database.QueryAsync<Tbl_Visited_Blocks>("SELECT id FROM Tbl_Visited_Blocks tvb WHERE tvb.block_code = ? AND tvb.fsu_id = ?", block_data.block_code, SessionStorage.SelectedFSUId);
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

        //public async Task<Su_Submission_Model?> FetchSSO_SubmissionData(List<Tbl_Sch_0_0_Block_5> submission_data)
        //{
        //    await _database.ExecuteAsync("BEGIN TRANSACTION");
        //    try
        //    {
        //        Su_Submission_Model? submissionData = new();
        //        bind_layer? layer = new bind_layer();
        //        List<SCH_25_Response> sCH_25_Responses = new List<SCH_25_Response>();
        //        SCH_25_Quaries sCH_25_Quaries = new();
        //        HashSet<int> hhdStatus = new HashSet<int>();
        //        int LookupStatus = 11;
        //        layer.TravelSch00block0 = await _database.Table<Tbl_Sch_0_0_Block_0_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).FirstOrDefaultAsync();
        //        layer.TravelSch00block3List = await _database.Table<Tbl_File>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4aList = await _database.Table<Tbl_Sch_0_0_Block_4_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4aList = await _database.Table<Tbl_Sch_0_0_Block_4_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4bList = await _database.Table<Tbl_Sch_0_0_Block_4_2>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4cList = await _database.Table<Tbl_Sch_0_0_Block_4_3>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block5aList = await _database.Table<Tbl_Sch_0_0_Block_5>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block2 = await _database.Table<Tbl_Sch_0_0_Block_2>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).FirstOrDefaultAsync();
        //        List<Tbl_Sch_0_0_Block_5> su_list = layer.TravelSch00Block5aList.Where(k => k.is_selected_travel == true && k.status_travel != "SUBSTITUTED").ToList();
        //        foreach (var item in su_list)
        //        {
        //            if (submission_data.Select(x => x.Block_5A_3).ToList().Contains(item.Block_5A_3))
        //            {
        //                hhdStatus.Add(21);
        //            }
        //            else
        //            {
        //                if (item.hhdStatus == 0 || item.hhdStatus == 10 || item.hhdStatus == 11)
        //                {
        //                    hhdStatus.Add(11);
        //                }
        //                else
        //                {
        //                    hhdStatus.Add(item.hhdStatus);
        //                }
        //            }
        //        }
        //        LookupStatus = hhdStatus.Min();
        //        //int Count_of_submitted = su_list.Count(k => k.hhdStatus == 21 && k.fsu_id == SessionStorage.SelectedFSUId && k.survey_id == SessionStorage.surveyId); 
        //        //int Count_ofSending = submission_data.Count();
        //        if (submission_data != null && submission_data.Count > 0)
        //        {
        //            foreach (var item in submission_data)
        //            {
        //                var status = await sCH_25_Quaries.FetchStatusManager(item.Block_5A_3);
        //                SCH_25_Response? sCH_25_Response = new();
        //                sCH_25_Response.block_1 = await _database.Table<Block_1>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                sCH_25_Response.block_2 = await _database.Table<Block_2>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                sCH_25_Response.block_3 = await _database.Table<Block_3>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_4 = await _database.Table<Block_4>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                sCH_25_Response.block_4_1 = await _database.Table<Block_4_1>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                sCH_25_Response.block_4_1_Item_3 = await _database.Table<Block_4_1_Item_3>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_5_1 = await _database.Table<Block_5_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_5_2 = await _database.Table<Block_5_2>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_6_1 = await _database.Table<Block_6_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_6_2 = await _database.Table<Block_6_2>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_7 = await _database.Table<Block_7>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_8 = await _database.Table<Block_8>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_8_1 = await _database.Table<Block_8_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_9 = await _database.Table<Block_9>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_9_1 = await _database.Table<Block_9_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.comments = await _database.Table<Tbl_Comments>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.warning = await _database.Table<Tbl_Warning>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.over_night_trip_details = await _database.Table<Tbl_Details_Overnight_Trip_Block_9>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.hhd_id = item.Block_5A_3;
        //                sCH_25_Response.hhd_status = 21;
        //                // await sCH_25_Quaries.SaveHhdStatus(sCH_25_Response.hhd_id ?? 0, 21,SessionStorage.SelectedFSUId);
        //                sCH_25_Responses.Add(sCH_25_Response);
        //            }
        //        }
        //        else
        //        {
        //            await _database.ExecuteAsync("COMMIT");
        //            return null;
        //        }
        //        layer.TravelSch25datumDto = sCH_25_Responses;
        //        submissionData.survey_id = SessionStorage.surveyId;
        //        submissionData.fsu_id = SessionStorage.SelectedFSUId;
        //        submissionData.submitted_json_data = Newtonsoft.Json.JsonConvert.SerializeObject(layer);
        //        submissionData.lookup_fsu_submit_status = LookupStatus;
        //        await _database.ExecuteAsync("COMMIT");
        //        return submissionData;
        //    }
        //    catch (Exception ex)
        //    {
        //        await _database.ExecuteAsync("ROLLBACK");
        //        System.Console.WriteLine($"Error while Fetching SSO Submission Data: {ex.Message}");
        //        return null;
        //    }
        //}

        //public async Task<Su_Submission_Model?> Fetch_SubmissionData(int hhd_id, int status_id, string type)
        //{
        //    await _database.ExecuteAsync("BEGIN TRANSACTION");
        //    try
        //    {
        //        int LookupStatus = 11;
        //        Su_Submission_Model? submissionData = new();
        //        bind_layer? layer = new bind_layer();
        //        List<SCH_25_Response> sCH_25_Responses = new List<SCH_25_Response>();
        //        SCH_25_Quaries sCH_25_Quaries = new();
        //        HashSet<int> hhdStatus = new HashSet<int>();
        //        layer.TravelSch00block0 = await _database.Table<Tbl_Sch_0_0_Block_0_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).FirstOrDefaultAsync();
        //        layer.TravelSch00block3List = await _database.Table<Tbl_File>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4aList = await _database.Table<Tbl_Sch_0_0_Block_4_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4aList = await _database.Table<Tbl_Sch_0_0_Block_4_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4bList = await _database.Table<Tbl_Sch_0_0_Block_4_2>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4cList = await _database.Table<Tbl_Sch_0_0_Block_4_3>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block5aList = await _database.Table<Tbl_Sch_0_0_Block_5>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block2 = await _database.Table<Tbl_Sch_0_0_Block_2>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).FirstOrDefaultAsync();
        //        var su_list = await _database.QueryAsync<Tbl_Sch_0_0_Block_5>("SELECT * FROM Tbl_Sch_0_0_Block_5 WHERE is_selected_travel = true AND fsu_id = ? AND tenant_id = ?", SessionStorage.SelectedFSUId, SessionStorage.tenant_id);
        //        List<Tbl_Sch_0_0_Block_5> HHDlist = layer.TravelSch00Block5aList.Where(k => k.is_selected_travel == true && k.status_travel != "SUBSTITUTED").ToList();
        //        foreach (var item in HHDlist)
        //        {
        //            if (item.Block_5A_3 == hhd_id)
        //            {
        //                hhdStatus.Add(status_id);
        //            }
        //            else
        //            {
        //                if (item.hhdStatus == 0 || item.hhdStatus == 10 || item.hhdStatus == 11)
        //                {
        //                    hhdStatus.Add(11);
        //                }
        //                else
        //                {
        //                    hhdStatus.Add(item.hhdStatus);
        //                }
        //            }
        //        }
        //        LookupStatus = hhdStatus.Min();
        //        List<Tbl_Sch_0_0_Block_5> submission_data = su_list.Where(x => x.Block_5A_3 == hhd_id).ToList();
        //        // int Count_of_selected =  su_list.Count(k => k.is_selected_travel == true && k.status_travel != "SUBSTITUTED");
        //        // int Count_of_submitted = su_list.Count(k => k.hhdStatus == 21);
        //        //int countOfsubmitted31 = su_list.Count(k => k.hhdStatus == 31);
        //        //int countOfsubmitted22 = su_list.Count(k => k.hhdStatus == 22);
        //        //int countOfsubmitted12 = su_list.Count(k => k.hhdStatus == 12);
        //        //int Count_ofSending = submission_data.Count();

        //        if (su_list != null && su_list.Count > 0)
        //        {
        //            foreach (var item in su_list)
        //            {
        //                if (item.Block_5A_3 == hhd_id)
        //                {
        //                    SCH_25_Response? sCH_25_Response = new();
        //                    sCH_25_Response.block_1 = await _database.Table<Block_1>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                    sCH_25_Response.block_2 = await _database.Table<Block_2>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                    sCH_25_Response.block_3 = await _database.Table<Block_3>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_4 = await _database.Table<Block_4>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                    sCH_25_Response.block_4_1 = await _database.Table<Block_4_1>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                    sCH_25_Response.block_4_1_Item_3 = await _database.Table<Block_4_1_Item_3>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_5_1 = await _database.Table<Block_5_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_5_2 = await _database.Table<Block_5_2>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_6_1 = await _database.Table<Block_6_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_6_2 = await _database.Table<Block_6_2>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_7 = await _database.Table<Block_7>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_8 = await _database.Table<Block_8>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_8_1 = await _database.Table<Block_8_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_9 = await _database.Table<Block_9>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.block_9_1 = await _database.Table<Block_9_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.comments = await _database.Table<Tbl_Comments>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.warning = await _database.Table<Tbl_Warning>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.over_night_trip_details = await _database.Table<Tbl_Details_Overnight_Trip_Block_9>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                    sCH_25_Response.hhd_id = item.Block_5A_3;
        //                    sCH_25_Response.hhd_status = status_id;
        //                    // await sCH_25_Quaries.SaveHhdStatus(sCH_25_Response.hhd_id ?? 0, status_id,SessionStorage.SelectedFSUId);
        //                    sCH_25_Responses.Add(sCH_25_Response);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            await _database.ExecuteAsync("COMMIT");
        //            return null;
        //        }
        //        layer.TravelSch25datumDto = sCH_25_Responses;
        //        submissionData.survey_id = SessionStorage.surveyId;
        //        submissionData.fsu_id = SessionStorage.SelectedFSUId;
        //        submissionData.submitted_json_data = Newtonsoft.Json.JsonConvert.SerializeObject(layer);
        //        submissionData.lookup_fsu_submit_status = LookupStatus;
        //        await _database.ExecuteAsync("COMMIT");
        //        return submissionData;
        //    }
        //    catch (Exception ex)
        //    {
        //        await _database.ExecuteAsync("ROLLBACK");
        //        System.Console.WriteLine($"Error while Fetching SSO Submission Data: {ex.Message}");
        //        return null;
        //    }
        //}

        //public async Task<Su_Submission_Model?> Fetch_SubmissionDataFinalSubmit(List<Tbl_Sch_0_0_Block_5> submission_data, int status_id)
        //{
        //    await _database.ExecuteAsync("BEGIN TRANSACTION");
        //    try
        //    {
        //        Su_Submission_Model? submissionData = new();
        //        bind_layer? layer = new bind_layer();
        //        List<SCH_25_Response> sCH_25_Responses = new List<SCH_25_Response>();
        //        SCH_25_Quaries sCH_25_Quaries = new();
        //        layer.TravelSch00block0 = await _database.Table<Tbl_Sch_0_0_Block_0_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).FirstOrDefaultAsync();
        //        layer.TravelSch00block3List = await _database.Table<Tbl_File>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4aList = await _database.Table<Tbl_Sch_0_0_Block_4_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4aList = await _database.Table<Tbl_Sch_0_0_Block_4_1>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4bList = await _database.Table<Tbl_Sch_0_0_Block_4_2>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block4cList = await _database.Table<Tbl_Sch_0_0_Block_4_3>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block5aList = await _database.Table<Tbl_Sch_0_0_Block_5>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).ToListAsync();
        //        layer.TravelSch00Block2 = await _database.Table<Tbl_Sch_0_0_Block_2>().Where(x => x.fsu_id == SessionStorage.SelectedFSUId && x.survey_id == SessionStorage.surveyId && x.tenant_id == SessionStorage.tenant_id).FirstOrDefaultAsync();

        //        if (submission_data != null && submission_data.Count > 0)
        //        {
        //            foreach (var item in submission_data)
        //            {
        //                SCH_25_Response? sCH_25_Response = new();
        //                sCH_25_Response.block_1 = await _database.Table<Block_1>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                sCH_25_Response.block_2 = await _database.Table<Block_2>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                sCH_25_Response.block_3 = await _database.Table<Block_3>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_4 = await _database.Table<Block_4>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                sCH_25_Response.block_4_1 = await _database.Table<Block_4_1>().FirstOrDefaultAsync(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id);
        //                sCH_25_Response.block_4_1_Item_3 = await _database.Table<Block_4_1_Item_3>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_5_1 = await _database.Table<Block_5_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_5_2 = await _database.Table<Block_5_2>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_6_1 = await _database.Table<Block_6_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_6_2 = await _database.Table<Block_6_2>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_7 = await _database.Table<Block_7>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_8 = await _database.Table<Block_8>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_8_1 = await _database.Table<Block_8_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_9 = await _database.Table<Block_9>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.block_9_1 = await _database.Table<Block_9_1>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.comments = await _database.Table<Tbl_Comments>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.warning = await _database.Table<Tbl_Warning>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.over_night_trip_details = await _database.Table<Tbl_Details_Overnight_Trip_Block_9>().Where(k => k.fsu_id == item.fsu_id && k.tenant_id == item.tenant_id && k.hhd_id == item.Block_5A_3 && k.survey_id == item.survey_id).ToListAsync();
        //                sCH_25_Response.hhd_id = item.Block_5A_3;
        //                sCH_25_Response.hhd_status = status_id;
        //                await sCH_25_Quaries.SaveHhdStatus(sCH_25_Response.hhd_id ?? 0, status_id, SessionStorage.SelectedFSUId);
        //                sCH_25_Responses.Add(sCH_25_Response);
        //            }
        //        }
        //        else
        //        {
        //            await _database.ExecuteAsync("ROLLBACK");
        //            return null;
        //        }
        //        layer.TravelSch25datumDto = sCH_25_Responses;
        //        submissionData.survey_id = SessionStorage.surveyId;
        //        submissionData.fsu_id = SessionStorage.SelectedFSUId;
        //        submissionData.submitted_json_data = Newtonsoft.Json.JsonConvert.SerializeObject(layer);
        //        submissionData.lookup_fsu_submit_status = status_id;
        //        await _database.ExecuteAsync("COMMIT");
        //        return submissionData;
        //    }
        //    catch (Exception ex)
        //    {
        //        await _database.ExecuteAsync("ROLLBACK");
        //        return null;
        //    }
        //}

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

        public async Task<string> ClearUnusedData()
        {
            await _database.ExecuteAsync("BEGIN TRANSACTION");
            try
            {
                await _database.ExecuteAsync("DELETE FROM Block_1 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_2 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_3 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_4 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_4_1 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_4_1_Item_3 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_5_1 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_5_2 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_6_1 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_6_2 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_7 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_8 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_8_1 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_9 WHERE is_deleted = 1");
                await _database.ExecuteAsync("DELETE FROM Block_9_1 WHERE is_deleted = 1");
                await _database.ExecuteAsync("COMMIT");
                return CommonConstants.SUCCESS_TEXT;
            }
            catch (Exception ex)
            {
                await _database.ExecuteAsync("ROLLBACK");
                return $"Facing error while cleaning database : {ex}";
            }
        }

        public async Task<string> DeleteBlockDataForCasulaty()
        {
            await _database.ExecuteAsync("BEGIN TRANSACTION");
            try
            {
                await _database.ExecuteAsync($@"DELETE FROM Block_2 WHERE fsu_id = {SessionStorage.SelectedFSUId} AND hhd_id = {SessionStorage.selected_hhd_id} AND tenant_id = {SessionStorage.tenant_id} AND survey_id = '{SessionStorage.surveyId}'");
                await _database.ExecuteAsync($@"DELETE FROM Block_3 WHERE fsu_id = {SessionStorage.SelectedFSUId} AND hhd_id = {SessionStorage.selected_hhd_id} AND tenant_id = {SessionStorage.tenant_id} AND survey_id = '{SessionStorage.surveyId}'");
                await _database.ExecuteAsync($@"DELETE FROM Block_4 WHERE fsu_id = {SessionStorage.SelectedFSUId} AND hhd_id = {SessionStorage.selected_hhd_id} AND tenant_id = {SessionStorage.tenant_id} AND survey_id = '{SessionStorage.surveyId}'");
                
                await _database.ExecuteAsync($@"DELETE FROM Tbl_Warning WHERE fsu_id = {SessionStorage.SelectedFSUId} AND hhd_id = {SessionStorage.selected_hhd_id}");
                await _database.ExecuteAsync("COMMIT");
                return CommonConstants.SUCCESS_TEXT;
            }
            catch (Exception ex)
            {
                await _database.ExecuteAsync("ROLLBACK");
                return $"Facing error while cleaning database : {ex}";
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

        //public async Task<bool> IsDSHasPendingCommentWarning(int fsu_id)
        //{
        //    try
        //    {
        //        var comment_result = await _database.Table<Tbl_Comments>().Where(x => (x.parent_comment_id == null || x.parent_comment_id == Guid.Empty) && (x.comment_status == 3 || x.comment_status == 1 || x.comment_status == 2 || x.comment_status == 4) && x.is_deleted != true).ToListAsync();
        //        if (comment_result != null && comment_result.Count > 0)
        //            return true;

        //        var warning_result = await _database.Table<Tbl_Warning>().Where(x => (x.parent_comment_id == null || x.parent_comment_id == Guid.Empty) && (x.warning_status == 3 || x.warning_status == 1 || x.warning_status == 2 || x.warning_status == 4) && x.is_deleted != true).ToListAsync();
        //        if (warning_result != null && warning_result.Count > 0)
        //            return true;

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}
