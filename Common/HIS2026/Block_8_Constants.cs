using Income.Database.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common.HIS2026
{
    public static class Block_8_Constants
    {
        public static readonly List<Tbl_Lookup> Q8_6_Schemes =
        [
            new() { id = 1,  title = "Old age pension" },
            new() { id = 2,  title = "Widow/destitute pension" },
            new() { id = 3,  title = "Disability pension" },
            new() { id = 4,  title = "Unemployment allowance" },
            new() { id = 5,  title = "Kisan Samman Nidhi Yojana" },
            new() { id = 6,  title = "Namo Shetkari Maha Sanman Nidhi Yojana - Maharashtra" },
            new() { id = 7,  title = "DBT for food grains (Chandigarh, Puducherry, DNH & DD)" },
            new() { id = 8,  title = "Pension to meritorious sportspersons" },
            new() { id = 9,  title = "Pension scheme for financial assistance for veteran artists" },

            // State Government Schemes
            new() { id = 10, title = "Shilpi Pension Scheme - Assam" },
            new() { id = 11, title = "Shravanbal Seva State Pension Scheme - Maharashtra" },
            new() { id = 12, title = "Mukhyamantri Ladli Behna Yojana - Madhya Pradesh" },
            new() { id = 13, title = "Laxmi Bhandar Yojana (SC/ST) – West Bengal" },
            new() { id = 14, title = "Laxmi Bhandar Yojana (Others) – West Bengal" },
            new() { id = 15, title = "Griha Aadhar Scheme - Goa" },
            new() { id = 16, title = "Orunodoi Scheme - Assam" },
            new() { id = 17, title = "Gruha Lakshmi - Karnataka" },
            new() { id = 18, title = "Kalaignar Magalir Urimai Thittam - Tamil Nadu" },
            new() { id = 19, title = "Mahalakshmi Scheme - Telangana" },
            new() { id = 20, title = "Mahatari Vandan Yojana - Chhattisgarh" },
            new() { id = 21, title = "Maiya Samman Yojana - Jharkhand" },
            new() { id = 22, title = "Mukhya Mantri Majhi Ladki Bahin Yojana - Maharashtra" },
            new() { id = 23, title = "Subhadra Yojana - Odisha" },
            new() { id = 24, title = "Indira Gandhi Pyari Behna Sukh Samman Nidhi Yojana – Himachal Pradesh" },
            new() { id = 25, title = "Mahila Samriddhi Yojana - Delhi" },
            new() { id = 26, title = "Lado Lakshmi Yojana - Haryana" },
            new() { id = 27, title = "Ladli Social Security Allowance Scheme - Haryana" },
            new() { id = 28, title = "Aadabidda Nidhi Scheme – Andhra Pradesh" },
            new() { id = 29, title = "Samajik Seva Bhatta Scheme - Sikkim" },
            new() { id = 30, title = "Sikkim Unmarried Women Pension Scheme" },
            new() { id = 31, title = "Mukhyamantri COVID-19 Parivar Arthik Sahayata Yojana" },
            new() { id = 32, title = "Pension to journalist/photojournalist" },
            new() { id = 33, title = "Matribhasha Satyagrahi Pension Scheme - Haryana" },
            new() { id = 34, title = "Allowance to Eunuchs Scheme" },
            new() { id = 35, title = "Leprosy Pension Scheme" },
            new() { id = 36, title = "Other cash transfer from government" }
        ];

    }
}
