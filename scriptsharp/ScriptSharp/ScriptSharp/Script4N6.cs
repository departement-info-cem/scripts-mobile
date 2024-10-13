using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script4N6
{
    public static async Task Handle4N6AndroidSpringAsync()
    {
        Utils.LogAndWriteLine("Installation pour 4N6 Android + serveur Spring ...");
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
            Program.HandleAndroidStudio(), 
            Program.DownloadRepoKMB(),
            DownloadRepo4N6());
        // start android studio
        Utils.CreateDesktopShortcut("IntelliJ", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin", "idea64.exe"));
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin"));

        await Utils.StartAndroidStudio();
        Utils.LogAndWriteLine("     FAIT Installation 4N6 Android + serveur Spring ");
    }

    public static async Task DownloadRepo4N6()
    {
        await Program.DownloadRepo(Config.URL_4N6, "4N6");
    }

    public static async Task Handle4N6AndroidAsync()
    {
        Utils.LogAndWriteLine("Installation pour 4N6 Android...");
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
            //Program.DownloadRepoKMB(),
            DownloadRepo4N6());
        // start android studio
        await Utils.StartAndroidStudio();
        Utils.LogAndWriteLine("     FAIT Installation 4N6 Android fini");
    }
}