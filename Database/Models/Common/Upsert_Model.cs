using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Upsert_Model
    {
        public int parent_id { get; set; }
        public Guid survey_id { get; set; }
        public string fsu_id { get; set; }
        public int visit_no { get; set; }
        public int quator { get; set; }
        public string user_id { get; set; }
        public int lookup_fsu_submit_status { get; set; }
        public int lookup_fsu_sub_submit_status { get; set; }
        public string submitted_json_data { get; set; }
        public int sso_id { get; set; }
        public int dpd_id { get; set; }
        public Guid id { get; set; } = Guid.Empty;
        public Guid created_by { get; set; }
        public DateTime created_on { get; set; }
        public Guid modified_by { get; set; }
        public DateTime modified_on { get; set; } = DateTime.Now;
        public bool isDeleted { get; set; }
        public bool isActive { get; set; }
    }
}
