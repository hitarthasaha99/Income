using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Income.Database.Models.SCH0_0;
using Income.Viewmodels.SCH0_0;

namespace Income.Validators.SCH0_0
{

    public partial class Block_0_1_Validator : AbstractValidator<Tbl_Sch_0_0_Block_0_1>
    {
        public Block_0_1_Validator()
        {
            // 1. Block_1_16 cannot be null
            RuleFor(x => x.Block_1_16)
                .NotNull()
                .WithMessage("Survey code (Q16) is required");

            // 2. If Block_1_16 = 1 or 4, then Block_1_14 must be greater than zero/not null
            When(x => x.Block_1_16 == 1 || x.Block_1_16 == 4, () =>
            {
                RuleFor(x => x.Block_1_14)
                    .NotNull().WithMessage("Approx. present population must be provided when Q16 is 1 or 4")
                    .GreaterThan(0).WithMessage("Approx. present population (Q14) must be greater than zero when Survey Code is 1 or 4");
            });

            // 3. If Block_1_16 = 2,3,5,6,7, Block_1_14 must be zero
            When(x => new[] { 2, 3, 5, 6, 7 }.Contains(x.Block_1_16 ?? 0), () =>
            {
                RuleFor(x => x.Block_1_14)
                    .Must(v => v == 0)
                    .WithMessage("Approx. present population must be zero when Survey Code is 2, 3, 5, 6, or 7");
            });

            // 4. If Block_1_16 = 4,5,6,7, Block_1_17 must not be null
            When(x => new[] { 4, 5, 6, 7 }.Contains(x.Block_1_16 ?? 0), () =>
            {
                RuleFor(x => x.Block_1_17)
                    .NotNull()
                    .WithMessage("Reason for substitution (Q17) must be provided when Survey Code is 4, 5, 6, or 7");
            });
        }
    }
}
