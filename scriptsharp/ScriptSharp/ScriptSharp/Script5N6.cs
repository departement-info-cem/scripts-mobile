using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class Script5N6
{
    public static async Task Handle5N6FlutterAsync()
    {
        Utils.LogAndWriteLine("Gestion de 5N6 flutter...");
        // Add your specific logic here

        await HandleFlutter();
        await DownloadRepo5N6();
        await Program.DownloadRepoKMB();
        Utils.LogAndWriteLine("5N6 Flutter fini");
    }

    private static async Task HandleFlutter()
    {
        Utils.LogAndWriteLine("Installation Flutter démarré");
        // TODO remove this in favor of cache flutter
        string zipPath = Path.Combine(Config.localCache, "flutter.7z");
        await Utils.CopyFileFromNetworkShareAsync(zipPath, "flutter.7z");
        // execute "flutter doctor --android-licenses"
        Utils.RunCommand("flutter doctor --android-licenses");
        Utils.RunCommand("flutter doctor --verbose");
        Utils.RunCommand("flutter precache");
        Utils.RunCommand("flutter pub global activate devtools");
        // create a fake project to initialize flutter
        Utils.RunCommand("flutter create fake_start");
        // cd to the fake project and run "flutter run"
        Utils.RunCommand("cd fake_start");
        Utils.RunCommand("flutter run");
        Utils.LogAndWriteLine("   FAIT Installation Flutter complet");
    }

    public static async Task Handle5N6FlutterFirebaseAsync()
    {
        Utils.LogAndWriteLine("Installation de 5N6 flutter + firebase...");
        await DownloadRepo5N6();
        Utils.LogAndWriteLine("    FAIT 5N6 Flutter + Firebase fini");
    }

    public static async Task DownloadRepo5N6()
    {
        await Program.DownloadRepo(Config.URL_5N6, "5N6");
    }
}