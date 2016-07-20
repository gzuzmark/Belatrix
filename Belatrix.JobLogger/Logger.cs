using System;
using System.Collections.Generic;
using System.Linq;

namespace Belatrix.JobLogger
{

    public enum LogType
    {
        Warning,
        Error,
        Info
    }

    public enum LogSource
    {
        Database,
        Console,
        File
    }

    public class Logger
    {
        private static List<LogType> _logTypes = new List<LogType> { LogType.Info, LogType.Warning, LogType.Error };
        private static List<LogSource> _logSources = new List<LogSource> { LogSource.Database, LogSource.Console, LogSource.File };
        private static IJobLogger _textFileJobLogger = new FileJobLogger();
        private static IJobLogger _consoleJobLogger = new ConsoleJobLogger();
        private static IJobLogger _databaseJobLogger = new DatabaseJobLogger();

        public static void SetLogTypes(params LogType[] logTypes)
        {
            _logTypes.Clear();

            foreach (var logLevel in logTypes)
            {
                _logTypes.Add(logLevel);    
            }
        }

        //public static void SetULogTypesAllowed(params LogSource[] logTypes)
        public static void SetLogSources(params LogSource[] logTypes)
        {
            _logSources.Clear();

            foreach (var logType in logTypes)
            {
                _logSources.Add(logType);
            }
        }

        public static void SetUpJobLoggers(IJobLogger textFileJobLogger
                                        , IJobLogger consoleJobLogger
                                        , IJobLogger databaseJobLogger)
        {
            if (textFileJobLogger != null)
            {
                _textFileJobLogger = textFileJobLogger;
            }

            if (consoleJobLogger != null)
            {
                _consoleJobLogger = consoleJobLogger;
            }

            if (databaseJobLogger != null)
            {
                _databaseJobLogger = databaseJobLogger;
            }
        }

        public static bool SetLog(string message, LogType type)
        {
            bool flagLog = false;
            if (_logSources.Contains(LogSource.Console))
            {
                _consoleJobLogger.LogMessage(message, type);
                flagLog = true;
                
            }
            if (_logSources.Contains(LogSource.Database))
            {

                _databaseJobLogger.LogMessage(message, type);
                 flagLog= true;
            }
            if (_logSources.Contains(LogSource.File))
            {

                _textFileJobLogger.LogMessage(message, type);
                flagLog= true;
            }
            return flagLog;
        }


        public static bool LogMessage(string message, LogType type)
        {
            var messageLog = false;

            if (_logTypes.Count <= 0)
            {
                throw new IndexOutOfRangeException("No types defined");
            }

            if (_logTypes.Count <= 0)
            {
                throw new IndexOutOfRangeException("No Sources defined");
            }

            if (!_logTypes.Contains(type))
            {
                return messageLog;
            }

            messageLog = SetLog(message, type);


            return messageLog;
        }
    }
}