using FluentValidation;
using Income.Database.Models.HIS_2026;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_3_Validator : AbstractValidator<Tbl_Block_3>
    {
        public Block_3_Validator()
        {

            RuleFor(x => x.item_2).NotNull().WithMessage("Please enter a name.").Matches(@"^[a-zA-Z ]+$").WithMessage("Name must contain only letters and spaces.");
            RuleFor(x => x.item_3).NotNull().WithMessage("H004(i):Invalid Entry, Please check the entry").InclusiveBetween(1, 9).WithMessage("H004(i):Invalid Entry, Please check the entry");
            //sl.no is one then reletion to head self-1
            RuleFor(x => x.item_3).Must((model, relation) => model.serial_no != 1 || relation == 1).When(x => x.item_3.HasValue).WithMessage("H004(ii): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_3).Must((model, relation) => model.serial_no == 1 || relation != 1).When(x => x.item_3.HasValue).WithMessage("H004(iii): Invalid Entry, Please check the entry");
            RuleFor(x => x.gender).NotNull().WithMessage("H005(i):Invalid Entry, Please check the entry").InclusiveBetween(1, 3).WithMessage("H005(i):Invalid Entry, Please check the entry");
            RuleFor(x => x.item_6).NotNull().WithMessage("H007(i):Invalid Entry, Please check the entry").InclusiveBetween(1, 4).WithMessage("H007(i):Invalid Entry, Please check the entry");
            RuleFor(x => x.item_6).Equal(2).When(x => x.item_3 == 2).WithMessage("H007(ii): Invalid Entry, Please check the entryies recorded against cols. 3 & 6");
            RuleFor(x => x.item_6).Must((model, item6) => item6 != 1).When(x => x.item_3 == 3 || x.item_3 == 4).WithMessage("H007(iii): Invalid Entry, Please check the entryies recorded against cols. 3 & 6");
            RuleFor(x => x.item_6).Must((model, item6) => item6 == 1).When(x => x.item_3 == 5).WithMessage("H007(iv): Invalid Entry, Please check the entryies recorded against cols. 3 & 6");
            RuleFor(x => x).Must(x => !(x.item_3 == 5 && x.item_6 != 1)).WithMessage("H007(iv): Invalid Entry, Please check the entries recorded against cols. 3 & 6");
            RuleFor(x => x.item_7).NotNull().WithMessage("H008(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_7).Must(v => new[] { 1, 2, 3, 4, 5, 6, 7, 8, 10, 11 }.Contains(v.Value)).When(x => x.item_7 != null).WithMessage("H008(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_8).NotNull().WithMessage("H008(i): Invalid Entry, Please check the entry").InclusiveBetween(1, 15).WithMessage("H008(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_9).NotNull().WithMessage("H009(i): Invalid Entry, Please check the entry").InclusiveBetween(1, 15).When(x => x.item_8 >= 1 && x.item_8 <= 13).WithMessage("H009(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_9).Must(v => v == null).When(x => x.item_8 >= 14 && x.item_8 <= 15).WithMessage("H009(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_9).Must((model, item9) => item9 != model.item_8).When(x => x.item_9 != null && x.item_8 != null).WithMessage("H009(iii): Please recheck entries recorded against cols. 8 & 9.");
            RuleFor(x => x.item_10).NotNull().WithMessage("H0010(i): Invalid Entry, Please check the entry").InclusiveBetween(1, 15).WithMessage("H0010(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_11).NotNull().WithMessage("H011(i): Invalid Entry, Please check the entry").InclusiveBetween(1, 15).When(x => x.item_10 >= 1 && x.item_10 <= 13).WithMessage("H011(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_11).Must(v => v == null).When(x => x.item_10 >= 14 && x.item_10 <= 15).WithMessage("H011(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_11).Must((model, item11) => item11 != model.item_10).When(x => x.item_11 != null && x.item_10 != null).WithMessage("H011(iii): Please recheck entries recorded against cols. 10 & 11.");
            RuleFor(x => x.item_12).NotNull().WithMessage("H0012(ii): Invalid Entry, Please check the entry").InclusiveBetween(1, 2).WithMessage("H0012(ii): Invalid Entry, Please check the entry").When(x => x.age >= 15);

        }
    }
}
