namespace Belatrix.JobLogger
{
    public interface IJobLogger
    {
        void LogMessage(string message, LogType type);
    }
}