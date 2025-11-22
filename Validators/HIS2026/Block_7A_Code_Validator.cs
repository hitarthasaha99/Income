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
                .NotNull().WithMessage("Crop code is required")
                .Must(c => allowedCodes.Contains(c.Value))
                .WithMessage("Invalid crop code");

            // Basic numeric validations (> 0 and not null)
            RuleFor(x => x.unit)
                .NotNull().WithMessage("Unit is required")
                .GreaterThan(0).WithMessage("Unit must be greater than 0");

            RuleFor(x => x.item_4)
                .NotNull().WithMessage("Total quantity produced is required")
                .GreaterThan(0).WithMessage("Total quantity produced must be greater than 0");

            RuleFor(x => x.item_5)
                .NotNull().WithMessage("Total quantity sold is required")
                .GreaterThan(0).WithMessage("Total quantity sold must be greater than 0");

            RuleFor(x => x.item_6)
                .NotNull().WithMessage("Total sale value is required")
                .GreaterThan(0).WithMessage("Total sale value must be greater than 0");

            RuleFor(x => x.item_7)
                .NotNull().WithMessage("Average per unit sale value is required")
                .GreaterThan(0).WithMessage("Average per unit sale value must be greater than 0");

            RuleFor(x => x.item_8)
                .NotNull().WithMessage("Value of pre-harvested sale is required")
                .GreaterThan(0).WithMessage("Value of pre-harvested sale must be greater than 0");

            RuleFor(x => x.item_9)
                .NotNull().WithMessage("Value of by-products is required")
                .GreaterThan(0).WithMessage("Value of by-products must be greater than 0");

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
                .WithMessage("Total quantity sold (item_5) must be > 0 when sale value (item_6) or avg sale value (item_7) is > 0.");

            // (iii) item_6 OR item_7 > 0 IF item_5 > 0
            RuleFor(x => x.item_6)
                .Must((model, item_6) =>
                {
                    if (model.item_5 > 0)
                        return (item_6 > 0 || model.item_7 > 0);
                    return true;
                })
                .WithMessage("Either total sale value (item_6) or avg sale value (item_7) must be > 0 when quantity sold (item_5) is > 0.");

            RuleFor(x => x.item_7)
                .Must((model, item_7) =>
                {
                    if (model.item_5 > 0)
                        return (item_7 > 0 || model.item_6 > 0);
                    return true;
                })
                .WithMessage("Either avg sale value (item_7) or total sale value (item_6) must be > 0 when quantity sold (item_5) is > 0.");

            // (iv) item_4 > 0 IF item_5 > 0
            RuleFor(x => x.item_4)
                .Must((model, item_4) =>
                {
                    if (model.item_5 > 0)
                        return item_4 > 0;
                    return true;
                })
                .WithMessage("Total quantity produced (item_4) must be > 0 when quantity sold (item_5) is > 0.");
        }
    }

}
