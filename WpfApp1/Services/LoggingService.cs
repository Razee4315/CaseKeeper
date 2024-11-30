using System;
using System.IO;

namespace WpfApp1.Services
{
    public static class LoggingService
    {
        private static readonly string LogDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CaseKeeper",
            "Logs"
        );

        private static readonly object LogLock = new();

        static LoggingService()
        {
            Directory.CreateDirectory(LogDirectory);
        }

        public static void LogError(string message, Exception? ex = null)
        {
            Log("ERROR", message, ex);
        }

        public static void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public static void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        private static void Log(string level, string message, Exception? ex = null)
        {
            try
            {
                string logFile = Path.Combine(LogDirectory, $"{DateTime.Now:yyyy-MM-dd}.log");
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                
                if (ex != null)
                {
                    logMessage += $"\nException: {ex.Message}\nStack Trace: {ex.StackTrace}";
                }

                lock (LogLock)
                {
                    File.AppendAllText(logFile, logMessage + Environment.NewLine);
                }
            }
            catch
            {
                // Silently fail if logging fails
            }
        }
    }
}
