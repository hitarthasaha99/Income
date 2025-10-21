using FluentValidation;
using Income.Common;
using Income.Database.Models.Common;
using Income.Database.Models.SCH0_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.SCH0_0
{
    //public static class Block_7_Validator
    //{
    //    private static List<ValidationResult> results = [];
    //    public static List<ValidationResult> Validate(Tbl_Sch_0_0_Block_7 obj, int sector)
    //    {
    //        //1. Item is_household cannot be null
    //        if (obj.is_household == null)
    //        {
    //            results.Add(new ValidationResult
    //            {
    //                PropertyName = "is_household",
    //                Severity = ValidationSeverity.Hard,
    //                IsValid = false,
    //                Errors = new List<string> { "Please select a value" }
    //            });
    //        }
    //        //2. If is_household is 2 (no), other fields must be null
    //        return results;
    //    }
    //}
    public class Block7Validator : AbstractValidator<Tbl_Sch_0_0_Block_7>
    {
        public Block7Validator()
        {
            RuleFor(x => x.is_household).NotEmpty().WithMessage("Please select a value");
            RuleFor(x => x.Block_5A_2).NotEmpty().WithMessage("House number is required");

            When(x => x.is_household == 2, () =>
            {
                RuleFor(x => x.Block_5A_4).NotEmpty().WithMessage("Household Head name is required");
                RuleFor(x => x.Block_5A_5).NotEmpty().WithMessage("Household size is required");
                RuleFor(x => x.Block_5A_6).NotEmpty().WithMessage("Education level is required").When(x => SessionStorage.FSU_Sector == 2);
                RuleFor(x => x.Block_5A_7).NotEmpty().WithMessage("Household type is required");
                RuleFor(x => x.Block_5A_8).NotEmpty().WithMessage("Total amount of land owned is required");
                RuleFor(x => x.Block_5A_9).NotEmpty().WithMessage("Please enter a value");
            });
        }
    }
}
