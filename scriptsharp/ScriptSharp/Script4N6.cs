using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public static class Script4N6
{
    public static async Task Handle4N6AndroidSpringAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation pour 4N6 Android + serveur Spring ...");
        Utils.AddToPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea", "bin"));
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.LocalCache, "Sdk.7z"), 
            Path.Combine(Config.LocalTemp,"Sdk.7z"));
        await Task.WhenAll(
            Program.InstallAndroidSdk(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.LocalCache, ".gradle.7z"), 
                Path.Combine(Config.LocalTemp,".gradle.7z")),
            Program.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "idea.7z"), 
                Path.Combine(Config.LocalTemp,"idea.7z")),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "android-studio.7z"), 
                Path.Combine(Config.LocalTemp,"android-studio.7z")));
        await Task.WhenAll(
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp,".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp,"idea.7z"), 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "idea")),
            Program.InstallAndroidStudio(), 
            Program.DownloadRepoKmb(),
            DownloadRepo4N6());
        // start android studio
        Utils.CreateDesktopShortcut("IntelliJ", Program.PathToIntellij());
        
        await Utils.StartAndroidStudio();
        Utils.StartKmb();
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation 4N6 Android + serveur Spring ");
    }


    private static async Task DownloadRepo4N6()
    {
        await Program.DownloadRepo(Config.Url4N6, "4N6");
    }

    public static async Task Handle4N6AndroidAsync()
    {
        LogSingleton.Get.LogAndWriteLine("Installation pour 4N6 Android...");
        await Utils.CopyFileFromNetworkShareAsync( 
            Path.Combine(Config.LocalCache, "Sdk.7z"), 
            Path.Combine(Config.LocalTemp, "Sdk.7z")  );
        await Task.WhenAll(
            Program.InstallAndroidSdk(), 
            Utils.CopyFileFromNetworkShareAsync( 
                Path.Combine(Config.LocalCache, ".gradle.7z"), 
                Path.Combine(Config.LocalTemp, ".gradle.7z")),
            Program.InstallJava(),
            Utils.CopyFileFromNetworkShareAsync(
                Path.Combine(Config.LocalCache, "android-studio.7z"), 
                Path.Combine(Config.LocalTemp, "android-studio.7z")));
        await Task.WhenAll(
            Utils.Unzip7ZFileAsync(
                Path.Combine(Config.LocalTemp, ".gradle.7z"), 
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
            Program.InstallAndroidStudio(), 
            //Program.DownloadRepoKMB(),
            DownloadRepo4N6());
        // start android studio
        await Utils.StartAndroidStudio();
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation 4N6 Android fini");
    }
}