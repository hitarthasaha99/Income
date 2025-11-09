using Income.Database.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common.HIS2026
{
    public class Block_4_Constants
    {
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
            new() { id = 1, title = "agricultural activivty : crop production - 1" },
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

        public static readonly List<Tbl_Lookup> NIC_CODES =
        [
            new() { id = 10,  title = "Manufacture of Food Products - 10" },
            new() { id = 11,  title = "Manufacture of Beverages - 11" },
            new() { id = 12,  title = "Manufacture of Tobacco Products - 12" },
            new() { id = 13,  title = "Manufacture of Textiles - 13" },
            new() { id = 14,  title = "Manufacture of Wearing Apparel - 14" },
            new() { id = 15,  title = "Manufacture of Leather and Related Products - 15" },
            new() { id = 16,  title = "Manufacture of Wood and of Products of Wood and Cork, except furniture; manufacture of articles of straw and plaiting materials - 16" },
            new() { id = 17,  title = "Manufacture of Paper and Paper Products - 17" },
            new() { id = 18,  title = "Printing and Reproduction of Recorded Media - 18" },
            new() { id = 19,  title = "Manufacture of Coke and Refined Petroleum Products - 19" },
            new() { id = 20,  title = "Manufacture of Chemicals and Chemical Products - 20" },
            new() { id = 21,  title = "Manufacture of Pharmaceuticals, Medicinal Chemical and Botanical Products - 21" },
            new() { id = 22,  title = "Manufacture of Rubber and Plastics Products - 22" },
            new() { id = 23,  title = "Manufacture of Other Non-Metallic Mineral Products - 23" },
            new() { id = 24,  title = "Manufacture of Basic Metals - 24" },
            new() { id = 25,  title = "Manufacture of Fabricated Metal Products, except Machinery and Equipment - 25" },
            new() { id = 26,  title = "Manufacture of Computer, Electronic and Optical Products - 26" },
            new() { id = 27,  title = "Manufacture of Electrical Equipment - 27" },
            new() { id = 28,  title = "Manufacture of Machinery and Equipment n.e.c. - 28" },
            new() { id = 29,  title = "Manufacture of Motor Vehicles, Trailers and Semi-Trailers - 29" },
            new() { id = 30,  title = "Manufacture of Other Transport Equipment - 30" },
            new() { id = 31,  title = "Manufacture of Furniture - 31" },
            new() { id = 32,  title = "Other Manufacturing - 32" },
            new() { id = 33,  title = "Repair and Installation of Machinery and Equipment - 33" },
            new() { id = 35,  title = "Electricity, Gas, Steam and Air Conditioning Supply - 35" },
            new() { id = 36,  title = "Water Collection, Treatment and Supply - 36" },
            new() { id = 37,  title = "Sewerage - 37" },
            new() { id = 38,  title = "Waste Collection, Treatment and Disposal Activities; Materials Recovery - 38" },
            new() { id = 39,  title = "Remediation Activities and Other Waste Management Services - 39" },
            new() { id = 41,  title = "Construction of Buildings - 41" },
            new() { id = 42,  title = "Civil Engineering - 42" },
            new() { id = 43,  title = "Specialized Construction Activities - 43" },
            new() { id = 45,  title = "Wholesale and Retail Trade and Repair of Motor Vehicles and Motorcycles - 45" },
            new() { id = 46,  title = "Wholesale Trade, except of Motor Vehicles and Motorcycles - 46" },
            new() { id = 47,  title = "Retail Trade, except of Motor Vehicles and Motorcycles - 47" },
            new() { id = 49,  title = "Land Transport and Transport via Pipelines - 49" },
            new() { id = 50,  title = "Water Transport - 50" },
            new() { id = 52,  title = "Warehousing and Support Activities for Transportation - 52" },
            new() { id = 53,  title = "Postal and Courier Activities - 53" },
            new() { id = 55,  title = "Accommodation - 55" },
            new() { id = 56,  title = "Food and Beverage Service Activities - 56" },
            new() { id = 58,  title = "Publishing Activities - 58" },
            new() { id = 59,  title = "Motion Picture, Video and Television Programme Production, Sound Recording and Music Publishing Activities - 59" },
            new() { id = 60,  title = "Broadcasting and Programming Activities - 60" },
            new() { id = 61,  title = "Telecommunications - 61" },
            new() { id = 62,  title = "Computer Programming, Consultancy and Related Activities - 62" },
            new() { id = 63,  title = "Information Service Activities - 63" },
            new() { id = 64,  title = "Financial Service Activities, except Insurance and Pension Funding - 64" },
            new() { id = 65,  title = "Insurance, Reinsurance and Pension Funding, except Compulsory Social Security - 65" },
            new() { id = 66,  title = "Other Financial Activities - 66" },
            new() { id = 68,  title = "Real Estate Activities - 68" },
            new() { id = 69,  title = "Legal and Accounting Activities - 69" },
            new() { id = 70,  title = "Activities of Head Offices; Management Consultancy Activities - 70" },
            new() { id = 71,  title = "Architecture and Engineering Activities; Technical Testing and Analysis - 71" },
            new() { id = 72,  title = "Scientific Research and Development - 72" },
            new() { id = 73,  title = "Advertising and Market Research - 73" },
            new() { id = 74,  title = "Other Professional, Scientific and Technical Activities - 74" },
            new() { id = 75,  title = "Veterinary Activities - 75" },
            new() { id = 77,  title = "Rental and Leasing Activities - 77" },
            new() { id = 78,  title = "Employment Activities - 78" },
            new() { id = 79,  title = "Travel Agency, Tour Operator and Other Reservation Service Activities - 79" },
            new() { id = 80,  title = "Security and Investigation Activities - 80" },
            new() { id = 81,  title = "Services to Buildings and Landscape Activities - 81" },
            new() { id = 82,  title = "Office Administrative, Office Support and Other Business Support Activities - 82" },
            new() { id = 85,  title = "Educational Activities - 85" },
            new() { id = 86,  title = "Human Health Activities - 86" },
            new() { id = 87,  title = "Residential Care Activities - 87" },
            new() { id = 88,  title = "Social Work Activities without Accommodation - 88" },
            new() { id = 90,  title = "Creative, Arts and Entertainment Activities - 90" },
            new() { id = 91,  title = "Libraries, Archives, Museums and Other Cultural Activities - 91" },
            new() { id = 93,  title = "Sports Activities and Amusement and Recreation Activities - 93" },
            new() { id = 94,  title = "Activities of Membership Organizations - 94" },
            new() { id = 95,  title = "Repair of Computers and Personal and Household Goods - 95" },
            new() { id = 96,  title = "Other Personal Service Activities - 96" },
            new() { id = 0,   title = "Cotton Ginning, Cleaning and Bailing (01632) - 00" },
            new() { id = 999,  title = "Others" },
        ];
    }
}
