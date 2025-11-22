using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_7A_Validator : AbstractValidator<Tbl_Block_7a>
    {
        public Block_7A_Validator()
        {
            // Items 2_1 to 2_11 → H042 errors
            RuleFor(x => x.item_2_1).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_2).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_3).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_4).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_5).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_6).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_7).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_8).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_9).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_10).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_11).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");

            // Items 2_12 & 2_13 – also assume H042 unless told otherwise
            RuleFor(x => x.item_2_12).NotNull().WithMessage("H042: Invalid entry, please check the entry").WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");
            RuleFor(x => x.item_2_13).NotNull().WithMessage("H042: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H042: Invalid entry, please check the entry");

            // Items 3, 4, 5 → H043 errors
            RuleFor(x => x.item_3).NotNull().WithMessage("H043: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H043: Invalid entry, please check the entry");
            RuleFor(x => x.item_4).NotNull().WithMessage("H043: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H043: Invalid entry, please check the entry");
            RuleFor(x => x.item_5).NotNull().WithMessage("H043: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H043: Invalid entry, please check the entry");

            // item_6 → Warning if value < 0
            RuleFor(x => x.item_6)
                .NotNull()
                .Must(v => v >= 0)
                .WithMessage("W043: Value cannot be negative");
        }
    }
}
