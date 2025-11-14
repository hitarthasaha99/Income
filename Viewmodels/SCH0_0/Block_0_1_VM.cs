using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.ComponentModel;
using Income.Common;
using Income.Components.Common;
using Income.Database.Queries;
using Income.Database.Models.SCH0_0;
using Income.Database.Models.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using Income.Common.SCH0_0;

namespace Income.Viewmodels.SCH0_0
{
    public partial class Block_0_1_VM : BaseVM 
    {
        private CommonQueries CommonQueries = new();
        private CommonList CommonList = new();
        private readonly DBQueries DBQueries = new();
        public event Action NotifyUiUpdate;
        private Tbl_Sch_0_0_Block_0_1? _block_0_1 = new();
        public Tbl_Sch_0_0_Block_0_1? block_0_1
        {
            get => _block_0_1;
            set
            {
                if (_block_0_1 != value)
                {
                    _block_0_1 = value;
                    OnPropertyChanged(nameof(block_0_1));
                }
            }
        }

        private List<Tbl_Lookup> _survey_code_list = new();
        public List<Tbl_Lookup> survey_code_list
        {
            get => _survey_code_list;
            set
            {
                if (_survey_code_list != value)
                {
                    _survey_code_list = value;
                    OnPropertyChanged(nameof(survey_code_list));
                }
            }
        }

        private List<Tbl_Lookup>? _substitution_reason_list = new();
        public List<Tbl_Lookup>? substitution_reason_list
        {
            get => _substitution_reason_list;
            set
            {
                if (_substitution_reason_list != value)
                {
                    _substitution_reason_list = value;
                    OnPropertyChanged(nameof(substitution_reason_list));
                }
            }
        }

        public bool is_substitution_reason_disable { get; set; } = true;
        public ValidationComponent approx_present_pop = new();
        public ValidationComponent survey_code = new();
        public ValidationComponent substitution_reason = new();
        public DBQueries SCH_0_0_Queries = new();
        public bool is_error = false;
        public bool EnablePopulation = false;
        public bool EnableItem16 = false;

        [ObservableProperty]
        private string _block_1_4_Selected = string.Empty;
        [ObservableProperty]
        private string _block_1_5_Selected = string.Empty;
        [ObservableProperty]
        private string _block_1_12_Selected = string.Empty;
        [ObservableProperty]
        private string? _block_1_16_Selected = string.Empty;
        [ObservableProperty]
        private string? _block_1_17_Selected = string.Empty;
        [ObservableProperty]
        private bool _showItem17 = false;

        public Block_0_1_VM()
        {
        }

        public async Task Init()
        {
            survey_code_list = CommonList.LOOKUP_CONST_SURVEY_CODE;
            substitution_reason_list = CommonList.LOOKUP_CONST_SUBSTITUTION_REASON;
            Tbl_Visited_Blocks vb = new Tbl_Visited_Blocks
            {
                block_title = "Sch 0.0 Block 1",
                block_uri = "/dashboard/sch0.0/block0_1",
                block_code = CommonEnum.sch_0_0_block_1.ToString()
            };
            await CommonQueries.InsertVisitedBlock(vb);
            block_0_1 = await SCH_0_0_Queries.FetchBlock1();
            if (block_0_1 == null)
            {
                block_0_1 = new Tbl_Sch_0_0_Block_0_1();
                var getfsuresponse = await CommonQueries.FetchFsuByFsuId(SessionStorage.SelectedFSUId);
                if (getfsuresponse != null)
                {
                    block_0_1.state_code = getfsuresponse.st;
                    block_0_1.Block_0_1 = getfsuresponse.stn;
                    block_0_1.Block_0_2 = getfsuresponse.dn;
                    block_0_1.Block_0_3 = getfsuresponse.tehn;
                    block_0_1.Block_0_4 = getfsuresponse.fsuname;
                    block_0_1.Block_0_5 = getfsuresponse.iv_unit;
                    block_0_1.Block_0_6 = getfsuresponse.block;
                    block_0_1.Block_1_1 = getfsuresponse.fsu_id.ToString();
                    block_0_1.Block_1_2 = 80;
                    block_0_1.Block_1_3 = "00";
                    block_0_1.Block_1_4 = Block_0_1_Constants.Sample.FirstOrDefault(x => x.id == Convert.ToInt16(getfsuresponse.sample))?.id ?? 0;
                    Block_1_4_Selected = Block_0_1_Constants.Sample.FirstOrDefault(x => x.id == Convert.ToInt16(getfsuresponse.sample))?.title ?? string.Empty;
                    block_0_1.Block_1_5 = Block_0_1_Constants.Sector.FirstOrDefault(x => x.id == Convert.ToInt16(getfsuresponse.sec))?.id;
                    Block_1_5_Selected = Block_0_1_Constants.Sector.FirstOrDefault(x => x.id == Convert.ToInt16(getfsuresponse.sec))?.title ?? string.Empty;
                    block_0_1.Block_1_6 = getfsuresponse.nss_reg;
                    block_0_1.Block_1_7 = getfsuresponse.dc;
                    block_0_1.Block_1_8 = getfsuresponse.strm;
                    block_0_1.Block_1_9 = getfsuresponse.sstrm;
                    block_0_1.Block_1_10 = getfsuresponse.subrnd;
                    block_0_1.Block_1_11 = getfsuresponse.sro;
                    block_0_1.Block_1_12 = Block_0_1_Constants.FrameCode.FirstOrDefault(x => x.id == Convert.ToInt16(getfsuresponse.fc))?.id ?? 0;
                    block_0_1.Block_1_13 = getfsuresponse.framepop;
                    block_0_1.Block_1_14 = null;
                    block_0_1.Block_1_15 = getfsuresponse.totalsu;
                    Block_1_12_Selected = Block_0_1_Constants.FrameCode.FirstOrDefault(x => x.id == Convert.ToInt16(getfsuresponse.fc))?.title ?? string.Empty;
                    Block_1_16_Selected = null;
                    Block_1_17_Selected = null;
                    block_0_1.remarks = string.Empty;
                    EnableItem16 = true;
                    EnablePopulation = true;
                    ShowItem17 = false;
                    //DisplayCoordinates = FormattedCoordinates;
                }
                //_surveyTimeStamp = DateTime.Now.ToString(Constants.DateTimeFormat);
            }
            else
            {
                Block_1_4_Selected = Block_0_1_Constants.Sample.FirstOrDefault(x => x.id == Convert.ToInt16(block_0_1.Block_1_4))?.title ?? string.Empty;
                Block_1_5_Selected = Block_0_1_Constants.Sector.FirstOrDefault(x => x.id == Convert.ToInt16(block_0_1.Block_1_5))?.title ?? string.Empty;
                Block_1_12_Selected = Block_0_1_Constants.FrameCode.FirstOrDefault(x => x.id == Convert.ToInt16(block_0_1.Block_1_12))?.title ?? string.Empty;
                Block_1_16_Selected = block_0_1.Block_1_16 != null ? CommonList.LOOKUP_CONST_SURVEY_CODE.FirstOrDefault(x => x.id == Convert.ToInt16(block_0_1.Block_1_16))?.title : string.Empty;
                Block_1_17_Selected = block_0_1.Block_1_17 != null ? CommonList.LOOKUP_CONST_SUBSTITUTION_REASON.FirstOrDefault(x => x.id == Convert.ToInt16(block_0_1.Block_1_17))?.title : string.Empty;
                ShowItem17 = block_0_1.Block_1_17 != null;
                EnablePopulation = !SessionStorage.selection_done;
            }
            OnPropertyChanged(nameof(block_0_1));
        }

        private bool _isAdd = true;
        private Guid _id;

        public async Task<bool> Save()
        {
            try
            {
                var dataObject = await SCH_0_0_Queries.FetchBlock1();
                if (dataObject != null)
                {
                    _isAdd = false;
                    _id = dataObject.id;
                }

                var identificationData = new Tbl_Sch_0_0_Block_0_1();
                identificationData = block_0_1;
                if (_isAdd)
                {
                    block_0_1.id = Guid.NewGuid();
                    block_0_1.survey_timestamp = DateTime.Now;
                    await SCH_0_0_Queries.SaveBlock1(block_0_1);
                }
                else
                {
                    await SCH_0_0_Queries.SaveBlock1(block_0_1);
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SurveyCodeSelectionChanged(ChangeEventArgs args)
        {
            try
            {
                var val = args.Value.ToString();
                EnablePopulation = (val == "1" || val == "4") && !SessionStorage.selection_done;
                ShowItem17 = val == "4" || val == "5" || val == "6" || val == "7";
                if (!EnablePopulation)
                {
                    block_0_1.Block_1_14 = 0;
                }
                if (!ShowItem17)
                {
                    block_0_1.Block_1_17 = null;
                    block_0_1.remarks_block_1_17 = string.Empty;
                }
                block_0_1.Block_1_16 = Convert.ToInt16(val);
                OnPropertyChanged(nameof(block_0_1));
                OnPropertyChanged(nameof(EnablePopulation));
                NotifyUiUpdate.Invoke();
            }
            catch (Exception ex)
            {
                block_0_1.Block_1_16 = null;
            }
        }
    }
}
