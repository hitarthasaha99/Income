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
            new() { id = 1,  title = "01 - Old age pension" },
            new() { id = 2,  title = "02 - Widow/destitute pension" },
            new() { id = 3,  title = "03 - Disability pension" },
            new() { id = 4,  title = "04 - Unemployment allowance" },
            new() { id = 5,  title = "05 - Kisan Samman Nidhi Yojana" },
            new() { id = 6,  title = "06 - Namo Shetkari Maha Sanman Nidhi Yojana - Maharashtra" },
            new() { id = 7,  title = "07 - DBT for food grains (Chandigarh, Puducherry, DNH & DD)" },
            new() { id = 8,  title = "08 - Pension to meritorious sportspersons" },
            new() { id = 9,  title = "09 - Pension scheme for financial assistance for veteran artists" },

            // State Government Schemes
            new() { id = 10, title = "10 - Shilpi Pension Scheme - Assam" },
            new() { id = 11, title = "11 - Shravanbal Seva State Pension Scheme - Maharashtra" },
            new() { id = 12, title = "12 - Mukhyamantri Ladli Behna Yojana - Madhya Pradesh" },
            new() { id = 13, title = "13 - Laxmi Bhandar Yojana (SC/ST) – West Bengal" },
            new() { id = 14, title = "14 - Laxmi Bhandar Yojana (Others) – West Bengal" },
            new() { id = 15, title = "15 - Griha Aadhar Scheme - Goa" },
            new() { id = 16, title = "16 - Orunodoi Scheme - Assam" },
            new() { id = 17, title = "17 - Gruha Lakshmi - Karnataka" },
            new() { id = 18, title = "18 - Kalaignar Magalir Urimai Thittam - Tamil Nadu" },
            new() { id = 19, title = "19 - Mahalakshmi Scheme - Telangana" },
            new() { id = 20, title = "20 - Mahatari Vandan Yojana - Chhattisgarh" },
            new() { id = 21, title = "21 - Maiya Samman Yojana - Jharkhand" },
            new() { id = 22, title = "22 - Mukhya Mantri Majhi Ladki Bahin Yojana - Maharashtra" },
            new() { id = 23, title = "23 - Subhadra Yojana - Odisha" },
            new() { id = 24, title = "24 - Indira Gandhi Pyari Behna Sukh Samman Nidhi Yojana – Himachal Pradesh" },
            new() { id = 25, title = "25 - Mahila Samriddhi Yojana - Delhi" },
            new() { id = 26, title = "26 - Lado Lakshmi Yojana - Haryana" },
            new() { id = 27, title = "27 - Ladli Social Security Allowance Scheme - Haryana" },
            new() { id = 28, title = "28 - Aadabidda Nidhi Scheme – Andhra Pradesh" },
            new() { id = 29, title = "29 - Samajik Seva Bhatta Scheme - Sikkim" },
            new() { id = 30, title = "30 - Sikkim Unmarried Women Pension Scheme" },
            new() { id = 31, title = "31 - Mukhyamantri COVID-19 Parivar Arthik Sahayata Yojana" },
            new() { id = 32, title = "32 - Pension to journalist/photojournalist" },
            new() { id = 33, title = "33 - Matribhasha Satyagrahi Pension Scheme - Haryana" },
            new() { id = 34, title = "34 - Allowance to Eunuchs Scheme" },
            new() { id = 35, title = "35 - Leprosy Pension Scheme" },
            new() { id = 36, title = "36 - Other cash transfer from government" }
        ];

    }
}
