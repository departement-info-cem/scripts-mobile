using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class UtilsFlutter
{

    public static async Task InstallFlutter()
    {
        LogSingleton.Get.LogAndWriteLine("Installation Flutter démarré");
        // ajouter flutter au path
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "flutter", "bin"));
        await Utils.CopyFileFromNetworkShareAsync(
            Path.Combine(Config.LocalCache, "flutter.7z"), 
            Path.Combine(Config.LocalTemp, "flutter.7z"));
        await Utils.Unzip7ZFileAsync(
            Path.Combine(Config.LocalTemp, "flutter.7z"), 
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        // execute "flutter doctor --android-licenses"
        Utils.RunCommand(PathToFlutter() +" config --android-sdk "+Utils.GetSdkPath());
        Utils.RunCommand(PathToFlutter() +" config --android-studio-dir "+Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "android-studio"));
        Utils.RunCommand(PathToFlutter() +" doctor --android-licenses");
        Utils.RunCommand(PathToFlutter() +" doctor --verbose");
        Utils.RunCommand(PathToFlutter() +" precache");
        // Utils.RunCommand(Program.PathToFlutter() +" pub global activate devtools"); Obsolete depuis nouvelles version flutter
        // create a fake project to initialize flutter
        Utils.RunCommand(PathToFlutter() +" create fake_start;cd fake_start;flutter run");
        LogSingleton.Get.LogAndWriteLine("   FAIT Installation Flutter complet");
    }
    public static string PathToFlutter()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "flutter", "bin", "flutter");
    }
}