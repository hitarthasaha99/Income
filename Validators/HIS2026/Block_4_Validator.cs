using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public partial class Block_4_Validator : AbstractValidator<Tbl_Block_4>
    {
        private static readonly int[] AllowedItem2Values = { 1, 2, 3, 9, 10 };
        private static readonly int[] AllowedItem3Values = { 1, 2, 3, 4, 5, 6, 7, 9, 10 };

        public Block_4_Validator()
        {
            //RuleFor(x => x.item_2)
            //    .Cascade(CascadeMode.Stop)
            //    .NotEmpty()
            //    .WithMessage("Please select a value")
            //    .Must(value => value.HasValue && AllowedItem2Values.Contains(value.Value))
            //    .WithMessage("H014: Invalid Entry, Please check the entry");

            //RuleFor(x => x.item_3)
            //    .Cascade(CascadeMode.Stop)
            //    .NotEmpty()
            //    .WithMessage("Please select a value")
            //    .Must(value => value.HasValue && AllowedItem3Values.Contains(value.Value))
            //    .WithMessage("H015: Invalid Entry, Please check the entry");

            //RuleFor(x => x.item_6)
            //    .Cascade(CascadeMode.Stop)
            //    .NotEmpty()
            //    .WithMessage("Please select a value")
            //    .Must(value => value.HasValue && new int[] {1,2}.Contains(value.Value))
            //    .WithMessage("H018: Invalid Entry, Please check the entry");


            RuleSet("Page3", () =>
            {
                RuleFor(x => x.item_6).NotEmpty().WithMessage("H018: Invalid Entry, Please check the entry");

                RuleFor(x => x.item_7)
                    .NotNull()
                    .GreaterThan(0)
                    .When(x => x.item_6 == 1)
                    .WithMessage("H019: Invalid Entry, Please check the entry")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.item_7)
                            .Must(HaveMaxThreeDecimals)
                            .WithMessage("Item 7 must have at most 3 decimal places.");
                    });

                RuleFor(x => x.item_8)
                    .NotEmpty()
                    .When(x => x.item_6 == 1)
                    .WithMessage("H020(i): Please check the entry")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.item_8)
                            .Must(IsValidCommaSeparatedList)
                            .WithMessage("Item 8 must contain only values 1,2,3,4,9.");
                    });

                RuleFor(x => x.item_9).NotEmpty().WithMessage("H021: Invalid Entry, Please check the entry");

                RuleFor(x => x.item_10)
                    .NotEmpty()
                    .When(x => x.item_9 == 1 || x.item_9 == 2 || x.item_9 == 3)
                    .WithMessage("H022(i): Please check the entry")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.item_10)
                            .Must(IsValidCommaSeparatedList)
                            .WithMessage("Item 10 must contain only values 1,2,3,4,9.");
                    });

                RuleFor(x => x.item_11).NotEmpty().WithMessage("H023: Invalid Entry, Please check the entry");

                // Condition: item_11 is 1, 2 or 3
                When(x => x.item_11 == 1 || x.item_11 == 2 || x.item_11 == 3, () =>
                {
                    // item_12: required, numeric, > 0
                    RuleFor(x => x.item_12)
                        .NotNull()
                        .WithMessage("H024: Invalid Entry, Please check the entry")
                        .GreaterThan(0)
                        .WithMessage("H024: Invalid Entry, Please check the entry");

                    // item_13: required (string)
                    RuleFor(x => x.item_13)
                        .NotEmpty()
                        .WithMessage("H025: Invalid Entry, Please check the entry");

                    // item_14: required, numeric, > 0
                    RuleFor(x => x.item_14)
                        .NotNull()
                        .WithMessage("H026: Invalid Entry, Please check the entry")
                        .GreaterThan(0)
                        .WithMessage("H026: Invalid Entry, Please check the entry");
                });

                RuleFor(x => x.item_15).NotEmpty().WithMessage("H027: Invalid Entry, Please check the entry");

                When(x => x.item_15 == 1, () =>
                {
                    //RuleFor(x => x.item_16)
                    //.NotNull()
                    //.WithMessage("H028(i): Please check the entry");

                    RuleFor(x => x.item_17)
                    .NotNull()
                    .GreaterThanOrEqualTo(500)
                    .WithMessage("H029: Invalid Entry, Please check the entry");

                });

            });
        }

        private bool HaveMaxThreeDecimals(decimal? value)
        {
            if (value == null) return false;

            return BitConverter.GetBytes(decimal.GetBits(value.Value)[3])[2] <= 3;
        }

        private bool IsValidCommaSeparatedList(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            var allowed = new HashSet<string> { "1", "2", "3", "4", "9" };

            return value
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .All(v => allowed.Contains(v));
        }

    }
}
