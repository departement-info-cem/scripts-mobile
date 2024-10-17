using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public static class ScriptRider
{
    public static async Task HandleRiderAsync()
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

        await UtilsRider.StartRider();
        LogSingleton.Get.LogAndWriteLine("     FAIT Installation de Rider complète");
    }
}