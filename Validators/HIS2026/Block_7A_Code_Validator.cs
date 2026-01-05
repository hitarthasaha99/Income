using FluentValidation;
using Income.Common.HIS2026;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_7A_Code_Validator : AbstractValidator<Tbl_Block_7a_1>
    {
        public Block_7A_Code_Validator()
        {
            // ---------------------------------------------------------
            // Lookup validations (always applicable)
            // ---------------------------------------------------------

            var allowedCodes = Block_7_1_Constants.CropCodes
                .Select(x => x.id)
                .ToHashSet();

            RuleFor(x => x.code)
                .NotNull().WithMessage("H038: Invalid entry, please check the entry")
                .Must(c => allowedCodes.Contains(c.GetValueOrDefault()))
                .WithMessage("H038: Invalid entry, please check the entry");

            // ---------------------------------------------------------
            // whetherCropSold (always applicable)
            // ---------------------------------------------------------

            RuleFor(x => x.whetherCropSold)
                .NotNull().WithMessage("Invalid entry, please check the entry")
                .InclusiveBetween(1, 2)
                .WithMessage("Invalid entry, please check the entry");

            // ---------------------------------------------------------
            // Conditional validations (ONLY when whetherCropSold = 2)
            // ---------------------------------------------------------

            When(x => x.whetherCropSold == 2, () =>
            {
                RuleFor(x => x.unit)
                    .NotNull().WithMessage("H039: Invalid entry, please check the entry")
                    .InclusiveBetween(1, 2)
                    .WithMessage("H039: Invalid entry, please check the entry");

                RuleFor(x => x.item_4)
                    .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("H040: Invalid entry, please check the entry");

                RuleFor(x => x.item_5)
                    .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("H040: Invalid entry, please check the entry");

                RuleFor(x => x.item_6)
                    .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("H040: Invalid entry, please check the entry");

                RuleFor(x => x.item_7)
                    .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("H040: Invalid entry, please check the entry");

                RuleFor(x => x.item_8)
                    .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("H040: Invalid entry, please check the entry");

                RuleFor(x => x.item_9)
                    .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("H040: Invalid entry, please check the entry");

                // ---------------------------------------------------------
                // Cross-field validations (H040 sub-rules)
                // ---------------------------------------------------------

                RuleFor(x => x)
                    .Custom((model, context) =>
                    {
                        var item4 = model.item_4.GetValueOrDefault();
                        var item5 = model.item_5.GetValueOrDefault();
                        var item6 = model.item_6.GetValueOrDefault();
                        var item7 = model.item_7.GetValueOrDefault();

                        // (ii) item_5 > 0 IF item_6 > 0 OR item_7 > 0
                        if ((item6 > 0 || item7 > 0) && item5 <= 0)
                        {
                            context.AddFailure(
                                "item_5",
                                "H040(ii): Invalid entry, please check the entry");
                        }

                        // (iii) item_6 OR item_7 > 0 IF item_5 > 0
                        if (item5 > 0 && item6 <= 0 && item7 <= 0)
                        {
                            context.AddFailure(
                                "item_6",
                                "H040(iii): Invalid entry, please check the entry");
                        }

                        // (iv) item_4 > 0 IF item_5 > 0
                        if (item5 > 0 && item4 <= 0)
                        {
                            context.AddFailure(
                                "item_4",
                                "H040(iv): Invalid entry, please check the entry");
                        }
                    });
            });
        }
    }



}
