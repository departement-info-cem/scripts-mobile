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
        await Utils.CopyFileFromNetworkShareAsync( Path.Combine(Config.localCache, "Sdk.7z"), "Sdk.7z");
        await Task.WhenAll(
            Program.HandleAndroidSDK(), 
            Program.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(Path.Combine(Config.localCache, "android-studio.7z"), "android-studio.7z"));
        await Task.WhenAll(
            Program.HandleAndroidStudio(), 
            DownloadRepo3N5());
        // start android studio
        await Utils.StartAndroidStudio();
        Utils.LogAndWriteLine("3N5 Android fini");
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
        Utils.CreateDesktopShortcut("IntelliJ3N5", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin", "idea64.exe"));
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin"));
        Utils.LogAndWriteLine("Premier gradle build pour constituer le .gradle");
        await Task.WhenAll(DownloadRepo3N5(), Utils.StartIntellij());
        Utils.LogAndWriteLine("IMPORTANT IMPORTANT, Si intellij vous propose de configurer defender automatique, faites le");
        Utils.LogAndWriteLine("Installation de kotlin (console) 3N5");
    }
}