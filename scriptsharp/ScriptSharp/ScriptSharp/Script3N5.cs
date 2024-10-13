using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script3N5
{
    public static async Task Handle3N5AndroidAsync()
    {
        Utils.LogAndWriteLine("Gestion de 3N5 Android...");
        await Utils.CopyFileFromNetworkShareAsync( Path.Combine(CacheCreation.localCache, "Sdk.7z"), "Sdk.7z");
        await Task.WhenAll(
            Program.HandleAndroidSDK(), 
            Program.HandleAndroidStudio(), 
            DownloadRepo3N5());
        // start android studio
        await Utils.StartAndroidStudio();
        Utils.LogAndWriteLine("3N5 Android fini");
    }

    public static async Task DownloadRepo3N5()
    {
        await Program.DownloadRepo(Program.URL_3N5, "3N5");
    }

    public static async Task Handle3N5KotlinConsoleAsync()
    {
        Utils.LogAndWriteLine("Installation de kotlin (console) 3N5...");
        string ideaZipPath = Path.Combine(CacheCreation.localCache, "idea.7z");
        await Utils.CopyFileFromNetworkShareAsync(ideaZipPath, "idea.7z");
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string destinationFolder = Path.Combine(desktopPath, "idea");
        
        await Task.WhenAll(
            Utils.Unzip7zFileAsync("idea.7z", destinationFolder),
            Utils.InstallGradleAsync("8.10.2",".")
            );
        Utils.CreateDesktopShortcut("IntelliJ3N5", Path.Combine(desktopPath, "idea", "bin", "idea64.exe"));
        // then create a directory in C:\EspaceLabo\fakotlin
        Directory.CreateDirectory("C:\\EspaceLabo\\fakotlin");
        // mv working directory to C:\EspaceLabo\fakotlin
        Directory.SetCurrentDirectory("C:\\EspaceLabo\\fakotlin");
        // execute gradle init --type kotlin-application --dsl kotlin --test-framework kotlintest --package ca.cem --project-name fake-kotlin  --no-split-project  --java-version 17
        Utils.RunCommand("gradle init --type kotlin-application --dsl kotlin --test-framework kotlintest --package ca.cem --project-name fake-kotlin  --no-split-project  --java-version 8 --incubating --overwrite");
        Utils.LogAndWriteLine("Premier gradle build pour constituer le .gradle");
        Utils.RunCommand("gradle run");

        await Task.WhenAll(DownloadRepo3N5(), Utils.StartIntellij());
        Utils.LogAndWriteLine("Installation de kotlin (console) 3N5");
    }
}