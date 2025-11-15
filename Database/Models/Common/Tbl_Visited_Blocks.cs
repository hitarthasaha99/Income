using SQLite;

namespace Income.Database.Models.Common
{
    public class Tbl_Visited_Blocks
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public string? block_uri { get; set; }
        public string? block_title { get; set; }
        public string? block_code { get; set; }
        public int fsu_id { get; set; }
        public int hhd_id { get; set; }
    }
}
