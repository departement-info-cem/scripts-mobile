using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptSharp;

public class CacheCreation
{
    /**
     * 2 ensembles de sdk / .gradle pour android studio kotlin ET android studio flutter
     *
     * Une fois une version de Android Studio, on va
     * - créer un projet avec le wizard
     * - le partir sur un émulateur
     * - quand tout est beau on ferme Android Studio et on supprime le .gradle puis le Sdk
     * - on ouvre le projet et on crée un émulateur avec la version Android de notre choix
     * - on laisse le SDK et le .gradle se remplir en sync compile execute
     * - FERMER TOUS LES PROCESSUS java.exe
     * - on fait le .gradle.7z et le Sdk.7z
     * - on dépose le tout sur ##ed5depinfo
     *
     * Pour Flutter:
     * - on crée un projet et on l'exécute
     * - on ferme l'IDE
     * - on supprime le .gradle
     * - on ouvre le projet et on laisse flutter run
     * - on ferme l'IDE
     * - FERMER TOUS LES PROCESSUS java.exe
     * - on fait .gradle-flutter.7z et Sdk-Android-Flutter.7z
     * - hop sur \\ed5depinfo
     * 
     */
    public static async Task HandleCache()
    {
        LogSingleton.Get.LogAndWriteLine("Creation de la cache ...");
        var downloadTasks = new[]
        {
            Utils.DownloadFileAsync(Config.IDEA_URL, "idea.zip"), 
            Utils.DownloadFileAsync(Config.STUDIO_URL, "studio.zip"), 
            Utils.DownloadFileAsync(Config.FLUTTER_SDK, "flutter.zip"), 
            Utils.DownloadFileAsync(Config.CORRETTO_URL, "corretto.zip"), 
            Utils.DownloadFileAsync(Config.FLUTTER_PLUGIN_URL_STUDIO, "plugin-flutter-android-studio.zip"), 
            Utils.DownloadFileAsync(Config.DART_PLUGIN_URL_STUDIO, "plugin-dart-android-studio.zip"), 
            Utils.DownloadFileAsync(Config.FLUTTER_INTL_PLUGIN_URL_STUDIO, "plugin-flutter-intl-android-studio.zip")
        };

        await Task.WhenAll(downloadTasks);

        string tempcache = "."; // Replace with the actual path to temp cache
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        // delete "android-studio" folder if it exists
        if (Directory.Exists(Path.Combine(tempcache, "android-studio")))
        {
            Directory.Delete(Path.Combine(tempcache, "android-studio"), true);
        }
        LogSingleton.Get.LogAndWriteLine("Creation du Android Studio avec plugins");
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
        LogSingleton.Get.LogAndWriteLine("Conversions des zip en 7z");
        var convertTasks = new[]
        {
            Utils.ConvertZipTo7zAsync("idea.zip", "idea.7z"),
            Utils.ConvertZipTo7zAsync("corretto.zip", "jdk.7z"),
            Utils.ConvertZipTo7zAsync("flutter.zip", "flutter.7z")
        };
        await Task.WhenAll(convertTasks);
        LogSingleton.Get.LogAndWriteLine("Copie des 7z dans le cache " + Config.cachePath);
        // copy the 7z files to the cache folder
        File.Copy("idea.7z", Path.Combine(Config.cachePath, "idea.7z"), true);
        File.Copy("idea.zip", Path.Combine(Config.cachePath, "idea.zip"), true);
        File.Copy("jdk.7z", Path.Combine(Config.cachePath, "jdk.7z"), true);
        File.Copy("corretto.zip", Path.Combine(Config.cachePath, "jdk.zip"), true);
        File.Copy("flutter.7z", Path.Combine(Config.cachePath, "flutter.7z"), true);
        File.Copy("flutter.zip", Path.Combine(Config.cachePath, "flutter.zip"), true);
        File.Copy("android-studio.7z", Path.Combine(Config.cachePath, "android-studio.7z"), true);
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
        LogSingleton.Get.LogAndWriteLine("Creation de la cache finie");
    }
}