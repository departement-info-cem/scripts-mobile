using System;
using System.IO;

namespace ScriptSharp;

public sealed class LogSingleton
{
    private static LogSingleton _instance;
    private static readonly object Padlock = new();


    private LogSingleton()
    {
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