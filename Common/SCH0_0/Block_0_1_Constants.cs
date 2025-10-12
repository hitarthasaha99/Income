using Income.Database.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common.SCH0_0
{
    public static class Block_0_1_Constants
    {
        public static readonly List<Tbl_Lookup> Sample =
        [
            new Tbl_Lookup() { title = "1- Central", id = 1 },
            new Tbl_Lookup() { title = "2- State", id = 2 },
        ];

        public static readonly List<Tbl_Lookup> Sector =
        [
            new Tbl_Lookup() { title = "1- Rural", id = 1 },
            new Tbl_Lookup() { title = "2- Urban", id = 2 },
        ];

        public static readonly List<Tbl_Lookup> FrameCode =
        [
            new Tbl_Lookup { title = "16- rural: 2011 census", id = 16 },
            new Tbl_Lookup { title = "15- urban: 2007-12 UFS", id = 15 },
            new Tbl_Lookup { title = "17- urban: 2012-17 UFS", id = 17 },
            new Tbl_Lookup { title = "18- urban: 2017-22 UFS", id = 18 },
            new Tbl_Lookup { title = "19- urban: 2022-27 UFS", id = 19 }
        ];
    }
}
