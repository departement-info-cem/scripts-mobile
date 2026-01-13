using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script5N6
{
    public static async Task Handle5N6FlutterAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation de 5N6 flutter (et Android Studio)...");
        Utils.RunCommand("git config --global --add safe.directory '*'");
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.LocalCache, "Sdk-Android-Flutter.7z"), 
            Path.Combine(Config.LocalTemp, "Sdk.7z") );
        await Task.WhenAll(
            UtilsAndroidSdk.InstallAndroidSdk(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.LocalCache, ".gradle-Android-Flutter.7z"), 
                Path.Combine(Config.LocalTemp, ".gradle.7z")),
            //UtilsJava.InstallJava("jdk"),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "android-studio.7z"), 
                Path.Combine(Config.LocalTemp, "android-studio.7z")));
        await Task.WhenAll(
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp, ".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            UtilsAndroidStudio.InstallAndroidStudio(), 
            Utils.DownloadRepoKmb(),
            DownloadRepo5N6());
        
        Utils.InstallASPlugin("Dart");
        Utils.InstallASPlugin("io.flutter");
        Utils.InstallASPlugin("com.github.copilot");
        Utils.InstallASPlugin("com.localizely.flutter-intl");
        await UtilsFlutter.InstallFlutter();
        await UtilsAndroidStudio.StartAndroidStudio();
        LogSingleton.Get.LogAndWriteLine("    FAIT 5N6 Flutter complet");
    }

    public static async Task Handle5N6FlutterFirebaseAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation de 5N6 flutter + firebase ...");
        UtilsFirebase.InstallFirebase();
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.LocalCache, "Sdk-Android-Flutter.7z"), 
            Path.Combine(Config.LocalTemp, "Sdk.7z") );
        await Task.WhenAll(
            UtilsAndroidSdk.InstallAndroidSdk(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.LocalCache, ".gradle-Android-Flutter.7z"), 
                Path.Combine(Config.LocalTemp, ".gradle.7z")),
            UtilsJava.InstallJava("jdk-flutter"),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "android-studio.7z"), 
                Path.Combine(Config.LocalTemp, "android-studio.7z")));
        await Task.WhenAll(
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp, ".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            UtilsAndroidStudio.InstallAndroidStudio(), 
            Utils.DownloadRepoKmb(),
            DownloadRepo5N6());
        Utils.InstallASPlugin("Dart");
        Utils.InstallASPlugin("io.flutter");
        Utils.InstallASPlugin("com.github.copilot");
        Utils.InstallASPlugin("com.localizely.flutter-intl");
        await UtilsFlutter.InstallFlutter();
        //Utils.StartKMB();
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData",
            "Local", "Pub", "Cache", "bin"));
        UtilsFirebase.InstallFlutterFire();
        await UtilsAndroidStudio.StartAndroidStudio();
        LogSingleton.Get.LogAndWriteLine("    FAIT 5N6 Flutter + firebase complet");
    }

    public static async Task DownloadRepo5N6()
    {
        await Utils.DownloadRepo(Config.Url5N6, "5N6");
    }
}