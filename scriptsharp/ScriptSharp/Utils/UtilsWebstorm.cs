using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public static class UtilsWebstorm
{
    public static string PathToWebstorm()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "webstorm", "bin", "webstorm64.exe");
    }
    public static Task StartWebstorm()
    {
        LogSingleton.Get.LogAndWriteLine("Démarrage de Webstorm");
        string path = PathToWebstorm();
        Utils.CreateDesktopShortcut("Webstorm", path);
        if (File.Exists(path))
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
        else
        {
            LogSingleton.Get.LogAndWriteLine("Webstorm n'est pas installé");
        }
        return Task.CompletedTask;
    }
}