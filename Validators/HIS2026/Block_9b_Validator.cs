using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_9b_Validator : AbstractValidator<Tbl_Block_9b>
    {
        private readonly List<Tbl_Block_3> _block3List;

        public Block_9b_Validator(List<Tbl_Block_3> block3List)
        {
            _block3List = block3List;

            const string msg64i = "H064(i): Invalid entry, please check the entry";
            const string msg64ii = "H064(ii): Invalid entry, please check the entry";
            const string msg65 = "H065: Please check the entry recorded against cols. 8-11";
            const string msg66 = "H066: Invalid entry, please check the entry";

            // ---------------------------------------------------------
            // 1. item_5_1_3 to 1_5, 5_2_3 to 2_5, 5_3_3 to 3_5: >=0 AND not null
            // ---------------------------------------------------------
            RuleFor(x => x.item_5_1_3).NotNull().WithMessage(msg64i).GreaterThanOrEqualTo(0).WithMessage(msg64i);
            RuleFor(x => x.item_5_1_4).NotNull().WithMessage(msg64i).GreaterThanOrEqualTo(0).WithMessage(msg64i);
            RuleFor(x => x.item_5_1_5).NotNull().WithMessage(msg64i).GreaterThanOrEqualTo(0).WithMessage(msg64i);

            RuleFor(x => x.item_5_2_3).NotNull().WithMessage(msg64i).GreaterThanOrEqualTo(0).WithMessage(msg64i);
            RuleFor(x => x.item_5_2_4).NotNull().WithMessage(msg64i).GreaterThanOrEqualTo(0).WithMessage(msg64i);
            RuleFor(x => x.item_5_2_5).NotNull().WithMessage(msg64i).GreaterThanOrEqualTo(0).WithMessage(msg64i);

            RuleFor(x => x.item_5_3_3).NotNull().WithMessage(msg64i).GreaterThanOrEqualTo(0).WithMessage(msg64i);
            RuleFor(x => x.item_5_3_4).NotNull().WithMessage(msg64i).GreaterThanOrEqualTo(0).WithMessage(msg64i);
            RuleFor(x => x.item_5_3_5).NotNull().WithMessage(msg64i).GreaterThanOrEqualTo(0).WithMessage(msg64i);

            // ---------------------------------------------------------
            // 2. item_5_1_3, 5_2_3, 5_3_3 must be <= 12
            // ---------------------------------------------------------
            RuleFor(x => x.item_5_1_3).LessThanOrEqualTo(12).WithMessage(msg64ii);
            RuleFor(x => x.item_5_2_3).LessThanOrEqualTo(12).WithMessage(msg64ii);
            RuleFor(x => x.item_5_3_3).LessThanOrEqualTo(12).WithMessage(msg64ii);

            // ---------------------------------------------------------
            // 3. If ANY Block3.item_8/9/10/11 == 10,
            //    Then: (1_3 & 1_4) OR (2_3 & 2_4) OR (3_3 & 3_4) MUST be > 0
            // ---------------------------------------------------------
            RuleFor(x => x).Custom((model, context) =>
            {
                bool condition = _block3List.Any(b =>
                    b.item_8 == 10 ||
                    b.item_9 == 10 ||
                    b.item_10 == 10 ||
                    b.item_11 == 10);

                if (condition)
                {
                    bool valid =
                        (model.item_5_1_3 > 0 && model.item_5_1_4 > 0) ||
                        (model.item_5_2_3 > 0 && model.item_5_2_4 > 0) ||
                        (model.item_5_3_3 > 0 && model.item_5_3_4 > 0);

                    if (!valid)
                    {
                        context.AddFailure("H065: Please check the entry recorded against cols. 8-11");
                    }
                }
            });

            // ---------------------------------------------------------
            // 4. item_6 and item_7: not null AND >= 0
            // ---------------------------------------------------------
            RuleFor(x => x.item_6)
                .NotNull().WithMessage(msg66)
                .GreaterThanOrEqualTo(0).WithMessage(msg66);

            RuleFor(x => x.item_7)
                .NotNull().WithMessage(msg66)
                .GreaterThanOrEqualTo(0).WithMessage(msg66);
        }
    }
}
