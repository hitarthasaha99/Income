using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_7C_Validator : AbstractValidator<Tbl_Block_7c>
    {
        public Block_7C_Validator()
        {
            RuleFor(x => x.item_7_11).NotNull().WithMessage("H051: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H051: Invalid entry, please check the entry");
        }
    }
}
