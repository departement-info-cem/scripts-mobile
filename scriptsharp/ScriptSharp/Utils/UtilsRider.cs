using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class UtilsRider
{
    public static string PathToRider()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "rider", "bin", "rider64.exe");
    }
    public static Task StartRider()
    {
        LogSingleton.Get.LogAndWriteLine("Démarrage de Rider");
        string path = PathToRider();
        Utils.CreateDesktopShortcut("Rider", path);
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
            LogSingleton.Get.LogAndWriteLine("Rider n'est pas installé");
        }
        return Task.CompletedTask;
    }
}