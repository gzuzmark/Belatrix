using System;
using System.Configuration;
using System.IO;

namespace Belatrix.JobLogger
{
    public class FileJobLogger
        : IJobLogger
    {
        private readonly string _logRoute;

        public FileJobLogger(string logRoute)
        {
            _logRoute = logRoute;
        }

        public FileJobLogger()
        {
            _logRoute = ConfigurationManager.AppSettings["SourceFile"].ToString();
        }

        public void LogMessage(string message, LogType logType)
        {
            var logFilePath = _logRoute + "LogFile" + DateTime.Now.ToShortDateString() + ".txt";
            string logFileContent = string.Empty;

            if (!File.Exists(logFilePath))
            {
                logFileContent = File.ReadAllText(logFilePath);
            }

            logFileContent += message;

            File.WriteAllText(logFilePath, logFileContent);
        }
    }
}