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
        private readonly IToastService toastService = DependencyService.Get<IToastService>();
        public int? Serialnumber { get; set; }
        public int? trip_serial_number { get; set; }
        public Guid ID { get; set; } = Guid.Empty;
        public string item_no { get; set; }
        DBQueries dQ = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action NotifyUiUpdate;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            NotifyUiUpdate?.Invoke();
        }

        public async Task<List<Tbl_Comments>> LoadData(string block)
        {
            List<Tbl_Comments> CommentList;
            try
            {
                CommentList = await dQ.GetItemsAsync(block);
                return CommentList;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async void LoadDataById(Tbl_Warning data)
        {
            //Tbl_Warning CommentList = new();
            //try
            //{
            //    CommentList = await dQ.GetItemsAsyncById(data);
            //    comment = CommentList.comment;
            //    Serialnumber = CommentList.serial_number;
            //    trip_serial_number = CommentList.trip_serial_number;
            //    item_no = CommentList.item_no ?? string.Empty;
            //    ID = CommentList.id;
            //    OnPropertyChanged(nameof(comment));
            //    OnPropertyChanged(nameof(Serialnumber));
            //    OnPropertyChanged(nameof(trip_serial_number));
            //    OnPropertyChanged(nameof(item_no));
            //    OnPropertyChanged(nameof(ID));
            //    return;

            //}
            //catch (Exception)
            //{
            //    return;
            //}
        }

        public async Task<int> Save(string block, string? item_no = null)
        {
            try
            {   
                Tbl_Warning Dto = new();
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
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> Delete(Tbl_Comments data)
        {
            try
            {
                var Result = await dQ.DeleteComments(data);
                return Result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
