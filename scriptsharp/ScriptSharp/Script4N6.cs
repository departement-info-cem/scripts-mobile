using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script4N6
{
    public static async Task Handle4N6AndroidSpringAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation pour 4N6 Android + serveur Spring ...");
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin"));
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
                Path.Combine(Config.localCache, "idea.7z"), 
                Path.Combine(Config.localTemp,"idea.7z")),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.localCache, "android-studio.7z"), 
                Path.Combine(Config.localTemp,"android-studio.7z")));
        await Task.WhenAll(
            Utils.Unzip7zFileAsync(
                Path.Combine(Config.localTemp,".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            Utils.Unzip7zFileAsync(
                Path.Combine(Config.localTemp,"idea.7z"), 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea")),
            Program.InstallAndroidStudio(), 
            Program.DownloadRepoKMB(),
            DownloadRepo4N6());
        // start android studio
        Utils.CreateDesktopShortcut("IntelliJ", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin", "idea64.exe"));
        
        await Utils.StartAndroidStudio();
        Utils.StartKMB();
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation 4N6 Android + serveur Spring ");
    }

    
    public static async Task DownloadRepo4N6()
    {
        await Program.DownloadRepo(Config.URL_4N6, "4N6");
    }

    public static async Task Handle4N6AndroidAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation pour 4N6 Android...");
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.localCache, "Sdk.7z"), 
            Path.Combine(Config.localTemp, "Sdk.7z")  );
        await Task.WhenAll(
            Program.InstallAndroidSDK(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.localCache, ".gradle.7z"), 
                Path.Combine(Config.localTemp, ".gradle.7z")),
            Program.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.localCache, "android-studio.7z"), 
                Path.Combine(Config.localTemp, "android-studio.7z")));
        await Task.WhenAll(
            Utils.Unzip7zFileAsync(
                Path.Combine(Config.localTemp, ".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            Program.InstallAndroidStudio(), 
            //Program.DownloadRepoKMB(),
            DownloadRepo4N6());
        // start android studio
        await Utils.StartAndroidStudio();
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation 4N6 Android fini");
    }
}