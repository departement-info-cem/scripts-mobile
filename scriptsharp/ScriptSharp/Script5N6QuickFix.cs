using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script5N6QuickFix
{
    public static async Task Handle5N6FlutterFirebaseQuickFixAsync()
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
                Path.Combine(Config.LocalCache, "android-studio-quickfix.7z"), 
                Path.Combine(Config.LocalTemp, "android-studio.7z")));
        await Task.WhenAll(
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp, ".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            UtilsAndroidStudio.InstallAndroidStudio(), 
            Utils.DownloadRepoKmb(),
            DownloadRepo5N6());
        Utils.RunCommand(UtilsAndroidStudio.PathToAndroidStudio() + " installPlugins Dart");
        Utils.RunCommand(UtilsAndroidStudio.PathToAndroidStudio() + " installPlugins io.flutter");
        Utils.RunCommand(UtilsAndroidStudio.PathToAndroidStudio() + " installPlugins com.github.copilot");
        Utils.RunCommand(UtilsAndroidStudio.PathToAndroidStudio() + " installPlugins com.localizely.flutter-intl");
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