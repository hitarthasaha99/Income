using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_10_Validator : AbstractValidator<Tbl_Block_10>
    {
        public  Block_10_Validator()
        {
            RuleFor(x => x.item_1).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_2).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_3).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_4).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_5).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_6).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_7).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_8).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_9).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_10).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_11).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_12).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_13).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_14).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_15).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_16).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_17).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_18).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_19).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_20).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_21).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_22).NotNull().WithMessage("H068: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H068: Invalid entry, please check the entry");
            RuleFor(x => x.item_23).NotNull().WithMessage("H069(i): Invalid entry, please check the entry").GreaterThan(0).WithMessage("H069(i): Invalid entry, please check the entry");
        }
    }
}
