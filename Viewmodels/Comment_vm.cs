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
                //if (CommentList == null || CommentList.Count == 0)
                //{
                //    Serialnumber = 1;
                //}
                //else
                //{
                //    Serialnumber = CommentList.Count + 1;
                //}
                return CommentList;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async void LoadDataById(Tbl_Comments data)
        {
            Tbl_Comments CommentList = new();
            try
            {
                CommentList = await dQ.GetItemsAsyncById(data);
                comment = CommentList.comment;
                Serialnumber = CommentList.serial_number;
                trip_serial_number = CommentList.trip_serial_number;
                item_no = CommentList.item_no ?? string.Empty;
                ID = CommentList.id;
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

        public async Task<int> Save(string block)
        {
            try
            {   
                Tbl_Comments Dto = new();
                Dto.comment = comment;
                Dto.item_no = block == "4.2" ? "1" : item_no;
                Dto.serial_number = Serialnumber ?? 0;
                Dto.trip_serial_number = trip_serial_number ?? null;
                Dto.id = ID == null || ID == Guid.Empty ? Guid.NewGuid() : ID;
                Dto.block = block;
                Dto.commented_by = SessionStorage.__user_id;
                Dto.created_on = DateTime.Now;
                Dto.commenter_full_name = SessionStorage.full_name;
                Dto.role_code = SessionStorage.user_role;
                Dto.Is_accepted = false; // Default value
                Dto.Is_rejected = true; // Default value
                Dto.comment_status = 1;
                if (!string.IsNullOrEmpty(comment) && !string.IsNullOrWhiteSpace(comment))
                {
                    var data = await dQ.SaveBlockTbl_Comments(Dto);
                    ID = Guid.Empty;
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
