using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_8_Validator : AbstractValidator<Tbl_Block_8>
    {
        private const string ErrorMsg = "H056: Invalid entry, please check the entry";

        public Block_8_Validator()
        {
            RuleFor(x => x.item_1)
                .NotNull().WithMessage(ErrorMsg)
                .GreaterThanOrEqualTo(0).WithMessage(ErrorMsg);

            RuleFor(x => x.item_2)
                .NotNull().WithMessage(ErrorMsg)
                .GreaterThanOrEqualTo(0).WithMessage(ErrorMsg);

            RuleFor(x => x.item_3)
                .NotNull().WithMessage(ErrorMsg)
                .GreaterThanOrEqualTo(0).WithMessage(ErrorMsg);

            RuleFor(x => x.item_4)
                .NotNull().WithMessage(ErrorMsg)
                .GreaterThanOrEqualTo(0).WithMessage(ErrorMsg);

            RuleFor(x => x.item_5)
                .NotNull().WithMessage(ErrorMsg)
                .GreaterThanOrEqualTo(0).WithMessage(ErrorMsg);
        }
    }
}
