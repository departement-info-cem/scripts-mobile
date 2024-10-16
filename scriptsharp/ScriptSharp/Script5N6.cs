using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script5N6
{
    public static async Task Handle5N6FlutterAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation de 5N6 flutter (et Android Studio plus Intellij)...");
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin"));
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.LocalCache, "Sdk-Android-Flutter.7z"), 
            Path.Combine(Config.LocalTemp, "Sdk.7z") );
        await Task.WhenAll(
            Program.InstallAndroidSdk(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.LocalCache, ".gradle-Android-Flutter.7z"), 
                Path.Combine(Config.LocalTemp, ".gradle.7z")),
            Program.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "idea.7z"), 
                Path.Combine(Config.LocalTemp, "idea.7z")),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "android-studio.7z"), 
                Path.Combine(Config.LocalTemp, "android-studio.7z")));
        await Task.WhenAll(
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp, ".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp, "idea.7z"), 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea")),
            Program.InstallAndroidStudio(), 
            Program.DownloadRepoKmb(),
            DownloadRepo5N6());
        
        Utils.RunCommand(Program.PathToAndroidStudio() + " installPlugins io.flutter");
        Utils.RunCommand(Program.PathToAndroidStudio() + " installPlugins com.github.copilot");
        Utils.RunCommand(Program.PathToAndroidStudio() + " installPlugins com.localizely.flutter-intl");
        await InstallFlutter();
        Utils.StartKmb();
        await Utils.StartAndroidStudio();
        Utils.CreateDesktopShortcut("IntelliJ", Program.PathToIntellij());
        LogSingleton.Get.LogAndWriteLine("    FAIT 5N6 Flutter complet");
    }

    private static async Task InstallFlutter()
    {
        LogSingleton.Get.LogAndWriteLine("Installation Flutter démarré");
        // ajouter flutter au path
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "flutter", "bin"));
        // TODO remove this in favor of cache flutter
        await Utils.CopyFileFromNetworkShareAsync(
            Path.Combine(Config.LocalCache, "flutter.7z"), 
            Path.Combine(Config.LocalTemp, "flutter.7z"));
        await Utils.Unzip7ZFileAsync(
            Path.Combine(Config.LocalTemp, "flutter.7z"), 
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        // execute "flutter doctor --android-licenses"
        Utils.RunCommand(Program.PathToFlutter() +" config --android-sdk "+Utils.GetSdkPath());
        Utils.RunCommand(Program.PathToFlutter() +" config --android-studio-dir "+Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "android-studio"));
        Utils.RunCommand(Program.PathToFlutter() +" doctor --android-licenses");
        Utils.RunCommand(Program.PathToFlutter() +" doctor --verbose");
        Utils.RunCommand(Program.PathToFlutter() +" precache");
        // Utils.RunCommand(Program.PathToFlutter() +" pub global activate devtools"); Obsolete depuis nouvelles version flutter
        // create a fake project to initialize flutter
        Utils.RunCommand(Program.PathToFlutter() +" create fake_start;cd fake_start;flutter run");
        LogSingleton.Get.LogAndWriteLine("   FAIT Installation Flutter complet");
    }

    public static async Task Handle5N6FlutterFirebaseAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation de 5N6 flutter  + firebase ...");
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.LocalCache, "Sdk-Android-Flutter.7z"), 
            Path.Combine(Config.LocalTemp, "Sdk.7z") );
        await Task.WhenAll(
            Program.InstallAndroidSdk(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.LocalCache, ".gradle-Android-Flutter.7z"), 
                Path.Combine(Config.LocalTemp, ".gradle.7z")),
            Program.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "idea.7z"), 
                Path.Combine(Config.LocalTemp, "idea.7z")),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "android-studio.7z"), 
                Path.Combine(Config.LocalTemp, "android-studio.7z")));
        await Task.WhenAll(
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp, ".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp, "idea.7z"), 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea")),
            Program.InstallAndroidStudio(), 
            Program.DownloadRepoKmb(),
            DownloadRepo5N6());
        Utils.CreateDesktopShortcut("IntelliJ", Program.PathToIntellij());
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin"));
        Utils.RunCommand(Program.PathToAndroidStudio() + " installPlugins io.flutter");
        Utils.RunCommand(Program.PathToAndroidStudio() + " installPlugins com.github.copilot");
        Utils.RunCommand(Program.PathToAndroidStudio() + " installPlugins com.localizely.flutter-intl");
        await InstallFlutter();
        Utils.StartKmb();
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData",
            "Local", "Pub", "Cache", "bin"));
        Utils.RunCommand("npm install -g firebase-tools");
        Utils.RunCommand("dart pub global activate flutterfire_cli");
        await Utils.StartAndroidStudio();
        LogSingleton.Get.LogAndWriteLine("    FAIT 5N6 Flutter + firebase complet");
    }

    public static async Task DownloadRepo5N6()
    {
        await Program.DownloadRepo(Config.Url5N6, "5N6");
    }
}