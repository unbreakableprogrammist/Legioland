using System.IO;

namespace Gra.Logging;

public class FileLoggerStrategy : ILoggerStrategy
{
    private List<string> _logs = new List<string>();
    private readonly object _logLock = new object();

    public void Log(string message)
    {
        lock (_logLock)
        {
            _logs.Add(message);
        }
    }

    public List<string> GetLogs()
    {
        lock (_logLock)
        {
            return new List<string>(_logs);
        }
    }

    public void SaveToFile(string playerName, string folderPath)
    {
        string fileName = $"{playerName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
        string fullPath = Path.Combine(folderPath, fileName);
        try
        {
            if (!string.IsNullOrEmpty(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            List<string> logsSnapshot;
            lock (_logLock)
            {
                logsSnapshot = new List<string>(_logs);
            }
            File.WriteAllLines(fullPath, logsSnapshot);
        }
        catch (Exception ex)
        {
        }
    }
}
