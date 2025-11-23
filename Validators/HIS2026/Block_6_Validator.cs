using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_6_Validator : AbstractValidator<Tbl_Block_6>
    {
        public Block_6_Validator()
        {
            // -------------------------------------------------------
            // Rule 1: item_1 → must be between 1 and 12
            // -------------------------------------------------------
            RuleFor(x => x.item_1)
                .NotNull().WithMessage("H034: Invalid Entry, Please check the entry")
                .InclusiveBetween(1, 31)
                .WithMessage("H034: Invalid Entry, Please check the entry");

            // -------------------------------------------------------
            // Rule 2: item_2 to item_3 → must be >= 0
            // Rule 3: No nulls
            // -------------------------------------------------------
            RuleFor(x => x.item_2)
                .NotNull().WithMessage("H035: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H035: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_3)
                .NotNull().WithMessage("H035: Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H035: Invalid Entry, Please check the entry");
        }
    }
}
