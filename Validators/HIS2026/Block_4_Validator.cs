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
            RuleFor(x => x.item_2)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please select a value")
                .Must(value => value.HasValue && AllowedItem2Values.Contains(value.Value))
                .WithMessage("H014: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_3)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please select a value")
                .Must(value => value.HasValue && AllowedItem3Values.Contains(value.Value))
                .WithMessage("H015: Invalid Entry, Please check the entry");

            RuleFor(x => x.item_6)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please select a value")
                .Must(value => value.HasValue && new int[] {1,2}.Contains(value.Value))
                .WithMessage("H018: Invalid Entry, Please check the entry");
        }

    }
}
