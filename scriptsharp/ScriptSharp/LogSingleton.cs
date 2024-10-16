using System;
using System.IO;

namespace ScriptSharp;

public sealed class LogSingleton
{
    private static LogSingleton _instance;
    private static readonly object Padlock = new object();


    private LogSingleton()
    {
        Initialize();
    }
    
    private static void Initialize()
    {
        Directory.CreateDirectory(Config.logPath);
        File.WriteAllText(Config.logFilePath, string.Empty);
    }

    public static LogSingleton Get
    {
        get
        {
            if (_instance != null) return _instance;
            lock (Padlock)
            {
                _instance ??= new LogSingleton();
            }

            return _instance;
        }
    }

    public void LogAndWriteLine(string message)
    {
        Console.WriteLine(message);
        using StreamWriter writer = new(Config.logFilePath, true);
        writer.WriteLine($"{DateTime.Now}: {message}");
    }
}