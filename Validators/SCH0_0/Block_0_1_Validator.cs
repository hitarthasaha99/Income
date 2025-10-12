using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Income.Viewmodels.SCH0_0;

namespace Income.Validators.SCH0_0
{

    public class Block_0_1_Validator : AbstractValidator<Block_0_1_VM>
    {
        public Block_0_1_Validator()
        {
            RuleFor(x => x.block_0_1.Block_1_14)
                .NotEmpty().WithMessage("Block 1.14 cannot be empty.");

            //RuleFor(x => x.block_0_1.Block_1_18)
            //    .NotEmpty().WithMessage("Block 1.18 is required.");

            //RuleFor(x => x.block_0_1.Block_1_19)
            //    .NotEmpty().WithMessage("Block 1.19 is required.");
        }
    }
}
