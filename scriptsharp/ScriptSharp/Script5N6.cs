using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script5N6
{
    public static async Task Handle5N6FlutterAsync()
    {
        Utils.LogAndWriteLine("Installation de 5N6 flutter (et Android Studio plus Intellij)...");
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.localCache, "Sdk.7z"), 
            "Sdk.7z");
        await Task.WhenAll(
            Program.InstallAndroidSDK(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.localCache, ".gradle.7z"), 
                ".gradle.7z"),
            Program.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.localCache, "idea.7z"), 
                "idea.7z"),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.localCache, "android-studio.7z"), 
                "android-studio.7z"));
        await Task.WhenAll(
            Utils.Unzip7zFileAsync(
                ".gradle.7z", 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            Utils.Unzip7zFileAsync(
                "idea.7z", 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea")),
            Program.InstallAndroidStudio(), 
            Program.DownloadRepoKMB(),
            DownloadRepo5N6());
        Utils.CreateDesktopShortcut("IntelliJ", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin", "idea64.exe"));
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin"));
        Utils.RunCommand("studio64.exe installPlugins io.flutter");
        Utils.RunCommand("studio64.exe installPlugins com.github.copilot");
        Utils.RunCommand("studio64.exe installPlugins com.localizely.flutter-intl");
        await Utils.StartAndroidStudio();
        await InstallFlutter();
        Utils.LogAndWriteLine("    FAIT 5N6 Flutter complet");
    }

    private static async Task InstallFlutter()
    {
        Utils.LogAndWriteLine("Installation Flutter démarré");
        // TODO remove this in favor of cache flutter
        await Utils.CopyFileFromNetworkShareAsync(Path.Combine(Config.localCache, "flutter.7z"), "flutter.7z");
        await Utils.Unzip7zFileAsync("flutter.7z", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        // ajouter flutter au path
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "flutter", "bin"));
        // execute "flutter doctor --android-licenses"
        Utils.RunCommand("flutter config --android-sdk "+Utils.GetSDKPath());
        Utils.RunCommand("flutter config --android-studio-dir "+Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "android-studio"));
        Utils.RunCommand("flutter doctor --android-licenses");
        Utils.RunCommand("flutter doctor --verbose");
        Utils.RunCommand("flutter precache");
        Utils.RunCommand("flutter pub global activate devtools");
        // create a fake project to initialize flutter
        Utils.RunCommand("flutter create fake_start;cd fake_start;flutter run");
        Utils.LogAndWriteLine("   FAIT Installation Flutter complet");
    }

    public static async Task Handle5N6FlutterFirebaseAsync()
    {
        Utils.LogAndWriteLine("Installation de 5N6 flutter + firebase...");
        await DownloadRepo5N6();
        Utils.LogAndWriteLine("    FAIT 5N6 Flutter + Firebase fini");
    }

    public static async Task DownloadRepo5N6()
    {
        await Program.DownloadRepo(Config.URL_5N6, "5N6");
    }
}