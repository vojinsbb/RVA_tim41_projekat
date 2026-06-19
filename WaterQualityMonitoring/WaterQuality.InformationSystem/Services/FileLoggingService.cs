using System;
using System.IO;

namespace WaterQuality.InformationSystem.Services
{
    public class FileLoggingService : ILoggingService
    {
        private readonly string _logFilePath;

        public FileLoggingService()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string logsDirectory = Path.Combine(baseDirectory, "Logs");

            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            _logFilePath = Path.Combine(logsDirectory, "activity-log.txt");
        }

        public void Log(string message)
        {
            string logEntry = string.Format(
                "{0:yyyy-MM-dd HH:mm:ss} | {1}",
                DateTime.Now,
                message);

            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
    }
}