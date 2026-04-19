using System.IO;

namespace Gra.Logging;

public class FileLoggerStrategy : ILoggerStrategy
{
    private List<string> _logs = new List<string>();

    public void Log(string message)
    {
        _logs.Add(message);
    }

    public List<string> GetLogs() => _logs;

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
            File.WriteAllLines(fullPath, _logs);
        }
        catch (Exception ex)
        {
        }
    }
}