using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public static class Script3N5
{
    public static async Task Handle3N5AndroidAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation pour 3N5 Android...");
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.LocalCache, "Sdk.7z"), 
            Path.Combine(Config.LocalTemp,"Sdk.7z"));
        await Task.WhenAll(
            UtilsAndroidSdk.InstallAndroidSdk(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.LocalCache, ".gradle.7z"), 
                Path.Combine(Config.LocalTemp,".gradle.7z")),
            UtilsJava.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "android-studio.7z"), 
                Path.Combine(Config.LocalTemp, "android-studio.7z") )
            );
        await Task.WhenAll(
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp,".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            UtilsAndroidStudio.InstallAndroidStudio(), 
            DownloadRepo3N5());
        // start android studio
        await UtilsAndroidStudio.StartAndroidStudio();
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation pour 3N5 Android complet");
    }

    private static async Task DownloadRepo3N5()
    {
        await Utils.DownloadRepo(Config.Url3N5, "3N5");
    }

    /**
     * Sans optimisation .gradle
     * 1 min debut install 
     * 1 min run projet
     * 
     * Performance
     * 1 min  install
     * 1 min  Intellij start
     * 4 min  first start
     *
     * 9h01
     * 9h03 cree projet
     * 9h05 run
     *
     * C'est en fait plus rapide si je n'essaie pas de reprendre un .gradle deja existant
     */
    public static async Task Handle3N5KotlinConsoleAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation de kotlin (console) 3N5...");
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin"));
        await Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "idea.7z"), 
                Path.Combine(Config.LocalTemp, "idea.7z"));
        await Task.WhenAll(
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp, "idea.7z"), 
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                    "idea")
                ),
            UtilsJava.InstallJava() );
        LogSingleton.Get.LogAndWriteLine("Premier gradle build pour constituer le .gradle");
        // install plugins  TODO ? one day?
        // Utils.RunCommand("idea64.exe installPlugins io.flutter");
        // Utils.RunCommand("idea64.exe installPlugins com.github.copilot");
        // Utils.RunCommand("idea64.exe installPlugins com.localizely.flutter-intl");
        await Task.WhenAll(DownloadRepo3N5(), UtilsIntellij.StartIntellij());
        LogSingleton.Get.LogAndWriteLine("IMPORTANT IMPORTANT, Si intellij ou Android Studio vous propose de configurer defender, faites-le et choisissez 'Automatically'");
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation de kotlin (console) 3N5");
    }
}