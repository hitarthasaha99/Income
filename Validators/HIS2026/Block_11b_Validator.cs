using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_11b_Validator : AbstractValidator<Tbl_Block_11b>
    {
        public Block_11b_Validator() {
            RuleFor(x => x.item_3).NotNull().WithMessage("H076: Invalid Entry, Please check the entry").InclusiveBetween(1, 2).WithMessage("H076: Invalid Entry, Please check the entry");
            RuleFor(x => x.item_4).NotNull().WithMessage("H076: Please check the entry").GreaterThan(0).WithMessage("H076: Please check the entry").When(x => x.item_3 == 1);

        }
    }
}
