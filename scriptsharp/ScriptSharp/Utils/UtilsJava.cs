using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class UtilsJava
{

    public static async Task InstallJava()
    {
        LogSingleton.Get.LogAndWriteLine("Installation de Java Dev Kit");
        await Utils.CopyFileFromNetworkShareAsync(
            Path.Combine(Config.LocalCache, "jdk.7z"),
            Path.Combine(Config.LocalTemp, "jdk.7z"));
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string destinationFolder = Path.Combine(desktopPath, "jdk");
        await Utils.Unzip7ZFileAsync(Path.Combine(Config.LocalTemp, "jdk.7z"), destinationFolder);
        string jdkPath = Path.Combine(desktopPath, "jdk");
        DirectoryInfo jdkDirectory = new DirectoryInfo(jdkPath);
        string jdkVersion = jdkDirectory.GetDirectories()[0].Name;
        string javaHome = Path.Combine(jdkPath, jdkVersion);
        Utils.AddToPath(Path.Combine(javaHome, "bin"));
        Utils.SetEnvVariable("JAVA_HOME", javaHome);
        LogSingleton.Get.LogAndWriteLine("    FAIT Installation Java");
    }
}