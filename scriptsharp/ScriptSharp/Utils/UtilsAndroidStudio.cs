using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class UtilsAndroidStudio
{

    public static string PathToAndroidStudio()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "android-studio", "bin", "studio64.exe");
    }
    public static async Task InstallAndroidStudio()
    {
        LogSingleton.Get.LogAndWriteLine("Installation Android Studio démarré");
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        Utils.AddToPath(Path.Combine(desktopPath, "android-studio", "bin"));
        string destinationFolder = Path.Combine(desktopPath);
        await Utils.Unzip7ZFileAsync(Path.Combine(Config.LocalTemp, "android-studio.7z"), destinationFolder);
        LogSingleton.Get.LogAndWriteLine("    FAIT Installation Android Studio fini");
    }
}