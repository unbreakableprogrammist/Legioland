namespace Gra.Logging;

public class Logger { // klasa jest dostepna z calego systemu 
    private static Logger _instance; // trzymamy nasza instancje ktora jest statyczna 
    private Logger() { } // prywatny konsturkor 
    public static Logger Instance { // metoda ktora zwraca instancje loggera 
        get { 
            if (_instance == null) _instance = new Logger(); // jesli jeszcze null stworz loger 
            return _instance;  // w przecinym razie daj moja instancje
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