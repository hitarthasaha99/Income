using SQLite;

namespace Income.Database.Models.Common
{
    public class Tbl_Visited_Blocks
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string? block_uri { get; set; }
        public string? block_title { get; set; }
        public string? block_code { get; set; }
    }
}
