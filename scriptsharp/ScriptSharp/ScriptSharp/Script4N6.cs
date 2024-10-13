using System.Threading.Tasks;

namespace ScriptSharp;

public class Script4N6
{
    public static async Task Handle4N6AndroidSpringAsync()
    {
        Utils.LogAndWriteLine("Installation de 4N6 Android + Spring...");
        await Program.InstallJava();
        await Program.HandleAndroidSDK();
        await Program.HandleAndroidStudio();
        await DownloadRepo4N6();
        await Program.DownloadRepoKMB();
        Utils.LogAndWriteLine("4N6 Android + Spring fini");
    }

    public static async Task DownloadRepo4N6()
    {
        await Program.DownloadRepo(Config.URL_4N6, "4N6");
    }

    public static async Task Handle4N6AndroidAsync()
    {
        Utils.LogAndWriteLine("Gestion de 4N6 Android...");
        await Program.InstallJava();
        // Add your specific logic here
        await Program.HandleAndroidSDK();
        await Program.HandleAndroidStudio();

        var downloadTasks = new[] { Script4N6.DownloadRepo4N6(), Program.DownloadRepoKMB() };
        await Task.WhenAll(downloadTasks);
        Utils.LogAndWriteLine("4N6 Android arrêté");
    }
}