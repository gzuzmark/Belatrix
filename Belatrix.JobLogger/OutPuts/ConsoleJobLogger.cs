using System;

namespace Belatrix.JobLogger
{
    public class ConsoleJobLogger
        : IJobLogger
    {

        public void LogMessage(string message, LogType logType)
        {
            SetTypeColour(logType);
            Console.WriteLine(DateTime.Now.ToShortDateString() + message);
        }


        private static void SetTypeColour(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

            }
        }

    }
}