using System.Collections.Generic;
using System.Threading.Tasks;
using BootstrapBlazor.Components;
using Income.Common;
using Income.Database.Models.Common;
using Income.Database.Models.SCH0_0;
using Income.Database.Queries;

namespace Income.Viewmodels
{
    public class Warning_VM
    {
        public List<Tbl_Warning> WarningList = [];

        public List<Tbl_Warning> _tempWarnings = [];
        public void AddWarning(string warningMsg, string schedule, string blockNumber, string item)
        {
            Tbl_Warning tbl_Warning = new();
            tbl_Warning.block = blockNumber;
            tbl_Warning.fsu_id = SessionStorage.SelectedFSUId;
            tbl_Warning.hhd_id = SessionStorage.selected_hhd_id;
            tbl_Warning.item_no = item;
            tbl_Warning.warning_status = 1;
            tbl_Warning.role_code = SessionStorage.user_role;
            tbl_Warning.user_name = SessionStorage.full_name;
            tbl_Warning.warning_message = warningMsg;
            tbl_Warning.schedule = schedule;
            _tempWarnings.Add(tbl_Warning);
        }

        DBQueries dQ = new();
        public List<Tbl_Sch_0_0_Block_7>? selected_HHdList = new();
        public bool Is_Accepted { get; set; }
        public bool IsRejected { get; set; }
        public Tbl_Warning warningcoment { get; set; } = new();

        public async Task SaveWarningAsync(bool isAdd, string schedule, string Block, int serialNo)
        {
            var warnings = _tempWarnings;
            List<Tbl_Warning> warningList = [];

            if (!isAdd)
            {
                if (warnings.Count() == 0)
                {
                    //delete all existing
                    foreach (var warning in warnings.Where(x => x.serial_number == serialNo))
                    {
                        warning.is_deleted = true;
                    }
                }
                else
                {
                    foreach (var warning in warnings)
                    {
                        var exist = warnings.Where(x => x.item_no == warning.item_no && (x.is_deleted == false || x.is_deleted == null)).FirstOrDefault();
                        if (exist != null)
                        {
                            warning.id = exist.id;
                            warning.warning_status = exist.warning_status;
                            exist.warning_message = warning.warning_message;
                        }
                    }

                    // Get warnings in Data for the same serial number that are not in _tempWarnings
                    var remainingWarnings = warnings
                        .Where(exist => exist.serial_number == serialNo &&
                                        !warnings.Any(temp => temp.id == exist.id))
                        .ToList();


                    // Delete remaining warnings
                    foreach (var warning in remainingWarnings)
                    {
                        warning.is_deleted = true;
                    }
                }
            }


            foreach (var warning in warnings)
            {
                if (warning.id == Guid.Empty)
                {
                    warning.id = Guid.NewGuid();
                    warning.serial_number = serialNo;
                    warningList.Add(warning);
                }
            }

            await dQ.UpsertWarningAsync(warningList);
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


        public async Task<List<Tbl_Warning?>> GetAllWarning(string schedule = "HIS")
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
            List <Tbl_Warning> List = new List<Tbl_Warning>();
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

                        // Update parent warning
                        warning.warning_status =
                            SessionStorage.role_name == CommonConstants.ROLE_NAME_JSO ? 2 :
                            SessionStorage.role_name == CommonConstants.ROLE_NAME_SSO ? Is_Accepted ? 4 : 3 :
                            Is_Accepted ? 5 : 3;
                        List.Add(warning);

                            // Create child warning
                            Tbl_Warning tbl_Warning = new Tbl_Warning();
                            tbl_Warning.id = Guid.NewGuid();
                            tbl_Warning.block = warning.block;
                            tbl_Warning.remarks = warningcoment.remarks;
                        tbl_Warning.warning_status =
                            SessionStorage.role_name == CommonConstants.ROLE_NAME_JSO ? 2 :
                            SessionStorage.role_name == CommonConstants.ROLE_NAME_SSO ? Is_Accepted ? 4 : 3 :
                            Is_Accepted ? 5 : 3;
                        tbl_Warning.parent_comment_id = warning.id;
                            tbl_Warning.hhd_id = warning.hhd_id;
                           tbl_Warning.role_code = SessionStorage.user_role;
                           tbl_Warning.user_name = SessionStorage.full_name;
                        List.Add(tbl_Warning);
                        
                        result = await dQ.UpsertWarningAsync(List);
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
