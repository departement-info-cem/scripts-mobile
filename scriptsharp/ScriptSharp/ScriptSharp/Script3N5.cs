using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ScriptSharp;

public class Script3N5
{
    public static async Task Handle3N5AndroidAsync()
    {
        Utils.LogAndWriteLine("Installation pour 3N5 Android...");
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.localCache, "Sdk.7z"), 
            "Sdk.7z");
        await Task.WhenAll(
            Program.HandleAndroidSDK(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.localCache, ".gradle.7z"), 
                ".gradle.7z"),
            Program.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.localCache, "android-studio.7z"), 
                "android-studio.7z"));
        await Task.WhenAll(
            Utils.Unzip7zFileAsync(
                ".gradle.7z", 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            Program.HandleAndroidStudio(), 
            DownloadRepo3N5());
        // start android studio
        await Utils.StartAndroidStudio();
        Utils.LogAndWriteLine("     FAIT Installation pour 3N5 Android complet");
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
        Utils.LogAndWriteLine("Installation de kotlin (console) 3N5...");
        await Task.WhenAll(
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.localCache, "idea.7z"), 
                "idea.7z")
        );
        await Task.WhenAll(
            Utils.Unzip7zFileAsync(
                "idea.7z", 
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                    "idea")
                ),
            Program.InstallJava() );
        Utils.CreateDesktopShortcut("IntelliJ", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin", "idea64.exe"));
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin"));
        Utils.LogAndWriteLine("Premier gradle build pour constituer le .gradle");
        // install plugins  TODO ? one day?
        // Utils.RunCommand("idea64.exe installPlugins io.flutter");
        await Task.WhenAll(DownloadRepo3N5(), Utils.StartIntellij());
        Utils.LogAndWriteLine("IMPORTANT IMPORTANT, Si intellij ou Android Studio vous propose de configurer defender, faites-le et choisissez 'Automatically'");
        Utils.LogAndWriteLine("     FAIT Installation de kotlin (console) 3N5");
    }
}