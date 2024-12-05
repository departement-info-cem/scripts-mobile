using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public static class ScriptWeb
{
    public static async Task HandleRiderAsync()
    {
        await Task.WhenAll(HandleRider(), HandleWebstorm());
    }

    private static async Task HandleRider()
    {
        LogSingleton.Get.LogAndWriteLine("Installation de Rider...");

        await Utils.CopyFileFromNetworkShareAsync(
            Path.Combine(Config.LocalCache, "rider.7z"),
            Path.Combine(Config.LocalTemp, "rider.7z"));

        await Utils.Unzip7ZFileAsync(
            Path.Combine(Config.LocalTemp, "rider.7z"),
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "rider")
            );

        Utils.RunCommand(UtilsRider.PathToRider() + " installPlugins com.github.copilot");

        await UtilsRider.StartRider();
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation de Rider complète");
    }

    private static async Task HandleWebstorm()
    {
        LogSingleton.Get.LogAndWriteLine("Installation de Webstorm...");

        await Utils.CopyFileFromNetworkShareAsync(
            Path.Combine(Config.LocalCache, "webstorm.7z"),
            Path.Combine(Config.LocalTemp, "webstorm.7z"));

        await Utils.Unzip7ZFileAsync(
            Path.Combine(Config.LocalTemp, "webstorm.7z"),
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "webstorm")
            );

        Utils.RunCommand(UtilsWebstorm.PathToWebstorm() + " installPlugins com.github.copilot");

        await UtilsWebstorm.StartWebstorm();
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation de Webstorm complète");
    }
}