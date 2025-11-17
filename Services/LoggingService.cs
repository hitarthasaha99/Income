using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Income.Services
{
    public interface ILoggingService
    {
        Task LogInfo(string message,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0);

        Task LogWarning(string message,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0);

        Task LogError(string message, Exception? ex = null,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0);

        Task LogDebug(string message,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0);
    }

    public class LoggingService : ILoggingService
    {
        private readonly SemaphoreSlim _lock = new(1, 1);
        private readonly string _logFilePath;
        const long maxSize = 5 * 1024 * 1024; // 5 MB


        public LoggingService()
        {
            string basePath = FileSystem.AppDataDirectory;
            string logFolder = Path.Combine(basePath, "Logs");

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            _logFilePath = Path.Combine(logFolder, "app_log.txt");
        }

        public Task LogInfo(string message,
            string caller = "", string file = "", int line = 0)
            => WriteLog("INFO", message, caller, file, line);

        public Task LogWarning(string message,
            string caller = "", string file = "", int line = 0)
            => WriteLog("WARNING", message, caller, file, line);

        public Task LogDebug(string message,
            string caller = "", string file = "", int line = 0)
            => WriteLog("DEBUG", message, caller, file, line);

        public Task LogError(string message, Exception? ex = null,
            string caller = "", string file = "", int line = 0)
        {
            string full = ex == null ? message : $"{message}\n{ex}";
            return WriteLog("ERROR", full, caller, file, line);
        }

        private async Task WriteLog(string level, string message,
            string caller, string file, int line)
        {
            var fileInfo = new FileInfo(_logFilePath);
            if (fileInfo.Exists && fileInfo.Length > maxSize)
            {
                string archivePath = Path.Combine(
                    Path.GetDirectoryName(_logFilePath)!,
                    $"log_{DateTime.Now:yyyyMMddHHmmss}.txt");

                File.Move(_logFilePath, archivePath);
            }
            string fileName = Path.GetFileNameWithoutExtension(file);

            string lineText =
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] " +
                $"{fileName}.{caller}() Ln:{line} → {message}{Environment.NewLine}";

            await _lock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(_logFilePath, lineText);
            }
            finally
            {
                _lock.Release();
            }
        }
    }


}
