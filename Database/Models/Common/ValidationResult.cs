using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public ValidationSeverity Severity { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public enum ValidationSeverity
    {
        Hard,
        Soft,
    }

}
