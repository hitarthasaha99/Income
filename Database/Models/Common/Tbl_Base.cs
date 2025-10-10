using Income.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Tbl_Base
    {
        public Guid? user_id { get; set; } = SessionStorage.__user_id;
        public int? tenant_id { get; set; } = SessionStorage.tenant_id;
        public int? sub_round { get; set; }
        public string? survey_coordinates { get; set; }
        public int fsu_id { get; set; } = SessionStorage.SelectedFSUId;
        public Guid? fsu_ref_id { get; set; }
        public Guid? survey_id { get; set; } = SessionStorage.surveyId;
        public bool? is_deleted { get; set; }
        public int? survey_duration_in_seconds { get; set; }
        public DateTime? survey_timestamp { get; set; }
        public Guid? _ref { get; set; }
    }
}
