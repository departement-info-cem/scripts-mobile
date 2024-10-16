using System;
using System.IO;

namespace ScriptSharp;

public static class Config
{
    public const string Url3N5 = "https://github.com/departement-info-cem/3N5-Prog3/archive/refs/heads/main.zip";
    public const string Url4N6 = "https://github.com/departement-info-cem/4N6-Mobile/archive/refs/heads/master.zip";
    public const string Url5N6 = "https://github.com/departement-info-cem/5N6-mobile-2/archive/refs/heads/main.zip";
    public const string UrlKmb = "https://github.com/departement-info-cem/KickMyB-Server/archive/refs/heads/main.zip";
    
    public const string CachePath  = @"\\ed5depinfo\Logiciels\Android\scripts\cachecache\";
    public const string LocalCache = @"\\ed5depinfo\Logiciels\Android\scripts\cachecache\";

    //create a temp folder on the Destkop
    public static readonly string LocalTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    
    public static readonly string LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log");
    public static readonly string LogFilePath = Path.Combine(LogPath, "installation-log.txt");

    public const string StudioUrl = "https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2024.2.1.10/android-studio-2024.2.1.10-windows.zip";

    public const string FlutterPluginUrlStudio = "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=582965";

    public const string DartPluginUrlStudio = "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=573248";

    public const string FlutterIntlPluginUrlStudio = "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=541419";

    // Android SDK and sdkmanager
    public const string CorrettoUrl = "https://corretto.aws/downloads/latest/amazon-corretto-17-x64-windows-jdk.zip";

    // IntelliJ
    public const string IdeaUrl = "https://download.jetbrains.com/idea/ideaIC-2024.2.1.win.zip";

    // Flutter
    public const string FlutterSdk = "https://storage.googleapis.com/flutter_infra_release/releases/stable/windows/flutter_windows_3.24.0-stable.zip";

}