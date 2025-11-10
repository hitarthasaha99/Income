using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Income.Database.Models.HIS_2026;

namespace Income.Validators.HIS2026
{

    public partial class Block_4_Q5_Validator : AbstractValidator<Tbl_Block_4_Q5>
    {
        public Block_4_Q5_Validator()
        {
            // Item 4: Business Seasonal (must be selected)
            RuleFor(x => x.BusinessSeasonal)
                .NotNull()
                .WithMessage("Please select whether the business is seasonal (Yes or No)");

            // Item 5: Number of months (required and within 1–12 range)
            RuleFor(x => x.NumberOfMonths)
                .NotNull()
                .WithMessage("Please enter the number of months of operation")
                .InclusiveBetween(1, 12)
                .WithMessage("Number of months must be between 1 and 12");
        }
    }

}
