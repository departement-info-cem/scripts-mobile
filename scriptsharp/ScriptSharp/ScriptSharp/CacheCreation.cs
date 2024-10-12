using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptSharp;

public class CacheCreation
{
    public static string STUDIO_URL =
        "https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2024.1.1.12/android-studio-2024.1.1.12-windows.zip";

    public static string FLUTTER_PLUGIN_URL_STUDIO =
        "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=582965";

    public static string DART_PLUGIN_URL_STUDIO = "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=573248";

    public static string FLUTTER_INTL_PLUGIN_URL_STUDIO =
        "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=541419";

    // Android SDK and sdkmanager
    public static string CORRETTO_URL = "https://corretto.aws/downloads/latest/amazon-corretto-17-x64-windows-jdk.zip";

    // IntelliJ
    public static string IDEA_URL = "https://download.jetbrains.com/idea/ideaIC-2024.2.1.win.zip";

    // Flutter
    public static string FLUTTER_SDK =
        "https://storage.googleapis.com/flutter_infra_release/releases/stable/windows/flutter_windows_3.24.0-stable.zip";

    public static async Task HandleCache()
    {
        Utils.LogAndWriteLine("Creation de la cache ...");
        var cachePath = "\\\\ed5depinfo\\Logiciels\\Android\\scripts\\cachecache\\";
        var downloadTasks = new[]
        {
            Utils.DownloadFileAsync(IDEA_URL, "idea.zip"), 
            Utils.DownloadFileAsync(STUDIO_URL, "studio.zip"), 
            Utils.DownloadFileAsync(FLUTTER_SDK, "flutter.zip"), 
            Utils.DownloadFileAsync(CORRETTO_URL, "corretto.zip"), 
            Utils.DownloadFileAsync(FLUTTER_PLUGIN_URL_STUDIO, "plugin-flutter-android-studio.zip"), 
            Utils.DownloadFileAsync(DART_PLUGIN_URL_STUDIO, "plugin-dart-android-studio.zip"), 
            Utils.DownloadFileAsync(FLUTTER_INTL_PLUGIN_URL_STUDIO, "plugin-flutter-intl-android-studio.zip")
        };

        await Task.WhenAll(downloadTasks);

        string tempcache = "."; // Replace with the actual path to temp cache
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        // delete "android-studio" folder if it exists
        if (Directory.Exists(Path.Combine(tempcache, "android-studio")))
        {
            Directory.Delete(Path.Combine(tempcache, "android-studio"), true);
        }

        Utils.LogAndWriteLine("Creation du Android Studio avec plugins");
        string localTempPath = Path.Combine(tempcache, "studio.zip");
        ZipFile.ExtractToDirectory(localTempPath, Path.Combine(tempcache));

        localTempPath = Path.Combine(tempcache, "plugin-dart-android-studio.zip");
        ZipFile.ExtractToDirectory(localTempPath,
            Path.Combine(tempcache, "android-studio", "plugins"));
        localTempPath = Path.Combine(tempcache, "plugin-flutter-android-studio.zip");
        ZipFile.ExtractToDirectory(localTempPath,
            Path.Combine(tempcache,  "android-studio", "plugins"));
        localTempPath = Path.Combine(tempcache, "plugin-flutter-intl-android-studio.zip");
        ZipFile.ExtractToDirectory(localTempPath,
            Path.Combine(tempcache,"android-studio", "plugins"));
        // create android-studio.7z from the folder with plugins
        await Utils.CompressFolderTo7zAsync("android-studio", "android-studio.7z");

        Utils.LogAndWriteLine("Conversions des zip en 7z");
        var convertTasks = new[]
        {
            Utils.ConvertZipTo7zAsync("idea.zip", "idea.7z"),
            Utils.ConvertZipTo7zAsync("corretto.zip", "jdk.7z"),
            Utils.ConvertZipTo7zAsync("flutter.zip", "flutter.7z")
        };

        await Task.WhenAll(convertTasks);
        Utils.LogAndWriteLine("Copie des 7z dans le cache " + cachePath);
        // copy the 7z files to the cache folder
        File.Copy("idea.7z", Path.Combine(cachePath, "idea.7z"), true);
        File.Copy("jdk.7z", Path.Combine(cachePath, "jdk.7z"), true);
        File.Copy("flutter.7z", Path.Combine(cachePath, "flutter.7z"), true);
        File.Copy("android-studio.7z", Path.Combine(cachePath, "android-studio.7z"), true);
        // get the size of the .gradle folder
        var gradleSize = new DirectoryInfo(Path.Combine(home, ".gradle"))
            .EnumerateFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
        // get the size in MB of the AppData\Local\Android\Sdk folder
        var sdkSize = new DirectoryInfo(Path.Combine(home, "AppData", "Local", "Android", "Sdk"))
            .EnumerateFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
        Console.WriteLine("taille de .gradle: " + gradleSize / 1024 / 1024 + " MB");
        Console.WriteLine("taille de Android SDK: " + sdkSize / 1024 / 1024 + " MB");
        Console.WriteLine(
            "Merci de partir Android Studio  creer un projet et le partir sur un emulateur pour constituer le SDK et le .gradle");
        var s = Console.ReadLine();

        Utils.LogAndWriteLine("Creation de la cache finie");
    }
}