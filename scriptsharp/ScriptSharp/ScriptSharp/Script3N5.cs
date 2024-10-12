using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script3N5
{
    public static async Task Handle3N5AndroidAsync()
    {
        Utils.LogAndWriteLine("Gestion de 3N5 Android...");
        //await Program.HandleAndroidSDK();
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
        await Utils.Unzip7zFileAsync("idea.7z", destinationFolder);
        //AddToPathEnvironmentVariable("C:\\Program Files\\JetBrains\\idea\\bin");
        Utils.CreateDesktopShortcut("IntelliJ3N5", Path.Combine(desktopPath, "idea", "bin", "idea64.exe"));
        await Utils.InstallGradleAsync("8.10.2",".");
        // then create a directory in C:\EspaceLabo\fakotlin
        Directory.CreateDirectory("C:\\EspaceLabo\\fakotlin");
        // mv working directory to C:\EspaceLabo\fakotlin
        Directory.SetCurrentDirectory("C:\\EspaceLabo\\fakotlin");
        // execute gradle init --type kotlin-application --dsl kotlin --test-framework kotlintest --package ca.cem --project-name fake-kotlin  --no-split-project  --java-version 17
        Utils.RunCommand("gradle init --type kotlin-application --dsl kotlin --test-framework kotlintest --package ca.cem --project-name fake-kotlin  --no-split-project  --java-version 17");
        Utils.RunCommand("gradle run");

        await Script3N5.DownloadRepo3N5();
        Utils.LogAndWriteLine("Installation de kotlin (console) 3N5");
    }
}