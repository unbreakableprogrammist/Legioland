namespace Gra.Logging;

public interface ILoggerStrategy
{
    void Log(string message);
    List<string> GetLogs(); 
    void SaveToFile(string playerName, string folderPath);
}