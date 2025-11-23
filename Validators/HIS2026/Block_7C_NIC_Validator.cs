using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_7C_NIC_Validator : AbstractValidator<Tbl_Block_7c_NIC>
    {
        public Block_7C_NIC_Validator()
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

            // Item 5: Number of months (required and within 1–12 range)
            RuleFor(x => x.GrossValue)
                .NotNull()
                .WithMessage("H049(ii): Invalid Entry, Please check the entry")
                .GreaterThanOrEqualTo(0)
                .WithMessage("H049(ii): Invalid Entry, Please check the entry");
        }
    }
}
