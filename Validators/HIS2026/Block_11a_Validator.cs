using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_11a_Validator : AbstractValidator<Tbl_Block_11a>
    {
        public Block_11a_Validator()
        {
            RuleFor(x => x.item_1).NotNull().WithMessage("H070: Invalid Entry, Please check the entry").InclusiveBetween(1, 2).WithMessage("H070: Invalid Entry, Please check the entry");
            RuleFor(x => x.item_2).NotNull().WithMessage("H071: Please check the entry").GreaterThan(0).WithMessage("H071: Please check the entry").When(x => x.item_1 == 1);
            RuleFor(x => x.item_3).NotNull().WithMessage("H072: Invalid Entry, Please check the entry").InclusiveBetween(1, 2).WithMessage("H072: Invalid Entry, Please check the entry").When(x=>x.item_1==2);
            RuleFor(x => x.item_4).NotNull().WithMessage("H073: Please check the entry").GreaterThan(0).WithMessage("H073:Please check the entry").When(x => x.item_3 == 1);
            RuleFor(x => x.item_5).NotNull().WithMessage("H074: Please check the entry").GreaterThan(0).WithMessage("H074: Please check the entry").When(x => x.item_3 == 1);
            RuleFor(x => x.item_6).NotNull().WithMessage("H075: Invalid Entry, Please check the entry").GreaterThanOrEqualTo(0).WithMessage("H075: Invalid Entry, Please check the entry");


        }

    }
}
