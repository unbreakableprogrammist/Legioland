namespace Gra.Logging;

public class Logger {
    private static Logger _instance;
    private Logger() { }
    public static Logger Instance {
        get { 
            if (_instance == null) _instance = new Logger();
            return _instance;
        }
    }
    private ILoggerStrategy _strategy;
    public void SetStrategy(ILoggerStrategy strategy)
    {
        _strategy = strategy;
    }

    public void Log(string message)
    {
        string formatted = $"[{DateTime.Now:HH:mm:ss}] {message}";
        _strategy?.Log(formatted);
    }

    public List<string> GetLogs() => _strategy?.GetLogs() ?? new List<string>();

    public void SaveToFile(string pName, string path)
    {
        _strategy?.SaveToFile(pName, path);
    }
}
