using System;
using System.IO;

namespace ScriptSharp;

public static class Config
{
    // TODO replace for 3M5 A26
    public const string Url3N5 = "https://github.com/departement-info-cem/3N5-Prog3/archive/refs/heads/main.zip";
    public const string Url4N6 = "https://github.com/departement-info-cem/4N6-Mobile/archive/refs/heads/master.zip";
    public const string Url5N6 = "https://github.com/departement-info-cem/5N6-mobile-2/archive/refs/heads/main.zip";
    public const string UrlKmb = "https://github.com/departement-info-cem/KickMyB-Server/archive/refs/heads/main.zip";
    
    public const string CachePath  = @"\\ed5depinfo\Logiciels\Android\cache\";
    public const string LocalCache = @"\\ed5depinfo\Logiciels\Android\cache\";

    //create a temp folder on the Destkop
    public static readonly string LocalTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    
    public static readonly string LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log");
    public static readonly string LogFilePath = Path.Combine(LogPath, "installation-log.txt");

    public const string StudioUrl = "https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2025.2.2.8/android-studio-2025.2.2.8-windows.zip";
    
    // Flutter
    public const string FlutterSdk = "https://storage.googleapis.com/flutter_infra_release/releases/stable/windows/flutter_windows_3.24.0-stable.zip";
}