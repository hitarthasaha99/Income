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
            // Allowed crop codes
            var allowedCodes = Block_7_1_Constants.CropCodes.Select(x => x.id).ToList();

            RuleFor(x => x.code)
                .NotNull().WithMessage("H038: Invalid entry, please check the entry")
                .Must(c => allowedCodes.Contains(c.GetValueOrDefault()))
                .WithMessage("H038: Invalid entry, please check the entry");

            // Basic numeric validations (> 0 and not null)
            RuleFor(x => x.unit)
                .NotNull().WithMessage("H039: Invalid entry, please check the entry")
                .GreaterThan(0).WithMessage("H039: Invalid entry, please check the entry");

            RuleFor(x => x.item_4)
                .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                .GreaterThan(0).WithMessage("H040: Invalid entry, please check the entry");

            RuleFor(x => x.item_5)
                .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                .GreaterThan(0).WithMessage("H040: Invalid entry, please check the entry");

            RuleFor(x => x.item_6)
                .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                .GreaterThan(0).WithMessage("H040: Invalid entry, please check the entry");

            RuleFor(x => x.item_7)
                .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                .GreaterThan(0).WithMessage("H040: Invalid entry, please check the entry");

            RuleFor(x => x.item_8)
                .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                .GreaterThan(0).WithMessage("H040: Invalid entry, please check the entry");

            RuleFor(x => x.item_9)
                .NotNull().WithMessage("H040: Invalid entry, please check the entry")
                .GreaterThan(0).WithMessage("H040: Invalid entry, please check the entry");

            // ---------------------------------------------------------
            // Cross-field validation rules
            // ---------------------------------------------------------

            // (ii) item_5 > 0 IF item_6 > 0 OR item_7 > 0
            RuleFor(x => x.item_5)
                .Must((model, item_5) =>
                {
                    if (model.item_6 > 0 || model.item_7 > 0)
                        return item_5 > 0;
                    return true;
                })
                .WithMessage("H040(ii): Invalid entry, please check the entry");

            // (iii) item_6 OR item_7 > 0 IF item_5 > 0
            RuleFor(x => x.item_6)
                .Must((model, item_6) =>
                {
                    if (model.item_5 > 0)
                        return (item_6 > 0 || model.item_7 > 0);
                    return true;
                })
                .WithMessage("H040(iii): Invalid entry, please check the entry");

            RuleFor(x => x.item_7)
                .Must((model, item_7) =>
                {
                    if (model.item_5 > 0)
                        return (item_7 > 0 || model.item_6 > 0);
                    return true;
                })
                .WithMessage("H040(iii): Invalid entry, please check the entry");

            // (iv) item_4 > 0 IF item_5 > 0
            RuleFor(x => x.item_4)
                .Must((model, item_4) =>
                {
                    if (model.item_5 > 0)
                        return item_4 > 0;
                    return true;
                })
                .WithMessage("H040(iv): Invalid entry, please check the entry");
        }
    }

}
