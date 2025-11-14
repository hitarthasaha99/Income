using Income.Database.Models.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.HIS_2026
{
    public class Tbl_Block_4 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int hhd_id { get; set; }

        // Q4.1 – Household size (auto-populated from Block 3)
        public int? item_1 { get; set; }

        // Q4.2 – Social Group (ST-1, SC-2, OBC-3, Others-9, Not reported-10)
        public int? item_2 { get; set; }

        // Q4.3 – Religion (Hinduism-1, Islam-2, Christianity-3, etc.)
        public int? item_3 { get; set; }

        // Q4.4 – Agricultural activities (multiple select)
        public bool? item_4_1 { get; set; }
        public bool? item_4_2 { get; set; }
        public bool? item_4_3 { get; set; }
        public bool? item_4_4 { get; set; }
        public bool? item_4_5 { get; set; }
        public bool? item_4_6 { get; set; }

        // Q4.5 – Non-agricultural economic activities (table; can be serialized JSON or comma-separated IDs)
        public string? item_5 { get; set; }

        // Q4.6 – Does the household own any land? (Yes-1, No-2)
        public int? item_6 { get; set; }

        // Q4.7 – Total area of all owned land (acre, up to 3 decimal places)
        public decimal? item_7 { get; set; }

        // Q4.8 – Use of land owned (multiple select; store codes as comma-separated string)
        public string? item_8 { get; set; }

        // Q4.9 – Economic activity on any building/structure (1–4)
        public int? item_9 { get; set; }

        // Q4.10 – Type of economic activity (multiple select; store codes as comma-separated string)
        public string? item_10 { get; set; }

        // Q4.11 – Type of dwelling unit (1–5)
        public int? item_11 { get; set; }

        // Q4.12 – Carpet area (sq. ft.)
        public decimal? item_12 { get; set; }

        // Q4.13 – Type of structure (independent house-1, flat-2, others-3)
        public int? item_13 { get; set; }

        // Q4.14 – Number of rooms
        public int? item_14 { get; set; }

        // Q4.15 – Any outstanding loan? (Yes-1, No-2)
        public int? item_15 { get; set; }

        // Q4.16 – Purpose of loan (1–3)
        public int? item_16 { get; set; }

        // Q4.17 – Amount paid towards loan repayment last month (₹)
        public decimal? item_17 { get; set; }

        // Remarks – free-text field for enumerator
        public string? remarks { get; set; }
    }

}
