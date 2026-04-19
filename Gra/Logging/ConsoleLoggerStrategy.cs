namespace Gra.Logging;

public class ConsoleLoggerStrategy : ILoggerStrategy
{
    private List<string> _logs = new List<string>();

    public void Log(string message)
    {
        _logs.Add(message);
    }
    public List<string> GetLogs() => _logs;
    public void SaveToFile(string playerName, string folderPath){}
}