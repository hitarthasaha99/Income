using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_9a_Validator : AbstractValidator<Tbl_Block_9a>
    {
        public Block_9a_Validator()
        {
            // Rule message
            const string msg = "H063: Invalid entry, please check the entry";

            // 1. item_1_1 to item_1_5 >= 0 and not null
            RuleFor(x => x.item_1_1)
                .NotNull().WithMessage(msg)
                .GreaterThanOrEqualTo(0).WithMessage(msg);

            RuleFor(x => x.item_1_2)
                .NotNull().WithMessage(msg)
                .GreaterThanOrEqualTo(0).WithMessage(msg);

            RuleFor(x => x.item_1_3)
                .NotNull().WithMessage(msg)
                .GreaterThanOrEqualTo(0).WithMessage(msg);

            RuleFor(x => x.item_1_4)
                .NotNull().WithMessage(msg)
                .GreaterThanOrEqualTo(0).WithMessage(msg);

            RuleFor(x => x.item_1_5)
                .NotNull().WithMessage(msg)
                .GreaterThanOrEqualTo(0).WithMessage(msg);

            // 2. item_2 to item_4 >= 0 and not null
            RuleFor(x => x.item_2)
                .NotNull().WithMessage(msg)
                .GreaterThanOrEqualTo(0).WithMessage(msg);

            RuleFor(x => x.item_3)
                .NotNull().WithMessage(msg)
                .GreaterThanOrEqualTo(0).WithMessage(msg);

            RuleFor(x => x.item_4)
                .NotNull().WithMessage(msg)
                .GreaterThanOrEqualTo(0).WithMessage(msg);
        }
    }
}
