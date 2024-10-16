using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script3N5
{
    public static async Task Handle3N5AndroidAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation pour 3N5 Android...");
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.localCache, "Sdk.7z"), 
            Path.Combine(Config.localTemp,"Sdk.7z"));
        await Task.WhenAll(
            Program.InstallAndroidSDK(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.localCache, ".gradle.7z"), 
                Path.Combine(Config.localTemp,".gradle.7z")),
            Program.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.localCache, "android-studio.7z"), 
                Path.Combine(Config.localTemp, "android-studio.7z") )
            );
        await Task.WhenAll(
            Utils.Unzip7zFileAsync(
                Path.Combine(Config.localTemp,".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            Program.InstallAndroidStudio(), 
            DownloadRepo3N5());
        // start android studio
        await Utils.StartAndroidStudio();
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation pour 3N5 Android complet");
    }

    public static async Task DownloadRepo3N5()
    {
        await Program.DownloadRepo(Config.URL_3N5, "3N5");
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
        await Task.WhenAll(
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.localCache, "idea.7z"), 
                Path.Combine(Config.localTemp, "idea.7z"))
        );
        await Task.WhenAll(
            Utils.Unzip7zFileAsync(
                Path.Combine(Config.localTemp, "idea.7z"), 
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                    "idea")
                ),
            Program.InstallJava() );
        LogSingleton.Get.LogAndWriteLine("Premier gradle build pour constituer le .gradle");
        // install plugins  TODO ? one day?
        // Utils.RunCommand("idea64.exe installPlugins io.flutter");
        // Utils.RunCommand("idea64.exe installPlugins com.github.copilot");
        // Utils.RunCommand("idea64.exe installPlugins com.localizely.flutter-intl");
        await Task.WhenAll(DownloadRepo3N5(), Utils.StartIntellij());
        LogSingleton.Get.LogAndWriteLine("IMPORTANT IMPORTANT, Si intellij ou Android Studio vous propose de configurer defender, faites-le et choisissez 'Automatically'");
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation de kotlin (console) 3N5");
    }
}