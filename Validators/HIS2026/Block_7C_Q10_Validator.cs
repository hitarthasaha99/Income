using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_7C_Q10_Validator : AbstractValidator<Tbl_Block_7c_Q10>
    {
        public Block_7C_Q10_Validator()
        {
            RuleFor(x => x.item_10_1)
            .NotNull().WithMessage("H050: Invalid entry, please check the entry")
            .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_2)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_3)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_4)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_5)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_6)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_7)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_8)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_9)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_10)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");

            RuleFor(x => x.item_10_11)
                .NotNull().WithMessage("H050: Invalid entry, please check the entry")
                .GreaterThanOrEqualTo(0).WithMessage("H050: Invalid entry, please check the entry");
        }
    }
}
