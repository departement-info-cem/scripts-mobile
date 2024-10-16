using System;
using System.IO;

namespace ScriptSharp;

public sealed class LogSingleton
{
    private static LogSingleton _instance;
    private static readonly object Padlock = new object();

    private StreamWriter writer;

    private LogSingleton()
    {
        Initialize();
    }

    private void Initialize()
    {
        Directory.CreateDirectory(Config.LogPath);
        File.WriteAllText(Config.LogFilePath, string.Empty);
        writer = new(Config.LogFilePath, true);
        AppDomain.CurrentDomain.ProcessExit += (s, e) => writer.Close();
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
        writer.WriteLine($"{DateTime.Now}: {message}");
    }
}