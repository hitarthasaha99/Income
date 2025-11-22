using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_5_Validator : AbstractValidator<Tbl_Block_5>
    {
        public Block_5_Validator()
        {
            // -------------------------------------------------------
            // Rule 1: item_1 → must be between 1 and 12
            // -------------------------------------------------------
            RuleFor(x => x.item_1)
                .NotNull().WithMessage("H031: Invalid Entry, Please check the entry")
                .InclusiveBetween(1, 12)
                .WithMessage("H031: Invalid Entry, Please check the entry");

            // -------------------------------------------------------
            // Rule 2: item_2 to item_9 → must be >= 0
            // Rule 3: No nulls
            // -------------------------------------------------------
            RuleFor(x => x.item_2)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_3_i)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_3_ii)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_4_i)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_4_ii)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_5)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_6)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_7)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_8)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_9)
                .NotNull().WithMessage("H032: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H032: Invalid Entry, Please check the entry");
        }
    }

}
