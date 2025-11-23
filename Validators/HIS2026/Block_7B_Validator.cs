using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_7B_Validator : AbstractValidator<Tbl_Block_7b>
    {
        public Block_7B_Validator()
        {
            // Items 6_1 to 6_12 → H046 errors
            RuleFor(x => x.item_6_1).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_2).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_3).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_4).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_5).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_6).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_7).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_8).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_9).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_10).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_11).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_6_12).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");

            RuleFor(x => x.item_7_1).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_2).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_3).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_4).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_5).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_6).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_7).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_8).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_9).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_10).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_11).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_12).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_7_13).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");

            RuleFor(x => x.item_8).NotNull().WithMessage("H046: Invalid entry, please check the entry").GreaterThanOrEqualTo(0).WithMessage("H046: Invalid entry, please check the entry");
            RuleFor(x => x.item_9)
            .Must((model, item9) =>
            {
                var expected =
                    model.item_6_12.GetValueOrDefault()
                    + model.item_8.GetValueOrDefault()
                    - model.item_7_13.GetValueOrDefault();

                return item9.GetValueOrDefault() == expected;
            })
            .WithMessage(model =>
            {
                var expected =
                    model.item_6_12.GetValueOrDefault()
                    + model.item_8.GetValueOrDefault()
                    - model.item_7_13.GetValueOrDefault();

                return $"H047(i): Invalid entry, please check the entry";
            });

        }
    }
}
