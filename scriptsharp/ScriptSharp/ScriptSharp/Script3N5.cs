using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script3N5
{
    public static async Task Handle3N5AndroidAsync()
    {
        Utils.LogAndWriteLine("Gestion de 3N5 Android...");
        await Program.HandleAndroidSDK();
        await Program.HandleAndroidStudio();
        await DownloadRepo3N5();
        Utils.LogAndWriteLine("3N5 Android fini");
    }

    public static async Task DownloadRepo3N5()
    {
        await Program.DownloadRepo(Program.URL_3N5, "3N5");
    }

    public static async Task Handle3N5KotlinConsoleAsync()
    {
        Utils.LogAndWriteLine("Installation de kotlin (console) 3N5...");
        string ideaZipPath = Path.Combine(Program.localCache, "idea.7z");
        await Utils.CopyFileFromNetworkShareAsync(ideaZipPath, "idea.7z");
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string destinationFolder = Path.Combine(desktopPath, "idea");
        await Utils.Unzip7zFileAsync(ideaZipPath, destinationFolder);
        //AddToPathEnvironmentVariable("C:\\Program Files\\JetBrains\\idea\\bin");


        await Script3N5.DownloadRepo3N5();
        Utils.LogAndWriteLine("Installation de kotlin (console) 3N5");
    }
}