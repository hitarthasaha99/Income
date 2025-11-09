using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Services
{
    public class LanguageState
    {
        public event Action? OnChange;
        public void NotifyLanguageChanged() => OnChange?.Invoke();
    }
}
