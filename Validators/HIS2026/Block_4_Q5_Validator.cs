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
            //Item 3: NIC code
            RuleFor(x => x.NicCode)
                .Must((model, nic) =>
                {
                    // If ActivityName is "Others", NULL NicCode is allowed
                    if (model.ActivityName == "Others")
                        return true;

                    // Otherwise NicCode must be selected
                    return nic != null;
                })
                .WithMessage("Please select a NIC code");
            // Item 4: Business Seasonal (must be selected)
            RuleFor(x => x.BusinessSeasonal)
                .NotNull()
                .WithMessage("H017(ii): Invalid Entry, Please check the entry");

            // Item 5: Number of months (required and within 1–12 range)
            RuleFor(x => x.NumberOfMonths)
                .NotNull()
                .WithMessage("H017(iii): Invalid Entry, Please check the entry")
                .InclusiveBetween(1, 12)
                .WithMessage("H017(iii): Invalid Entry, Please check the entry");
        }
    }

}
