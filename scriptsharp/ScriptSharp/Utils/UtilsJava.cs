using System;
using System.IO;
using System.Threading.Tasks;

namespace ScriptSharp;

public class UtilsJava
{

    public static async Task InstallJava(string version)
    {
        LogSingleton.Get.LogAndWriteLine("Installation de Java Dev Kit");
        Utils.RemoveFromPath("C:\\Program Files\\Amazon Corretto\\jdk1.8.0_412\\bin", EnvironmentVariableTarget.Machine); // TODO FIX TEMPORAIRE POUR E25. NE FONCTIONNERA PLUS EN A25
        await Utils.CopyFileFromNetworkShareAsync(
            Path.Combine(Config.LocalCache, version + ".7z"),
            Path.Combine(Config.LocalTemp, version + ".7z"));
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string destinationFolder = Path.Combine(desktopPath, version);
        await Utils.Unzip7ZFileAsync(Path.Combine(Config.LocalTemp, version + ".7z"), destinationFolder);
        string jdkPath = Path.Combine(desktopPath, version);
        DirectoryInfo jdkDirectory = new DirectoryInfo(jdkPath);
        string jdkVersion = jdkDirectory.GetDirectories()[0].Name;
        string javaHome = Path.Combine(jdkPath, jdkVersion);
        Utils.AddToPath(Path.Combine(javaHome, "bin"));
        Utils.SetEnvVariable("JAVA_HOME", javaHome);
        LogSingleton.Get.LogAndWriteLine("    FAIT Installation Java");
    }
}