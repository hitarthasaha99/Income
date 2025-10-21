using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Income.Services
{
    public interface IGlobalExceptionHandler
    {
        void Initialize();
        void Log(Exception ex);
        Task LogAsync(Exception ex);
    }

    public class GlobalExceptionHandler : IGlobalExceptionHandler
    {
        private readonly IJSRuntime _js;

        public GlobalExceptionHandler(IJSRuntime js)
        {
            _js = js;
        }

        public void Initialize()
        {
            // Handle MAUI App-level exceptions
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                    Log(ex);
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Log(e.Exception);
                e.SetObserved();
            };
        }

        public void Log(Exception ex)
        {
            // Example: Log locally
            Console.WriteLine($"[Global Exception] {ex.Message}\n{ex.StackTrace}");

            // Optional: send to remote server asynchronously
            _ = LogAsync(ex);
        }

        public async Task LogAsync(Exception ex)
        {
            try
            {
                // Example: you can also call a JS alert or logging library
                await _js.InvokeVoidAsync("console.error", $"[JS Log] {ex.Message}\n{ex.StackTrace}");
            }
            catch
            {
                // ignore JS logging failures
            }
        }
    }
}
