using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_8_6_Validator : AbstractValidator<Tbl_Block_8_Q6>
    {
        private readonly int _totalMembers;

        public Block_8_6_Validator(int totalMembers)
        {
            _totalMembers = totalMembers;

            // 1. item_2 cannot be null or empty
            RuleFor(x => x.item_2)
                .NotNull()
                .WithMessage("Please select a value");

            // 2. item_3 should not be null or zero and <= totalMembers
            RuleFor(x => x.item_3)
                .NotNull()
                .WithMessage("H060: Invalid entry, please check recorded in col 12, block 3")
                .GreaterThan(0)
                .WithMessage("H060: Invalid entry, please check recorded in col 12, block 3")
                .LessThanOrEqualTo(_ => _totalMembers)
                .WithMessage("H060: Invalid entry, please check recorded in col 12, block 3");

            // 3. item_4 should be >= 0 and <= 12
            RuleFor(x => x.item_4)
                .NotNull()
                .WithMessage("H061: Invalid Entry, please check the entry")
                .Must(v => v >= 0d && v <= 12d)
                .WithMessage("H061: Invalid Entry, please check the entry");

            // 4. item_5 should be >= 0
            RuleFor(x => x.item_5)
                .NotNull()
                .WithMessage("H062: Invalid Entry, please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H062: Invalid Entry, please check the entry");
        }
    }
}
