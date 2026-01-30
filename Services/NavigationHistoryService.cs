using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Services
{ 
    public class NavigationHistoryService
    {
        private readonly Stack<string> _history = new();

        public void Track(string uri)
        {
            if (_history.Count == 0 || _history.Peek() != uri)
                _history.Push(uri);
        }

        public bool CanGoBack() => _history.Count > 1;

        public string GoBack()
        {
            if (_history.Count > 1)
            {
                _history.Pop();          // remove current page
                return _history.Peek();  // previous page
            }
            return "/";
        }

        // NEW: reset stack to a single page
        public void ResetTo(string uri)
        {
            _history.Clear();
            _history.Push(uri);
        }

        public void Clear() => _history.Clear();
    }

}
