using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common
{
    public static class ObjectCloneHelper
    {
        public static Tbl_Block_5 CloneTblHISBlock5(this Tbl_Block_5 src)
        {
            return new Tbl_Block_5
            {
                id = src.id,
                hhd_id = src.hhd_id,
                serial_number = src.serial_number,
                serial_member = src.serial_member,
                fk_block_3 = src.fk_block_3,
                item_1 = src.item_1,
                item_2 = src.item_2,
                item_3_i = src.item_3_i,
                item_3_ii = src.item_3_ii,
                item_4_i = src.item_4_i,
                item_4_ii = src.item_4_ii,
                item_5 = src.item_5,
                item_6 = src.item_6,
                item_7 = src.item_7,
                item_8 = src.item_8,
                item_9 = src.item_9,
                isUpdated = src.isUpdated
            };
        }

        public static Tbl_Block_6 CloneTblHISBlock6(this Tbl_Block_6 src)
        {
            return new Tbl_Block_6
            {
                id = src.id,
                hhd_id = src.hhd_id,
                serial_number = src.serial_number,
                serial_member = src.serial_member,
                fk_block_3 = src.fk_block_3,
                item_1 = src.item_1,
                item_2 = src.item_2,
                item_3 = src.item_3,
                item_4 = src.item_4,
                isUpdated = src.isUpdated
            };
        }

        public static Tbl_Block_7c_NIC CloneTbl7NICList(this Tbl_Block_7c_NIC src)
        {
            return new Tbl_Block_7c_NIC
            {
                id = src.id,
                hhd_id = src.hhd_id,
                SerialNumber = src.SerialNumber,
                ActivityName = src.ActivityName,
                NicCode = src.NicCode,
                GrossValue = src.GrossValue,
            };
        }

        public static Tbl_Block_7c_Q10 CloneTbl7Q10List(this Tbl_Block_7c_Q10 src)
        {
            return new Tbl_Block_7c_Q10
            {
                id = src.id,
                hhd_id = src.hhd_id,
                serial_number = src.serial_number,
                parent_id = src.parent_id,
                item_10_1 = src.item_10_1,
                item_10_2 = src.item_10_2,
                item_10_3 = src.item_10_3,
                item_10_4 = src.item_10_4,
                item_10_5 = src.item_10_5,
                item_10_6 = src.item_10_6,
                item_10_7 = src.item_10_7,
                item_10_8 = src.item_10_8,
                item_10_9 = src.item_10_9,
                item_10_10 = src.item_10_10,
                item_10_11 = src.item_10_11,
                isUpdated = src.isUpdated,
            };
        }
    }
}
