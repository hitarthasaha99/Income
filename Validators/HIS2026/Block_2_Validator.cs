using FluentValidation;
using Income.Database.Models.HIS_2026;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Validators.HIS2026
{
    public class Block_2_Validator : AbstractValidator<Tbl_Block_FieldOperation>
    {
        public Block_2_Validator()
        {
            RuleFor(x => x.total_time)
            .NotNull().WithMessage("H078: Invalid entry, please check the entry.")
            .NotEqual(0).WithMessage("H078: Invalid entry, please check the entry.");

            RuleFor(x => x.number_of_enumerators)
                .NotNull().WithMessage("H079: Invalid entry, please check the entry.")
                .InclusiveBetween(1, 4).WithMessage("H079: Invalid entry, please check the entry.");

            RuleFor(x => x.informant_serial)
                .NotNull().WithMessage("H082: Invalid entry, please check the entry.")
                .NotEqual(0).WithMessage("H082: Invalid entry, please check the entry.");

            RuleFor(x => x.informant_mobile)
                .NotNull().WithMessage("H083 (ii): Invalid entry, please check the entry.")
                .Must(m => !string.IsNullOrWhiteSpace(m) &&
                           (m.StartsWith("9") || m.StartsWith("8") || m.StartsWith("7") || m.StartsWith("6")))
                .WithMessage("H083 (ii): Invalid entry, please check the entry.");

            RuleFor(x => x.informant_response_code)
                .NotNull().WithMessage("H084: Invalid entry, please check the entry.")
                .Must(code => code != null && ((code >= 1 && code <= 4) || code == 9))
                .WithMessage("H084: Invalid entry, please check the entry.");
        }
    }
}
