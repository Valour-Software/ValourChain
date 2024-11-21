namespace ValourChain;

public class Logger<T>
{
    private readonly string _prefix = typeof(T).Name;
    
    private void LogInternal(string message, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{_prefix}] {message}");
        Console.ResetColor();
    }
    
    public void LogError(string message)
    {
        LogInternal(message, ConsoleColor.Red);
    }
    
    public void LogWarning(string message)
    {
        LogInternal(message, ConsoleColor.Yellow);
    }
    
    public void LogSuccess(string message)
    {
        LogInternal(message, ConsoleColor.Green);
    }
    
    public void Log(string message)
    {
        LogInternal(message, ConsoleColor.Cyan);
    }
}