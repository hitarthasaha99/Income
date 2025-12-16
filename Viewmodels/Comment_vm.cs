using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorBootstrap;
using Blazored.Toast.Services;
using Income.Common;
using Income.Database.Models.Common;
using Income.Database.Queries;
using static Blazored.Toast.Services.IToastService;


namespace Income.Viewmodels
{
    public partial class Comment_vm : INotifyPropertyChanged
    {
        public Modal AddModal = default;
        public string? comment { get; set; }
        public bool ShowError = false;
        public bool ShowMemberError = false;
        public bool ShowItemError = false;
        private readonly IToastService toastService;
        public int? Serialnumber { get; set; }
        public int? trip_serial_number { get; set; }
        public Guid ID { get; set; } = Guid.Empty;
        public string item_no { get; set; }
        DBQueries dQ = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action NotifyUiUpdate;
        public Tbl_Warning? editObject;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            NotifyUiUpdate?.Invoke();
        }

        public Comment_vm()
        {
            toastService = DependencyService.Get<IToastService>();
        }

        public async Task<List<Tbl_Warning>> LoadData(string block)
        {
            List<Tbl_Warning> CommentList;
            try
            {
                CommentList = await dQ.GetCommentsAsync(block);
                return CommentList;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async void LoadDataById(Tbl_Warning data)
        {
            editObject = new();
            try
            {
                editObject = await dQ.FetchByIdAsync<Tbl_Warning>(data.id);
                comment = editObject.warning_message;
                Serialnumber = editObject.serial_number;
                item_no = editObject.item_no ?? string.Empty;
                ID = editObject.id;
                OnPropertyChanged(nameof(comment));
                OnPropertyChanged(nameof(Serialnumber));
                OnPropertyChanged(nameof(trip_serial_number));
                OnPropertyChanged(nameof(item_no));
                OnPropertyChanged(nameof(ID));
                return;

            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task<int> Save(string block, bool update = false)
        {
            try
            {
                if(update)
                {
                    if (editObject != null)
                    {
                        editObject.warning_message = comment;
                        editObject.item_no = item_no;
                        editObject.serial_number = Serialnumber ?? 0;
                        var data = await dQ.SaveAsync<Tbl_Warning>(editObject);
                        return data;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    Tbl_Warning Dto = new();
                    Dto.warning_type = 99;
                    Dto.warning_message = comment;
                    Dto.item_no = item_no;
                    Dto.serial_number = Serialnumber ?? 0;
                    Dto.block = block;
                    Dto.user_id = SessionStorage.__user_id;
                    Dto.user_name = SessionStorage.user_name;
                    Dto.created_on = DateTime.Now;
                    Dto.role_code = SessionStorage.user_role;
                    Dto.warning_status = 3;

                    if (!string.IsNullOrEmpty(comment) && !string.IsNullOrWhiteSpace(comment))
                    {
                        var data = await dQ.SaveAsync<Tbl_Warning>(Dto);
                        return data;
                    }
                    else
                    {
                        toastService?.ShowError("Please enter some comment to save!");
                        return 0;
                    }
                }
                    
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                editObject = null;
            }
        }

        public async Task Delete(Tbl_Warning data)
        {
            try
            {
                await dQ.DeleteEntryAsync<Tbl_Warning>(data.id);
            }
            catch (Exception ex)
            {
                toastService.ShowError("Could not delete comment");
            }
        }

    }
}
