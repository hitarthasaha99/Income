using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Tbl_Fsu_List : Tbl_Base
    {
        public string? iv_unit { get; set; }
        public string? block { get; set; }
        public int? total_hhd { get; set; }
        public int? completed_hhd { get; set; }
        public int? framepop { get; set; }
        public bool? is_selection_done { get; set; }
        public int? lookup_fsu_submit_status { get; set; }
        public string? st { get; set; }
        public string? sro { get; set; }
        public string? nss_reg { get; set; }
        public string? strm { get; set; }
        public string? sstrm { get; set; }
        public string? dc { get; set; }
        public string? dn { get; set; }
        public string? tehn { get; set; }
        public string? fc { get; set; }
        public string? hh { get; set; }
        public string? fsuname { get; set; }
        public string? stn { get; set; }
        public string? sample { get; set; }
        public int? totalsu { get; set; }
        public int? selsu { get; set; }
        public int? subrnd { get; set; }
        public int? sec { get; set; }
        public int? quarter { get; set; }
        public int? lookupFsuSubmitStatus { get; set; }
        public string? enumaretor { get; set; }
        public string? supervisor { get; set; }
        public int? assigntoEnumaretor { get; set; }
        public int? returnBacktoEnumaretor { get; set; }
        public int? submittedbyEnumaretorToSuperviosr { get; set; }
        public int? submiitedbySuperviosrToDs { get; set; }
        public int? acceptedByDS { get; set; }
        public int? returntoSuperviosrbyDS { get; set; }
        public int? casualty { get; set; }
        public string? fsu_houses { get; set; }
        [Ignore]
        public List<fsu_houses>? fsuHouses { get; set; }
        public int? isSubstituteDone { get; set; } = 0;
        public int? updatelistingcount { get; set; } = 0;
        [JsonIgnore]
        public bool NeedDownload { get; set; } = false;

    }

    public class fsu_houses
    {
        public Guid id { get; set; }
        public int tenant_id { get; set; }
        public int fsu_id { get; set; }
        public int hhd_id { get; set; }
        public Guid survey_id { get; set; }
        public int lookup_submit_status { get; set; }
    }
}
