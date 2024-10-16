using System;
using System.IO;

namespace ScriptSharp;

public class Config
{
    public static string URL_3N5 = "https://github.com/departement-info-cem/3N5-Prog3/archive/refs/heads/main.zip";
    public static string URL_4N6 = "https://github.com/departement-info-cem/4N6-Mobile/archive/refs/heads/master.zip";
    public static string URL_5N6 = "https://github.com/departement-info-cem/5N6-mobile-2/archive/refs/heads/main.zip";
    public static string URL_KMB = "https://github.com/departement-info-cem/KickMyB-Server/archive/refs/heads/main.zip";
    
    public static string cachePath  = "\\\\ed5depinfo\\Logiciels\\Android\\scripts\\cachecache\\";
    public static string localCache = "\\\\ed5depinfo\\Logiciels\\Android\\scripts\\cachecache\\";

    //create a temp folder on the Destkop
    public static string localTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    
    public static string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log");
    public static string logFilePath = Path.Combine(logPath, "installation-log.txt");
    
    public static string STUDIO_URL =
        "https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2024.2.1.10/android-studio-2024.2.1.10-windows.zip";

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

}