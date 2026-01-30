using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Services
{
    // Services/BackButtonService.cs
    public class BackButtonService
    {
        public Func<bool>? HandleBackRequested;

        public bool OnBackPressed()
            => HandleBackRequested?.Invoke() ?? false;
    }

}
