using Income.Database.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common.SCH0_0
{
    public class Block_7_Constants
    {
        public static readonly List<Tbl_Lookup> HighestEdu =
        [
            new Tbl_Lookup() { title = "01- not literate", id = 1 },
            new Tbl_Lookup() { title = "02- literate without formal schooling", id = 2 },
            new Tbl_Lookup() { title = "03- below primary", id = 3 },
            new Tbl_Lookup() { title = "04- primary", id = 4 },
            new Tbl_Lookup() { title = "05- upper primary / middle", id = 5 },
            new Tbl_Lookup() { title = "06- secondary", id = 6 },
            new Tbl_Lookup() { title = "07- higher secondary", id = 7 },
            new Tbl_Lookup() { title = "08- diploma / certificate course", id = 8 },
            new Tbl_Lookup() { title = "10- graduate", id = 10 },
            new Tbl_Lookup() { title = "11- postgraduate and above", id = 11 }
        ];

        public static readonly List<Tbl_Lookup> HHDTypeRural =
        [
            new Tbl_Lookup() { title = "01- self-employed in agriculture", id = 1 },
            new Tbl_Lookup() { title = "02- self-employed in non-agriculture", id = 2 },
            new Tbl_Lookup() { title = "03- regular wage/salary earning", id = 3 },
            new Tbl_Lookup() { title = "04- casual labour", id = 4 },
            new Tbl_Lookup() { title = "05- others", id = 9 }
        ];

        public static readonly List<Tbl_Lookup> HHDTypeUrban =
        [
            new Tbl_Lookup() { title = "01- self-employed", id = 1 },
            new Tbl_Lookup() { title = "02- regular wage/salary earning", id = 2 },
            new Tbl_Lookup() { title = "03- casual labour", id = 3 },
            new Tbl_Lookup() { title = "04- interest, pensioner, remittances earner", id = 4 },
            new Tbl_Lookup() { title = "05- others", id = 9 }
        ];
    }
}
