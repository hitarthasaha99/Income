using Income.Database.Models.Common;
using SQLite;
using System.Text.Json.Serialization;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_2_2 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public bool is_enabled { get; set; } = true;
        public bool is_selected { get; set; } = false;
        public int? serial_number { get; set; }
        public string? serial_no_of_hamlets_in_su { get; set; }
        public double Percentage { get; set; }
        public string? HamletName { get; set; }
        public string SampleHgSbNumber { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;
        [JsonIgnore]
        [Ignore]
        public List<SerialPopulation> SerialPopulationsList { get; set; }
    }
    public class SerialPopulation
    {
        public int Id { get; set; }
        public double PopulationPercentage { get; set; }
        public bool IsSelected { get; set; }

    }
}
