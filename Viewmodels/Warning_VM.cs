using BootstrapBlazor.Components;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Income.Common;
using Income.Database.Models.Common;
using Income.Database.Models.SCH0_0;
using Income.Database.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Income.Viewmodels
{
    public class Warning_VM
    {
        public List<Tbl_Warning> WarningList = [];

        public List<Tbl_Warning> _tempWarnings = [];
        public void AddWarning(string warningMsg, string schedule, string blockNumber, string item, string warningCode = "", int hhdNo = 0, int serial = 0)
        {
            Tbl_Warning tbl_Warning = new();
            tbl_Warning.block = blockNumber;
            tbl_Warning.fsu_id = SessionStorage.SelectedFSUId;
            tbl_Warning.hhd_id = hhdNo == 0 ? SessionStorage.selected_hhd_id : hhdNo;
            tbl_Warning.item_no = item;
            tbl_Warning.warning_status = 1;
            tbl_Warning.role_code = SessionStorage.user_role;
            tbl_Warning.user_name = SessionStorage.user_name;
            tbl_Warning.warning_message = warningMsg;
            tbl_Warning.schedule = schedule;
            tbl_Warning.serial_number = serial;
            tbl_Warning.warning_code = warningCode;
            _tempWarnings.Add(tbl_Warning);
        }

        DBQueries dQ = new();
        public List<Tbl_Sch_0_0_Block_7>? selected_HHdList = new();
        public bool Is_Accepted { get; set; }
        public bool IsRejected { get; set; }
        public Tbl_Warning warningcoment { get; set; } = new();


        public async Task SaveWarningAsync(string schedule, string block, int serial = 0)
        {
            try
            {
                // Fetch all saved warnings for this block + schedule + hhd
                var savedWarnings = await dQ.GetWarningTableDataForBlock(
                    SessionStorage.SelectedFSUId,
                    SessionStorage.selected_hhd_id,
                    schedule,
                    block
                );

                if (serial != 0)
                {
                    savedWarnings = savedWarnings.Where(w => w.serial_number == serial).ToList();
                }

                // Convert lists to comparison keys
                var currentKeys = _tempWarnings
                    .Select(w => $"{w.item_no}::{w.serial_number}")
                    .ToHashSet();

                var savedKeys = savedWarnings
                    .Select(w => $"{w.item_no}::{w.serial_number}")
                    .ToHashSet();

                // -----------------------------------------
                // INSERT NEW WARNINGS
                // -----------------------------------------
                var warningsToInsert = _tempWarnings
                    .Where(w => !savedKeys.Contains($"{w.item_no}::{w.serial_number}"))
                    .ToList();

                foreach (var warn in warningsToInsert)
                {
                    warn.id = Guid.NewGuid();
                    warn.created_on = DateTime.Now;
                    warn.updated_at = null;
                    warn.is_deleted = false;

                    await dQ.SaveAsync<Tbl_Warning>(warn);
                }

                // -----------------------------------------
                // DELETE REMOVED WARNINGS (AND THEIR CHILD COMMENTS)
                // -----------------------------------------
                var warningsToDelete = savedWarnings
                    .Where(w => !currentKeys.Contains($"{w.item_no}::{w.serial_number}"))
                    .ToList();

                //            var warningsToDelete = savedWarnings
                //.Where(saved =>
                //    !_tempWarnings.Any(current =>
                //        current.item_no == saved.item_no &&
                //        current.schedule == saved.schedule
                //    )
                //)
                //.ToList();


                foreach (var warn in warningsToDelete)
                {
                    // -----------------------------------------
                    // 1. DELETE CHILD COMMENTS FIRST
                    // -----------------------------------------
                    var childComments = await dQ.GetChildCommentsAsync(warn.id);

                    foreach (var child in childComments)
                    {
                        if (SessionStorage.FSU_Submitted == true)
                        {
                            child.is_deleted = true;
                            child.updated_at = DateTime.Now;
                            await dQ.SaveAsync<Tbl_Warning>(child);
                        }
                        else
                        {
                            await dQ.DeleteEntryAsync<Tbl_Warning>(child.id);
                        }
                    }

                    // -----------------------------------------
                    // 2. DELETE THE PARENT WARNING
                    // -----------------------------------------
                    if (SessionStorage.FSU_Submitted == true)
                    {
                        warn.is_deleted = true;
                        warn.updated_at = DateTime.Now;
                        await dQ.SaveAsync<Tbl_Warning>(warn);
                    }
                    else
                    {
                        await dQ.DeleteEntryAsync<Tbl_Warning>(warn.id);
                    }
                }
            }
            catch (Exception ex)
            {
                // log if needed
            }
            finally
            {
                _tempWarnings.Clear();
                WarningList.Clear();
            }
        }



        public async Task DeleteWarning(string schedule, string block, int serialNo)
        {
            try
            {
                List<Tbl_Warning> data = WarningList.Where(x=> x.serial_number == serialNo).ToList();
                if (data.Any())
                {
                    foreach (var warning in data)
                    {
                        warning.is_deleted = true;
                    }
                    //await sch_25_quaries.UpsertWarningAsync(data);
                }
                else
                {
                    var warningResult = await dQ.GetWarningTableDataForSerial(SessionStorage.SelectedFSUId, SessionStorage.selected_hhd_id, block.ToString(), serialNo);
                    if (warningResult != null)
                    {
                        foreach (var warning in warningResult)
                        {
                            warning.is_deleted = true;
                        }
                        await dQ.UpsertWarningAsync(warningResult);
                    }
                }

            }
            catch (Exception)
            {
            }
        }


        public async Task<List<Tbl_Warning?>> GetAllWarning(string schedule = "NHIS")
        {
            List<Tbl_Warning?> warningList = new List<Tbl_Warning?>();
            try
            {
                if (schedule == "0")
                {
                    List<Tbl_Warning> data = await dQ.GetWarningList(schedule: schedule);
                    warningList.AddRange(data);
                }
                else
                {
                    foreach (var item in selected_HHdList)
                    {
                        List<Tbl_Warning> data = await dQ.GetWarningList(item.Block_7_3.GetValueOrDefault(), schedule: schedule);
                        warningList.AddRange(data);
                    }
                }
                    
            }
            catch (Exception)
            {

            }
            
            return warningList;
        }


        public async Task<int> ValidateWarning(Tbl_Warning warning)
        {
            int result = 0;
            List <Tbl_Warning> list = new List<Tbl_Warning>();
            try
            {
                if (warning != null)
                {
                    if (warningcoment.remarks == null || warningcoment.remarks == string.Empty)
                    {
                        result = -1;
                    }
                    else
                    {
                        int child_srl = 1;
                        var childComments = await dQ.GetChildCommentsAsync(warning.id);
                        if (childComments != null && childComments.Count > 0)
                        {
                            child_srl = childComments.Max(x => x.serial_number.GetValueOrDefault()) + 1;
                        }

                        int status =
            SessionStorage.role_name == CommonConstants.ROLE_NAME_JSO ? 2 :
            SessionStorage.role_name == CommonConstants.ROLE_NAME_SSO ? (Is_Accepted ? 4 : 3) :
            (Is_Accepted ? 5 : 3);
                        // Update parent warning
                        warning.warning_status =
                            SessionStorage.role_name == CommonConstants.ROLE_NAME_JSO ? 2 :
                            SessionStorage.role_name == CommonConstants.ROLE_NAME_SSO ? Is_Accepted ? 4 : 3 :
                            Is_Accepted ? 5 : 3;

                        list.Add(warning);

                        Tbl_Warning? lastChild = childComments?
            .OrderByDescending(x => x.serial_number)
            .FirstOrDefault();

                        bool updateSameUser =
                            lastChild != null &&
                            lastChild.user_name == SessionStorage.user_name &&
                            lastChild.role_code == SessionStorage.user_role;

                        if (updateSameUser && lastChild != null)
                        {
                            // 🔁 UPDATE existing child
                            lastChild.remarks = warningcoment.remarks;
                            lastChild.warning_status = status;
                            lastChild.survey_timestamp = DateTime.Now;

                            list.Add(lastChild);
                        }
                        else
                        {
                            // ➕ INSERT new child
                            int child_serial = lastChild?.serial_number.GetValueOrDefault() + 1 ?? 1;

                            Tbl_Warning child = new Tbl_Warning
                            {
                                id = Guid.NewGuid(),
                                parent_comment_id = warning.id,
                                block = warning.block,
                                remarks = warningcoment.remarks,
                                warning_status = status,
                                hhd_id = warning.hhd_id,
                                role_code = SessionStorage.user_role,
                                user_name = SessionStorage.user_name,
                                serial_number = child_srl
                            };

                            list.Add(child);
                        }

                        result = await dQ.UpsertWarningAsync(list);
                        if (result>0)
                        {
                            Is_Accepted = false;
                            IsRejected = false;
                            warningcoment = new();
                            warningcoment.remarks = string.Empty;
                        }
                    }
                }
            }
            catch(Exception) 
            {
                return result;
            }
            return result;
        }
    }
}
