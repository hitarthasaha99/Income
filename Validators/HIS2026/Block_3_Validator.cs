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
            RuleFor(x => x).NotNull().WithMessage("H004(ii): Invalid Entry, Please check the entry").Must(x => x.serial_no != 1 || x.item_3 == 1).WithMessage("H004(ii): Invalid Entry, Please check the entry");
            RuleFor(x => x.gender).NotNull().WithMessage("H005(i):Invalid Entry, Please check the entry").InclusiveBetween(1, 3).WithMessage("H005(i):Invalid Entry, Please check the entry");
            RuleFor(x => x.item_6).NotNull().WithMessage("H007(i):Invalid Entry, Please check the entry").InclusiveBetween(1, 4).WithMessage("H007(i):Invalid Entry, Please check the entry");
            RuleFor(x => x).Must(x => !(x.item_3 == 2 && x.item_6 != 2)).WithMessage("H007(ii): Invalid Entry, Please check the entries recorded against cols. 3 & 6");
            RuleFor(x => x).Must(x => !((x.item_3 == 3 || x.item_3 == 4) && x.item_6 == 1)).WithMessage("H007(iii): Invalid Entry, Please check the entries recorded against cols. 3 & 6");
            RuleFor(x => x).Must(x => !(x.item_3 == 5 && x.item_6 != 1)).WithMessage("H007(iv): Invalid Entry, Please check the entries recorded against cols. 3 & 6");
            RuleFor(x => x.item_7).NotNull().WithMessage("H008(i): Invalid Entry, Please check the entry").Must(v => new[] { 1, 2, 3, 4, 5, 6, 7, 8, 10, 11 }.Contains(v.Value)).WithMessage("H008(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_8).NotNull().WithMessage("H009(i): Invalid Entry, Please check the entry").Must(v => v >= 1 && v <= 15).WithMessage("H009(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_9).NotNull().When(x => x.item_8.HasValue && x.item_8 >= 1 && x.item_8 <= 13).WithMessage("H009(i): Invalid Entry, Please check the entry").Must((model, item9) =>
            {
                // Rule applies only when item_8 is 1–13
                if (model.item_8.HasValue && model.item_8 >= 1 && model.item_8 <= 13)
                {
                    return item9.HasValue && item9 >= 1 && item9 <= 15;
                }

                return true;
            }).WithMessage("H009(i): Invalid Entry, Please check the entry");
            RuleFor(x => x).Must(m => !(m.item_9.HasValue && m.item_8.HasValue && m.item_9 == m.item_8) && !(m.item_11.HasValue && m.item_10.HasValue && m.item_11 == m.item_10)).WithMessage("H009(iii): Please recheck entries recorded against cols. 8 & 9.");
            RuleFor(x => x.item_10).NotNull().WithMessage("H010(i): Invalid Entry, Please check the entry").InclusiveBetween(1, 15).WithMessage("H010(i): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_11).NotNull().When(x => x.item_10.HasValue && x.item_10 >= 1 && x.item_10 <= 13).WithMessage("H011(i): Invalid Entry, Please check the entry").Must((x, v) => !(x.item_10 >= 1 && x.item_10 <= 13) || (v.HasValue && v >= 1 && v <= 15)).WithMessage("H011(i): Invalid Entry, Please check the entry");
            RuleFor(x => x).Must(x => !(x.item_10.HasValue && x.item_11.HasValue && x.item_10 == x.item_11)).WithMessage("H011(iii): Please recheck entries recorded against cols. 10 & 11.");
            RuleFor(x => x).Must(x => !(x.age.HasValue && x.age < 15 && x.item_12.HasValue)).WithMessage("H0012(ii): Invalid Entry, Please check the entry");
            RuleFor(x => x.item_12).Must((x, v) => !(x.age.HasValue && x.age >= 15) || (v.HasValue && (v == 1 || v == 2))).WithMessage("H0012(ii): Invalid Entry, Please check the entry");

        }
    }
}
