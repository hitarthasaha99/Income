using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block7DValidator : AbstractValidator<Tbl_Block_7d>
    {
        public Block7DValidator()
        {
            RuleFor(x => x.item_3)
            .NotEmpty().WithMessage("H054: Invalid entry, please check the entry")
            .Must(x => x == 1 || x == 2)
            .WithMessage("H054: Invalid entry, please check the entry");

            When(x => x.item_3 == 2, () =>
            {
                RuleFor(x => x.item_4)
                .NotNull()
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("H055: Invalid entry, please check the entry");
            });
        }
    }
}
