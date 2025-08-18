using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class UtilsIntellij
{

    public static string PathToIntellij()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "idea", "bin", "idea64.exe");
    }
    // public static Task StartIntellij()
    // {
    //     // start android studio
    //     LogSingleton.Get.LogAndWriteLine("Démarrage d'Intellij IDEA");
    //     string path = PathToIntellij();
    //     Utils.CreateDesktopShortcut("Intellij", path);
    //     if (File.Exists(path))
    //     {
    //         ProcessStartInfo processStartInfo = new ProcessStartInfo
    //         {
    //             FileName = path,
    //             UseShellExecute = true
    //         };
    //         Process.Start(processStartInfo);
    //     }
    //     else
    //     {
    //         LogSingleton.Get.LogAndWriteLine("Intellij n'est pas installé");
    //     }
    //     return Task.CompletedTask;
    // }
}