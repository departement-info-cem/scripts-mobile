using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class UtilsAndroidSdk
{

    public static async Task InstallAndroidSdk()
    {
        LogSingleton.Get.LogAndWriteLine("Installation Android SDK démarré");
        string sdkPath = Utils.GetSdkPath();
        string androidSdkRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Android", "Sdk");
        Utils.AddToPath(Path.Combine(androidSdkRoot, "cmdline-tools", "latest", "bin"));
        Utils.AddToPath(Path.Combine(androidSdkRoot, "emulator"));
        // get the parent directory of the SDK path
        string sdkParentPath = Directory.GetParent(sdkPath)?.FullName;
        await Utils.Unzip7ZFileAsync(Path.Combine(Config.LocalTemp, "Sdk.7z"), sdkParentPath);
        // Add environment variables
        Utils.SetEnvironmentVariable("ANDROID_SDK_ROOT", androidSdkRoot);
        Utils.SetEnvironmentVariable("ANDROID_HOME", androidSdkRoot);

        // Append to PATH

        LogSingleton.Get.LogAndWriteLine("    FAIT Installation Android SDK complet");
    }
}