using Income.Database.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common.HIS2026
{
    public class Tbl_Lookup_NIC
    {
        public int? id { get; set; }
        public string? lookup_type { get; set; }
        public string? title { get; set; }
    }

    public class Block_4_Constants
    {
        public static readonly List<Tbl_Lookup> Q4_4_AgriculturalActivities =
        [
            new() { id = 1, title = "Cultivation of crops / fruits / vegetables / spices" },
            new() { id = 2, title = "Cultivation of flowers" },
            new() { id = 3, title = "Animal husbandry" },
            new() { id = 4, title = "Fisheries" },
            new() { id = 5, title = "Agroforestry activity" },
            new() { id = 6, title = "Others (bee keeping, sericulture, lac culture, ancillary etc.)" },
        ];

        public static List<Tbl_Lookup> Q4_8 =
        [
            new() { id = 1, title = "For agricultural uses : Crop production -1" },
            new() { id = 2, title = "For Animal husbandry / dairy - 2" },
            new() { id = 3, title = "For Other agricultural activity - 3" },
            new() { id = 4, title = "For Non-agricultural activity - 4" },
            new() { id = 5, title = "For Residential land including homestead - 5" },
            new() { id = 9, title = "Other land - 9" },
        ];

        public static readonly List<Tbl_Lookup> Q4_9 =
        [
            new() { id = 1, title = "yes: on owned & possessed - 1" },
            new() { id = 2, title = "yes: on leased-in - 2" },
            new() { id = 3, title = "yes: on both 1 & 2 - 3" },
            new() { id = 4, title = "no - 4" },
        ];

        public static readonly List<Tbl_Lookup> Q4_10 =
        [
            new() { id = 1, title = "agricultural activity : crop production - 1" },
            new() { id = 2, title = "for animal husbandry / dairy - 2" },
            new() { id = 3, title = "other agricultural activity - 3" },
            new() { id = 4, title = "non-agricultural activity - 4" },
        ];

        // Q4.11: Type of dwelling unit
        public static readonly List<Tbl_Lookup> Q4_11 =
        [
            new() { id = 1, title = "Owned - 1" },
            new() { id = 2, title = "Hired - 2" },
            new() { id = 3, title = "Provided by the employer - 3" },
            new() { id = 4, title = "Others - 4" },
            new() { id = 5, title = "No dwelling unit - 5" },
        ];

        // Q4.13: Type of structure of the dwelling unit
        public static readonly List<Tbl_Lookup> Q4_13 =
        [
            new() { id = 1, title = "Independent house - 1" },
            new() { id = 2, title = "Flat - 2" },
            new() { id = 3, title = "Others - 3" },
        ];

        // Q4.15: Whether the household has any outstanding loan
        public static readonly List<Tbl_Lookup> Q4_15 =
        [
            new() { id = 1, title = "Yes - 1" },
            new() { id = 2, title = "No - 2" },
        ];

        // Q4.16: Purpose of the loan
        public static readonly List<Tbl_Lookup> Q4_16 =
        [
            new() { id = 1, title = "Purchase and/or construction of land/house/building/flat - 1" },
            new() { id = 2, title = "To meet expenditure of economic activity - 2" },
            new() { id = 3, title = "Both 1 & 2 - 3" },
        ];

        public static readonly List<Tbl_Lookup_NIC> NIC_CODES =
        [
            new() { id = 10, title = "Manufacture of food products - 10" },
            new() { id = 11, title = "Manufacture of beverages - 11" },
            new() { id = 12, title = "Manufacture of tobacco products - 12" },
            new() { id = 13, title = "Manufacture of textiles - 13" },
            new() { id = 14, title = "Manufacture of wearing apparel - 14" },
            new() { id = 15, title = "Manufacture of leather and related products - 15" },
            new() { id = 16, title = "Manufacture of wood and of products of wood and cork, except furniture; manufacture of articles of straw and plaiting materials - 16" },
            new() { id = 17, title = "Manufacture of paper and paper products - 17" },
            new() { id = 18, title = "Printing and reproduction of recorded media - 18" },
            new() { id = 19, title = "Manufacture of coke and refined petroleum products - 19" },
            new() { id = 20, title = "Manufacture of chemicals and chemical products - 20" },
            new() { id = 21, title = "Manufacture of basic pharmaceutical products and pharmaceutical preparations - 21" },
            new() { id = 22, title = "Manufacture of rubber and plastics products - 22" },
            new() { id = 23, title = "Manufacture of other non-metallic mineral products - 23" },
            new() { id = 24, title = "Manufacture of basic metals - 24" },
            new() { id = 25, title = "Manufacture of fabricated metal products, except machinery and equipment - 25" },
            new() { id = 26, title = "Manufacture of computer, electronic and optical products - 26" },
            new() { id = 27, title = "Manufacture of electrical equipment - 27" },
            new() { id = 28, title = "Manufacture of machinery and equipment n.e.c. - 28" },
            new() { id = 29, title = "Manufacture of motor vehicles, trailers and semi-trailers - 29" },
            new() { id = 30, title = "Manufacture of other transport equipment - 30" },
            new() { id = 31, title = "Manufacture of furniture - 31" },
            new() { id = 32, title = "Other manufacturing - 32" },
            new() { id = 33, title = "Repair, maintenance and installation of machinery and equipment - 33" },

            new() { id = 35, title = "Electricity, gas, steam and air conditioning supply - 35" },
            new() { id = 36, title = "Water supply, sewerage, waste management and remediation activities - 36" },
            new() { id = 37, title = "Sewerage - 37" },
            new() { id = 38, title = "Waste collection, treatment and disposal, and recovery activities - 38" },
            new() { id = 39, title = "Remediation and other waste management service activities - 39" },

            new() { id = 41, title = "Construction of residential and non-residential buildings - 41" },
            new() { id = 42, title = "Civil engineering - 42" },
            new() { id = 43, title = "Specialized construction activities - 43" },

            new() { id = 46, title = "Wholesale trade - 46" },
            new() { id = 47, title = "Retail trade - 47" },

            new() { id = 49, title = "Land transport and transport via pipelines - 49" },
            new() { id = 50, title = "Water transport - 50" },
            new() { id = 52, title = "Warehousing and support activities for transportation - 52" },
            new() { id = 53, title = "Postal and courier activities - 53" },

            new() { id = 55, title = "Accommodation - 55" },
            new() { id = 56, title = "Food and beverage service activities - 56" },

            new() { id = 58, title = "Publishing activities - 58" },
            new() { id = 59, title = "Motion picture, video and television programme production, sound recording and music publishing activities - 59" },
            new() { id = 60, title = "Programming, broadcasting, news agency and other content distribution activities - 60" },
            new() { id = 61, title = "Telecommunications - 61" },

            new() { id = 62, title = "Computer programming, consultancy and related activities - 62" },
            new() { id = 63, title = "Computer infrastructure, data processing, hosting, and other information service activities - 63" },

            new() { id = 64, title = "Financial service activities, except insurance and pension funding - 64" },
            new() { id = 65, title = "Insurance, reinsurance and pension funding, except compulsory social security - 65" },
            new() { id = 66, title = "Activities auxiliary to financial service and insurance activities - 66" },

            new() { id = 68, title = "Real estate activities - 68" },
            new() { id = 69, title = "Legal and accounting activities - 69" },
            new() { id = 70, title = "Activities of head offices; management consultancy activities - 70" },
            new() { id = 71, title = "Architectural and engineering activities; technical testing and analysis - 71" },
            new() { id = 72, title = "Scientific research and development - 72" },
            new() { id = 73, title = "Activities of advertising, market research and public relations - 73" },
            new() { id = 74, title = "Other professional, scientific and technical activities - 74" },
            new() { id = 75, title = "Veterinary activities - 75" },

            new() { id = 77, title = "Rental and leasing activities - 77" },
            new() { id = 78, title = "Employment activities - 78" },
            new() { id = 79, title = "Travel agency, tour operator and other travel related activities - 79" },
            new() { id = 80, title = "Investigation and security activities - 80" },
            new() { id = 81, title = "Services to buildings and landscape activities - 81" },
            new() { id = 82, title = "Office administrative, office support and other business support activities - 82" },

            new() { id = 85, title = "Education - 85" },
            new() { id = 86, title = "Human health activities - 86" },
            new() { id = 87, title = "Residential care activities - 87" },
            new() { id = 88, title = "Social work activities without accommodation - 88" },

            new() { id = 90, title = "Arts creation and performing arts activities - 90" },
            new() { id = 91, title = "Libraries, archives, museum and other cultural activities - 91" },
            new() { id = 93, title = "Sports activities and amusement and recreation activities - 93" },
            new() { id = 94, title = "Activities of membership organizations - 94" },
            new() { id = 95, title = "Repair and maintenance of computers, personal and household goods and motor vehicles and motorcycles - 95" },
            new() { id = 96, title = "Personal service activities - 96" },

            new() { id = 0,  title = "Cotton ginning, cleaning and bailing (016302) - 00" },

            new() { id = -1, title = "Others" }
        ];
    }
}
